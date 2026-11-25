using FaceRecognition.Models;
using FaceRecognition.Services;
using FaceRecognition.UI;

namespace FaceRecognition;

// Eğitim / veri ekleme ekranı: kameradan yüz örneği toplama, etiket listesi, model eğitimi
public partial class TrainingForm : Form
{
    private const int SamplesPerSession = 25;
    private const int MinSamplesPerLabel = 10;
    private static readonly TimeSpan SampleInterval = TimeSpan.FromMilliseconds(200);

    private readonly CameraService _camera;
    private readonly FrameAnalyzer _analyzer;
    private readonly DatasetService _dataset;
    private readonly FaceRecognizerService _recognizer;

    private volatile string? _collectLabel;   // null değilse örnek toplanıyor
    private int _collected;
    private DateTime _lastSample;             // yalnızca kamera iş parçacığında kullanılır
    private int _framePending;
    private bool _cameraStarting;
    private bool _training;

    public TrainingForm(CameraService camera, FrameAnalyzer analyzer, DatasetService dataset, FaceRecognizerService recognizer)
    {
        InitializeComponent();
        _camera = camera;
        _analyzer = analyzer;
        _dataset = dataset;
        _recognizer = recognizer;

        progressCollect.Maximum = SamplesPerSession;
        progressCollect.BarColor = Theme.Success;
        btnCollect.Text = $"Örnek Topla ({SamplesPerSession})";
        lblCollectCount.Text = $"0 / {SamplesPerSession}";
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        _camera.FrameReady += Camera_FrameReady;
        _camera.RunningChanged += Camera_RunningChanged;

        RefreshLabels();
        UpdateControls();
        SetCollectStatus(_camera.IsRunning
            ? "Hazır. Etiket adını yazıp \"Örnek Topla\"ya basın."
            : "Kamera kapalı. \"Örnek Topla\" kamerayı otomatik başlatır.");
    }

    #region Örnek toplama

    private async void btnCollect_Click(object? sender, EventArgs e)
    {
        string label;
        try
        {
            label = DatasetService.ValidateLabel(txtLabel.Text);
        }
        catch (ArgumentException ex)
        {
            ShowWarning(ex.Message, "Etiket adı");
            txtLabel.Focus();
            return;
        }
        txtLabel.Text = label;

        if (!_camera.IsRunning)
        {
            _cameraStarting = true;
            UpdateControls();
            SetCollectStatus("Kamera açılıyor…");
            try
            {
                await _camera.StartAsync();
            }
            catch (CameraException ex)
            {
                SetCollectStatus("Kamera açılamadı.", true);
                ShowWarning(ex.Message, "Kamera");
                return;
            }
            finally
            {
                _cameraStarting = false;
                UpdateControls();
            }
        }

        Volatile.Write(ref _collected, 0);
        progressCollect.Value = 0;
        lblCollectCount.Text = $"0 / {SamplesPerSession}";
        _collectLabel = label;
        UpdateControls();
        SetCollectStatus($"\"{label}\" için örnek toplanıyor…");
    }

    private void btnCancelCollect_Click(object? sender, EventArgs e)
    {
        StopCollecting();
        SetCollectStatus($"Toplama durduruldu ({Volatile.Read(ref _collected)} örnek kaydedildi).");
        RefreshLabels();
    }

    private void StopCollecting()
    {
        _collectLabel = null;
        UpdateControls();
    }

    // Kamera iş parçacığında çalışır: önizleme ve aralıklı örnek kaydı
    private void Camera_FrameReady(object? sender, FrameEventArgs e)
    {
        if (Interlocked.CompareExchange(ref _framePending, 1, 0) != 0)
            return;

        string? label = _collectLabel;
        bool wantSample = label != null &&
                          Volatile.Read(ref _collected) < SamplesPerSession &&
                          DateTime.Now - _lastSample >= SampleInterval;

        AnalyzedFrame frame;
        bool saved;
        try
        {
            frame = _analyzer.CaptureSample(e.Frame, label, wantSample, out saved);
        }
        catch (Exception ex)
        {
            Volatile.Write(ref _framePending, 0);
            this.TryBeginInvoke(() =>
            {
                StopCollecting();
                SetCollectStatus("Örnek kaydedilemedi: " + ex.Message, true);
            });
            return;
        }

        if (saved)
        {
            _lastSample = DateTime.Now;
            Interlocked.Increment(ref _collected);
        }

        bool posted = this.TryBeginInvoke(() =>
        {
            ShowFrame(frame, label);
            Volatile.Write(ref _framePending, 0);
        });
        if (!posted)
            frame.Dispose();
    }

