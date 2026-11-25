using FaceRecognition.Models;
using FaceRecognition.Services;
using FaceRecognition.UI;

namespace FaceRecognition;

// Ana pencere: kamera / resim görüntüsü, yüz algılama ve tanıma sonuçları.
// OpenCV işlemleri servislerde yapılır; form yalnızca sonuçları gösterir.
public partial class MainForm : Form
{
    private const string CameraOffText = "Kamera kapalı\r\n\r\n\"Kamera Başlat\" ya da \"Görüntü Seç\" ile başlayın";

    private readonly CameraService _camera = new();
    private readonly DatasetService _dataset;
    private readonly FaceRecognizerService _recognizer;
    private FaceDetector? _detector;
    private FrameAnalyzer? _analyzer;

    private int _framePending;          // 1: önceki kare henüz ekrana basılmadı (yeni kare atlanır)
    private volatile bool _recognitionEnabled = true;
    private volatile bool _saveFaces;
    private bool _cameraStarting;

    private string? _imagePath;         // kamera kapalıyken gösterilen resim
    private bool _imageBusy;
    private bool _imageReanalyze;
    private int _savedFaceTotal;

    public MainForm()
    {
        InitializeComponent();

        // Veri seti ve model uygulamanın çalıştığı klasörde tutulur (yalnızca yerel)
        string root = AppContext.BaseDirectory;
        _dataset = new DatasetService(root);
        _recognizer = new FaceRecognizerService(root);

        _camera.RunningChanged += Camera_RunningChanged;
        _camera.CameraError += Camera_CameraError;
        _camera.FrameReady += Camera_FrameReady;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        tbThreshold.Value = (int)FaceRecognizerService.DefaultThreshold;
        LoadDetector();
        LoadModel();
        UpdateControls();
    }

    // Daha önce eğitilmiş model varsa yüklenir; yoksa tanıma kapalı kalır
    private void LoadModel()
    {
        try
        {
            _recognizer.TryLoad();
        }
        catch (ModelException ex)
        {
            ShowWarning(ex.Message, "Model");
        }
        UpdateModelInfo();
    }

    // Cascade dosyası yüklenemezse kamera ve resim işlemleri kapalı kalır
    private void LoadDetector()
    {
        try
        {
            _detector = new FaceDetector(Path.Combine(AppContext.BaseDirectory, "Cascades", FaceDetector.CascadeFileName));
            _analyzer = new FrameAnalyzer(_detector, _recognizer, _dataset);
        }
        catch (FaceDetectorException ex)
        {
            videoView.PlaceholderText = "Yüz algılama dosyası yüklenemedi.\r\nKamera ve resim işlemleri kullanılamıyor.";
            SetStatus("Yüz algılama dosyası (cascade) bulunamadı", true);
            ShowWarning(ex.Message, "Yüz algılama dosyası");
        }
    }

    #region Kamera

    private async void btnStartCamera_Click(object? sender, EventArgs e)
    {
        _cameraStarting = true;
        _imagePath = null;
        UpdateControls();
        videoView.SetFrame(null);
        videoView.PlaceholderText = "Kamera açılıyor…";
        SetStatus("Kamera açılıyor…");
        try
        {
            await _camera.StartAsync();
            SetStatus("Kamera çalışıyor");
        }
        catch (CameraException ex)
        {
            videoView.PlaceholderText = CameraOffText;
            SetStatus("Kamera açılamadı", true);
            ShowWarning(ex.Message, "Kamera");
        }
        finally
        {
            _cameraStarting = false;
            UpdateControls();
        }
    }

    private async void btnStopCamera_Click(object? sender, EventArgs e)
    {
        btnStopCamera.Enabled = false;
        await _camera.StopAsync();
    }

    // Arka plan iş parçacığından gelir
    private void Camera_RunningChanged(object? sender, bool running)
    {
        this.TryBeginInvoke(() =>
        {
            // Kamera, resim açmak için kapatıldıysa resim ekranda kalır
            if (!running && _imagePath == null)
            {
                videoView.SetFrame(null);
                videoView.PlaceholderText = CameraOffText;
                lblFaceCount.Text = "0";
                SetStatus("Kamera durduruldu");
            }
            if (!running)
                lblFps.Text = "–";
            UpdateControls();
        });
    }

    private void Camera_CameraError(object? sender, string message)
    {
        this.TryBeginInvoke(() =>
        {
            SetStatus(message, true);
            ShowWarning(message, "Kamera");
        });
    }

