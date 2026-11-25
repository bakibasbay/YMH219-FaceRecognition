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
        LoadDetector();
        UpdateControls();
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
            if (!running)
            {
                videoView.SetFrame(null);
                videoView.PlaceholderText = CameraOffText;
                lblFps.Text = "–";
                lblFaceCount.Text = "0";
                SetStatus("Kamera durduruldu");
            }
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

    private void btnOpenImage_Click(object? sender, EventArgs e)
    {
    }

    private void btnTraining_Click(object? sender, EventArgs e)
    {
    }

    private void tglRecognition_CheckedChanged(object? sender, EventArgs e)
    {
        _recognitionEnabled = tglRecognition.Checked;
    }

    private void tglSaveFaces_CheckedChanged(object? sender, EventArgs e)
    {
        _saveFaces = tglSaveFaces.Checked;
    }

    private void tbThreshold_ValueChanged(object? sender, EventArgs e)
    {
    }

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
        lblSource.Text = running ? "Kaynak: Kamera" : string.Empty;
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