    private void ShowFrame(AnalyzedFrame frame, string? label)
    {
        FaceOverlayRenderer.Draw(frame.Image, frame.Faces, _ => label != null ? Theme.Success : Theme.Primary);
        videoPreview.SetFrame(frame.Image);

        // Toplama bu arada bittiyse ya da iptal edildiyse ilerleme güncellenmez
        if (label == null || _collectLabel != label)
            return;

        int count = Volatile.Read(ref _collected);
        progressCollect.Value = count;
        lblCollectCount.Text = $"{count} / {SamplesPerSession}";

        if (count >= SamplesPerSession)
        {
            StopCollecting();
            SetCollectStatus($"\"{label}\" için {count} örnek eklendi. Diğer etiket için toplayın veya modeli eğitin.");
            RefreshLabels();
            return;
        }

        SetCollectStatus(frame.Faces.Count switch
        {
            0 => "Yüz bulunamadı – kameraya doğru bakın.",
            1 => $"\"{label}\" için örnek toplanıyor… Başınızı hafifçe sağa-sola çevirin.",
            _ => "Karede birden fazla yüz var – yalnızca bir kişi görünmeli."
        }, frame.Faces.Count != 1);
    }

    private void Camera_RunningChanged(object? sender, bool running)
    {
        this.TryBeginInvoke(() =>
        {
            if (!running)
            {
                if (_collectLabel != null)
                {
                    StopCollecting();
                    SetCollectStatus("Kamera kapandığı için toplama durduruldu.", true);
                    RefreshLabels();
                }
                videoPreview.SetFrame(null);
            }
            UpdateControls();
        });
    }

    #endregion

    #region Etiketler ve model

    // Etiket listesini ve model durumunu yeniler
    private void RefreshLabels()
    {
        IReadOnlyList<PersonLabel> labels;
        try
        {
            labels = _dataset.GetLabels();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            SetCollectStatus("Veri seti okunamadı: " + ex.Message, true);
            return;
        }

        string? selected = SelectedLabel;
        lvLabels.BeginUpdate();
        lvLabels.Items.Clear();
        foreach (var label in labels)
        {
            var item = new ListViewItem(label.Name) { Tag = label };
            item.SubItems.Add(label.SampleCount.ToString());
            // Az örnekli etiketler turuncu gösterilir
            if (label.SampleCount < MinSamplesPerLabel)
                item.ForeColor = Theme.Warning;
            item.Selected = label.Name == selected;
            lvLabels.Items.Add(item);
        }
        lvLabels.EndUpdate();

        UpdateModelState(labels);
        UpdateControls();
    }

    private void UpdateModelState(IReadOnlyList<PersonLabel> labels)
    {
        int usable = labels.Count(l => l.SampleCount > 0);
        (string text, Color color) = usable < 2
            ? ($"Eğitim için en az iki etiket gerekli (şu an {usable}).", Theme.Warning)
            : !_recognizer.IsTrained
                ? ("Model henüz eğitilmedi. \"Modeli Eğit\"e basın.", Theme.Warning)
                : !_recognizer.MatchesDataset(labels)
                    ? ("Veri seti değişti; modeli yeniden eğitin.", Theme.Warning)
                    : ($"Model güncel: {_recognizer.Labels.Count} etiket, {_recognizer.SampleCount} örnek.", Theme.Success);
        lblModelState.Text = text;
        lblModelState.ForeColor = color;
    }

    private string? SelectedLabel =>
        lvLabels.SelectedItems.Count > 0 ? ((PersonLabel)lvLabels.SelectedItems[0].Tag!).Name : null;

