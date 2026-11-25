using OpenCvSharp;

namespace FaceRecognition.Services;

// Cascade dosyası yok / bozuk olduğunda fırlatılır
public sealed class FaceDetectorException(string message) : Exception(message);

// Haar cascade ile görüntüdeki yüzleri bulur ve tanıma için yüz örneği hazırlar
public sealed class FaceDetector : IDisposable
{
    // Eğitim ve tanımada kullanılan yüz örneği boyutu (piksel)
    public const int FaceSize = 100;
    public const string CascadeFileName = "haarcascade_frontalface_default.xml";

    // Büyük resimler taramadan önce bu boyuta küçültülür (hız için)
    private const int MaxScanSize = 800;

    private readonly CascadeClassifier _cascade;
    private readonly object _lock = new();

    public FaceDetector(string cascadePath)
    {
        if (!File.Exists(cascadePath))
            throw new FaceDetectorException(
                $"Yüz algılama dosyası bulunamadı:\n{cascadePath}\n\n" +
                $"'{CascadeFileName}' dosyasının uygulama klasöründeki Cascades klasöründe olduğundan emin olun.");

        _cascade = new CascadeClassifier();
        try
        {
            // Dosya bellekten okunur; böylece Türkçe karakterli klasör yolları sorun çıkarmaz
            string xml = File.ReadAllText(cascadePath);
            using var storage = new FileStorage(xml, FileStorage.Modes.Read | FileStorage.Modes.Memory);
            var node = storage.GetFirstTopLevelNode();
            if (node == null || !_cascade.Read(node) || _cascade.Empty())
                throw new FaceDetectorException($"Yüz algılama dosyası geçersiz veya bozuk:\n{cascadePath}");
        }
        catch (Exception ex)
        {
            _cascade.Dispose();
            if (ex is FaceDetectorException)
                throw;
            throw new FaceDetectorException($"Yüz algılama dosyası okunamadı:\n{cascadePath}\n\n{ex.Message}");
        }
    }

    // Renkli (BGR / BGRA) veya gri görüntüyü gri tonlamaya çevirir
    public static Mat ToGray(Mat image) => image.Channels() switch
    {
        1 => image.Clone(),
        4 => image.CvtColor(ColorConversionCodes.BGRA2GRAY),
        _ => image.CvtColor(ColorConversionCodes.BGR2GRAY)
    };

    // Gri görüntüdeki yüzlerin dikdörtgenlerini döndürür
    public Rect[] Detect(Mat gray)
    {
        double scale = Math.Min(1.0, (double)MaxScanSize / Math.Max(gray.Width, gray.Height));
        using var work = scale < 1.0 ? gray.Resize(new OpenCvSharp.Size(), scale, scale, InterpolationFlags.Area) : gray.Clone();
        Cv2.EqualizeHist(work, work);

        // Çok küçük bölgeler yanlış algılamaya yol açtığından en küçük yüz boyutu sınırlanır
        int minFace = Math.Max(30, (int)(Math.Min(work.Width, work.Height) * 0.08));
        Rect[] faces;
        lock (_lock)
        {
            faces = _cascade.DetectMultiScale(work, 1.1, 5, HaarDetectionTypes.ScaleImage,
                new OpenCvSharp.Size(minFace, minFace));
        }

        if (scale < 1.0)
        {
            // Küçültülmüş görüntüdeki konumlar asıl boyuta geri ölçeklenir
            for (int i = 0; i < faces.Length; i++)
            {
                var f = faces[i];
                faces[i] = new Rect((int)(f.X / scale), (int)(f.Y / scale), (int)(f.Width / scale), (int)(f.Height / scale));
            }
        }
        return faces;
    }

    // Yüz bölgesini kesip sabit boyuta getirir ve ışık farkını azaltmak için histogram eşitler
    public static Mat ExtractFace(Mat gray, Rect face)
    {
        var roi = face & new Rect(0, 0, gray.Width, gray.Height);
        using var crop = new Mat(gray, roi);
        var result = new Mat();
        Cv2.Resize(crop, result, new OpenCvSharp.Size(FaceSize, FaceSize), 0, 0, InterpolationFlags.Area);
        Cv2.EqualizeHist(result, result);
        return result;
    }

    public void Dispose() => _cascade.Dispose();
}