    // Kamera iş parçacığında çalışır: kare işlenir, sonuç arayüze aktarılır
    private void Camera_FrameReady(object? sender, FrameEventArgs e)
    {
        var analyzer = _analyzer;
        // Arayüz önceki kareyi henüz göstermediyse bu kare atlanır (gecikme birikmez)
        if (analyzer == null || Interlocked.CompareExchange(ref _framePending, 1, 0) != 0)
            return;

        AnalyzedFrame result;
        try
        {
            result = analyzer.Analyze(e.Frame, _recognitionEnabled, _saveFaces);
        }
        catch (Exception ex)
        {
            Volatile.Write(ref _framePending, 0);
            this.TryBeginInvoke(() => SetStatus("Kare işlenemedi: " + ex.Message, true));
            return;
        }

        double fps = e.Fps;
        bool posted = this.TryBeginInvoke(() =>
        {
            ShowResult(result, fps);
            Volatile.Write(ref _framePending, 0);
        });
        if (!posted)
            result.Dispose(); // form kapanıyorsa Bitmap burada serbest bırakılır
    }

    #endregion

    #region Görüntü ve sonuçlar

    // İşlenmiş kareyi yüz dikdörtgenleriyle gösterir; Bitmap'in sahipliği görüntü alanına geçer
    private void ShowResult(AnalyzedFrame result, double? fps)
    {
        FaceOverlayRenderer.Draw(result.Image, result.Faces, ColorOf);
        videoView.SetFrame(result.Image);

        lblFaceCount.Text = result.Faces.Count.ToString();
        lblFps.Text = fps is double f ? f.ToString("0.0") : "–";

        if (result.SavedFaces > 0)
        {
            _savedFaceTotal += result.SavedFaces;
            SetStatus($"{_savedFaceTotal} yüz kaydedildi: {_dataset.DetectedFacesDirectory}");
        }

        // Son tahmin, karedeki en büyük (kameraya en yakın) yüze göre gösterilir
        if (result.PrimaryFace is { } face)
            ShowPrediction(face);
        else if (fps == null)
            ShowPredictionText("Yüz bulunamadı", Theme.Idle, "Güven skoru: –");
    }

    private void ShowPrediction(RecognitionResult face)
    {
        if (!face.IsRecognized)
        {
            string text = _recognizer.IsTrained ? "Tanıma kapalı" : "Model eğitilmemiş";
            ShowPredictionText(text, Theme.Idle, "Güven skoru: –");
            return;
        }

        ShowPredictionText(face.Label, ColorOf(face),
            $"Güven skoru: {face.Confidence:0.0}   (eşik {_recognizer.Threshold:0})");
    }

    private void ShowPredictionText(string label, Color color, string score)
    {
        lblPredictionLabel.Text = label;
        lblPredictionLabel.ForeColor = color;
        pnlPredictionAccent.BackColor = color;
        lblPredictionScore.Text = score;
    }

    // Sağ paneldeki model kartı ve durum çubuğu
    private void UpdateModelInfo()
    {
        if (!_recognizer.IsTrained)
        {
            lblModelStatus.Text = "Model eğitilmemiş.\nEğitim / Veri Ekleme ekranından en az iki etiket için örnek toplayıp modeli eğitin.";
            lblModelStatus.ForeColor = Theme.Warning;
            lblModelInfo.Text = "Model: eğitilmemiş";
            return;
        }

        var labels = _recognizer.Labels;
        bool upToDate = _recognizer.MatchesDataset(_dataset.GetLabels());
        lblModelStatus.Text =
            $"{labels.Count} etiket · {_recognizer.SampleCount} örnek\n" +
            string.Join(", ", labels) + "\n" +
            (upToDate
                ? $"Eğitim: {_recognizer.TrainedAt:dd.MM.yyyy HH:mm}"
                : "Veri seti değişti, modeli yeniden eğitin.");
        lblModelStatus.ForeColor = upToDate ? Theme.Text : Theme.Warning;
        lblModelInfo.Text = $"Model: {labels.Count} etiket, {_recognizer.SampleCount} örnek";
    }

    // Etiket rengi: tanıma yoksa mavi, bilinmiyorsa kırmızı, aksi halde etiketin sırasına göre
    private Color ColorOf(RecognitionResult face)
    {
        if (!face.IsRecognized)
            return Theme.Primary;
        if (!face.IsKnown)
            return Theme.Danger;
        return Theme.LabelColor(IndexOfLabel(face.Label));
    }

    private int IndexOfLabel(string label)
    {
        var labels = _recognizer.Labels;
        for (int i = 0; i < labels.Count; i++)
        {
            if (labels[i] == label)
                return i;
        }
        return -1;
    }

    #endregion

    #region Görüntü seçme

    private async void btnOpenImage_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Görüntü Seç",
            Filter = "Resim dosyaları|*.jpg;*.jpeg;*.png;*.bmp;*.webp;*.tif;*.tiff|Tüm dosyalar|*.*"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        // Resim gösterilirken kamera kapatılır
        if (_camera.IsRunning)
            await _camera.StopAsync();