    // Listeden seçilen etikete yeni örnek eklenebilir
    private void lvLabels_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (SelectedLabel is { } name && _collectLabel == null)
            txtLabel.Text = name;
        UpdateControls();
    }

    // Etiket klasörü ve içindeki tüm örnekler onaydan sonra silinir
    private void btnDeleteLabel_Click(object? sender, EventArgs e)
    {
        if (lvLabels.SelectedItems.Count == 0)
            return;
        var label = (PersonLabel)lvLabels.SelectedItems[0].Tag!;

        var answer = MessageBox.Show(this,
            $"\"{label.Name}\" etiketi ve {label.SampleCount} yüz örneği kalıcı olarak silinecek.\n\nDevam edilsin mi?",
            "Etiketi Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        if (answer != DialogResult.Yes)
            return;

        try
        {
            _dataset.DeleteLabel(label.Name);
            SetCollectStatus($"\"{label.Name}\" etiketi silindi.");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException)
        {
            ShowWarning("Etiket silinemedi:\n\n" + ex.Message, "Etiketi Sil");
        }
        if (txtLabel.Text == label.Name)
            txtLabel.Clear();
        RefreshLabels();
    }

    // Veri setini yükleyip LBPH modelini arka planda eğitir
    private async void btnTrain_Click(object? sender, EventArgs e)
    {
        var labels = _dataset.GetLabels().Where(l => l.SampleCount > 0).ToList();
        if (labels.Count < 2)
        {
            ShowWarning("Eğitim için en az iki etiket gerekli.\n\nÖrneğin \"Baki\" ve \"Baki (gözlüklü)\" etiketleri için ayrı ayrı örnek toplayın.", "Modeli Eğit");
            return;
        }
        var few = labels.Where(l => l.SampleCount < MinSamplesPerLabel).Select(l => l.Name).ToList();
        if (few.Count > 0)
        {
            ShowWarning($"Her etiket için en az {MinSamplesPerLabel} örnek gerekli. Az örnekli etiketler:\n\n" +
                        string.Join("\n", few), "Modeli Eğit");
            return;
        }

        _training = true;
        UpdateControls();
        Cursor = Cursors.WaitCursor;
        SetCollectStatus("Model eğitiliyor…");
        try
        {
            await Task.Run(() =>
            {
                using var set = _dataset.LoadTrainingSet();
                _recognizer.Train(set);
            });
            SetCollectStatus($"Model eğitildi: {_recognizer.Labels.Count} etiket, {_recognizer.SampleCount} örnek. Ana pencerede tanıma kullanılabilir.");
        }
        catch (Exception ex) when (ex is ModelException or IOException or UnauthorizedAccessException)
        {
            SetCollectStatus("Model eğitilemedi.", true);
            ShowWarning(ex.Message, "Modeli Eğit");
        }
        finally
        {
            _training = false;
            Cursor = Cursors.Default;
            RefreshLabels();
        }
    }

    #endregion

    #region Yardımcılar

    // Butonları duruma göre aktif / pasif yapar
    private void UpdateControls()
    {
        bool collecting = _collectLabel != null;
        bool busy = collecting || _training || _cameraStarting;

        txtLabel.Enabled = !busy;
        btnCollect.Enabled = !busy;
        btnCancelCollect.Enabled = collecting;
        lvLabels.Enabled = !busy;
        btnDeleteLabel.Enabled = !busy && lvLabels.SelectedItems.Count > 0;
        btnTrain.Enabled = !busy && lvLabels.Items.Count > 0;
    }

    private void SetCollectStatus(string text, bool warning = false)
    {
        lblCollectStatus.Text = text;
        lblCollectStatus.ForeColor = warning ? Theme.Danger : Theme.Text;
    }

    private void ShowWarning(string message, string title) =>
        MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    #endregion

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Eğitim sürerken pencere kapatılmaz
        if (_training)
        {
            e.Cancel = true;
            ShowWarning("Model eğitimi sürüyor, lütfen bitmesini bekleyin.", "Eğitim");
            return;
        }

        _collectLabel = null;
        _camera.FrameReady -= Camera_FrameReady;
        _camera.RunningChanged -= Camera_RunningChanged;
        base.OnFormClosing(e);
    }
}
