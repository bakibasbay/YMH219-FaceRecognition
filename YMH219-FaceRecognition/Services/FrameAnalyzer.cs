using FaceRecognition.Models;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace FaceRecognition.Services;

// Seçilen dosya resim olarak açılamadığında fırlatılır
public sealed class InvalidImageException(string message) : Exception(message);

// Bir kareyi baştan sona işler: yüz algılama, tanıma, yüz kaydetme ve Bitmap'e dönüştürme.
// Formlar OpenCV ile doğrudan uğraşmaz; yalnızca bu sınıfı kullanır.
public sealed class FrameAnalyzer
{
    private const long MaxImageBytes = 50 * 1024 * 1024;
    private static readonly TimeSpan SaveInterval = TimeSpan.FromSeconds(1);

    private readonly FaceDetector _detector;
    private readonly FaceRecognizerService _recognizer;
    private readonly DatasetService _dataset;
    private DateTime _lastSave = DateTime.MinValue;

    public FrameAnalyzer(FaceDetector detector, FaceRecognizerService recognizer, DatasetService dataset)
    {
        _detector = detector;
        _recognizer = recognizer;
        _dataset = dataset;
    }

    // Kameradan gelen kareyi işler (tanıma model eğitilmişse yapılır)
    public AnalyzedFrame Analyze(Mat frame, bool recognize, bool saveFaces)
    {
        using var gray = FaceDetector.ToGray(frame);
        var rects = _detector.Detect(gray);
        bool canRecognize = recognize && _recognizer.IsTrained;

        var faces = new List<RecognitionResult>(rects.Length);
        foreach (var rect in rects)
        {
            if (canRecognize)
            {
                using var face = FaceDetector.ExtractFace(gray, rect);
                var (label, confidence) = _recognizer.Predict(face);
                faces.Add(new RecognitionResult(ToRectangle(rect), label, confidence,
                    label != RecognitionResult.UnknownLabel));
            }
            else
            {
                faces.Add(RecognitionResult.Detected(ToRectangle(rect)));
            }
        }

        int saved = saveFaces ? SaveDetectedFaces(frame, rects) : 0;
        return new AnalyzedFrame(frame.ToBitmap(), faces, saved);
    }

    // Diskten seçilen resmi açıp işler
    public AnalyzedFrame AnalyzeFile(string path, bool recognize, bool saveFaces)
    {
        using var image = LoadImage(path);
        return Analyze(image, recognize, saveFaces);
    }

    // Eğitim ekranı için: kareyi gösterir ve istenirse tek yüzlü karelerden örnek kaydeder
    public AnalyzedFrame CaptureSample(Mat frame, string? label, bool save, out bool saved)
    {
        saved = false;
        using var gray = FaceDetector.ToGray(frame);
        var rects = _detector.Detect(gray);

        // Örneğin karışmaması için karede tam olarak bir yüz olmalı
        if (save && label != null && rects.Length == 1)
        {
            using var face = FaceDetector.ExtractFace(gray, rects[0]);
            _dataset.AddSample(label, face);
            saved = true;
        }

        var faces = rects
            .Select(r => label == null
                ? RecognitionResult.Detected(ToRectangle(r))
                : new RecognitionResult(ToRectangle(r), label, null, true))
            .ToList();
        return new AnalyzedFrame(frame.ToBitmap(), faces);
    }

    // Saniyede en fazla bir kez karedeki tüm yüzleri renkli olarak kaydeder
    private int SaveDetectedFaces(Mat frame, Rect[] rects)
    {
        if (rects.Length == 0 || DateTime.Now - _lastSave < SaveInterval)
            return 0;

        _lastSave = DateTime.Now;
        var bounds = new Rect(0, 0, frame.Width, frame.Height);
        foreach (var rect in rects)
        {
            using var crop = new Mat(frame, rect & bounds);
            _dataset.SaveDetectedFace(crop);
        }
        return rects.Length;
    }

    // Dosyayı renkli Mat olarak yükler; resim değilse anlaşılır hata verir
    private static Mat LoadImage(string path)
    {
        var info = new FileInfo(path);
        if (!info.Exists)
            throw new InvalidImageException("Seçilen dosya bulunamadı:\n" + path);
        if (info.Length == 0 || info.Length > MaxImageBytes)
            throw new InvalidImageException("Dosya boyutu geçersiz (0 bayt veya 50 MB'tan büyük).");

        Mat image;
        try
        {
            image = Cv2.ImDecode(File.ReadAllBytes(path), ImreadModes.Color);
        }
        catch (OpenCVException)
        {
            throw new InvalidImageException("Seçilen dosya geçerli bir resim değil:\n" + info.Name);
        }

        if (image.Empty())
        {
            image.Dispose();
            throw new InvalidImageException("Seçilen dosya geçerli bir resim değil veya bozuk:\n" + info.Name);
        }
        return image;
    }

    private static Rectangle ToRectangle(Rect r) => new(r.X, r.Y, r.Width, r.Height);
}