        _imagePath = dialog.FileName;
        await AnalyzeImageAsync(allowSave: true);
    }

    // Seçili resmi arka planda işler; ayar değişince aynı resim yeniden işlenir
    private async Task AnalyzeImageAsync(bool allowSave)
    {
        var analyzer = _analyzer;
        string? path = _imagePath;
        if (analyzer == null || path == null)
            return;

        // Önceki işlem sürüyorsa bittiğinde bir kez daha çalıştırılır
        if (_imageBusy)
        {
            _imageReanalyze = true;
            return;
        }

        _imageBusy = true;
        try
        {
            do
            {
                _imageReanalyze = false;
                bool recognize = _recognitionEnabled;
                bool save = allowSave && _saveFaces;
                allowSave = false;

                var result = await Task.Run(() => analyzer.AnalyzeFile(path, recognize, save));
                if (_camera.IsRunning || _imagePath != path)
                {
                    result.Dispose(); // bu sırada kamera açıldıysa sonuç gösterilmez
                    return;
                }

                ShowResult(result, null);
                SetStatus($"{Path.GetFileName(path)}: {result.Faces.Count} yüz algılandı");
            } while (_imageReanalyze);
        }
        catch (InvalidImageException ex)
        {
            ClearImage();
            SetStatus("Resim açılamadı", true);
            ShowWarning(ex.Message, "Geçersiz resim");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            ClearImage();
            SetStatus("Resim okunamadı", true);
            ShowWarning("Dosya okunamadı:\n\n" + ex.Message, "Görüntü Seç");
        }
        finally
        {
            _imageBusy = false;
            UpdateControls();
        }
    }

    private void ClearImage()
    {
        _imagePath = null;
        videoView.SetFrame(null);
        lblFaceCount.Text = "0";
    }

    // Kamera kapalı ve ekranda resim varsa ayar değişikliğinden sonra resmi yeniden işler
    private void RefreshStillImage()
    {
        if (!_camera.IsRunning && _imagePath != null)
            _ = AnalyzeImageAsync(allowSave: false);
    }

    #endregion

    // Eğitim ekranı açıkken kameranın karelerini o ekran işler
    private void btnTraining_Click(object? sender, EventArgs e)
    {
        if (_analyzer == null)
            return;

        _camera.FrameReady -= Camera_FrameReady;
        try
        {
            using var form = new TrainingForm(_camera, _analyzer, _dataset, _recognizer);
            form.ShowDialog(this);
        }
        finally
        {
            _camera.FrameReady += Camera_FrameReady;
            Volatile.Write(ref _framePending, 0);
            UpdateModelInfo();
            UpdateControls();
            RefreshStillImage();
        }
    }

    #region Tanıma ayarları

    private void tglRecognition_CheckedChanged(object? sender, EventArgs e)
    {
        _recognitionEnabled = tglRecognition.Checked;
        tglRecognition.Text = tglRecognition.Checked ? "Tanıma açık" : "Tanıma kapalı";
        RefreshStillImage();
    }

    // Açıkken algılanan yüzler saniyede en fazla bir kez DetectedFaces/<tarih>/ klasörüne kaydedilir
    private void tglSaveFaces_CheckedChanged(object? sender, EventArgs e)
    {
        _saveFaces = tglSaveFaces.Checked;
        _savedFaceTotal = 0;
        SetStatus(_saveFaces
            ? "Yüz kaydetme açık: algılanan yüzler saniyede bir DetectedFaces klasörüne kaydedilir"
            : "Yüz kaydetme kapalı");
    }

    // Eşik değiştikçe tahminler anında yeni eşikle değerlendirilir
    private void tbThreshold_ValueChanged(object? sender, EventArgs e)
    {
        _recognizer.Threshold = tbThreshold.Value;
        lblThreshold.Text = $"\"Bilinmiyor\" eşiği: {tbThreshold.Value}";
        RefreshStillImage();
    }

    #endregion

    #region Yardımcılar

    // Butonları duruma göre aktif / pasif yapar
    private void UpdateControls()
    {
        bool ready = _analyzer != null;
        bool running = _camera.IsRunning;

        btnStartCamera.Enabled = ready && !running && !_cameraStarting;
        btnStopCamera.Enabled = running;
        btnOpenImage.Enabled = ready && !_cameraStarting;
        btnTraining.Enabled = ready && !_cameraStarting;

        // Model yokken tanıma ayarları kullanılamaz
        tglRecognition.Enabled = _recognizer.IsTrained;
        tbThreshold.Enabled = _recognizer.IsTrained;
        lblSource.Text = running ? "Kaynak: Kamera"
            : _imagePath != null ? "Kaynak: " + Path.GetFileName(_imagePath)
            : string.Empty;
    }

    private void SetStatus(string text, bool error = false)
    {
        lblStatus.Text = text;
        lblStatus.ForeColor = error ? Theme.Danger : Theme.Text;
    }

    private void ShowWarning(string message, string title) =>
        MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    #endregion

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        // Kamera döngüsü durdurulur, OpenCV kaynakları serbest bırakılır
        _camera.FrameReady -= Camera_FrameReady;
        _camera.Dispose();
        _detector?.Dispose();
        _recognizer.Dispose();
        base.OnFormClosed(e);
    }
}
