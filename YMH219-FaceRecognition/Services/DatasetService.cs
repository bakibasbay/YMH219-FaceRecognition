using FaceRecognition.Models;
using OpenCvSharp;

namespace FaceRecognition.Services;

// Eğitim için yüklenen yüz örnekleri (Mat'ler Dispose ile serbest bırakılır)
public sealed class TrainingSet : IDisposable
{
    public List<Mat> Faces { get; } = new();
    public List<int> LabelIds { get; } = new();
    public List<string> LabelNames { get; } = new();

    public void Dispose()
    {
        foreach (var face in Faces)
            face.Dispose();
        Faces.Clear();
    }
}

// Yerel veri seti: Dataset/<etiket>/*.png (gri, 100x100 yüz örnekleri) ve DetectedFaces/ klasörü
public sealed class DatasetService
{
    public const int MaxLabelLength = 40;

    public DatasetService(string rootDirectory)
    {
        DatasetDirectory = Path.Combine(rootDirectory, "Dataset");
        DetectedFacesDirectory = Path.Combine(rootDirectory, "DetectedFaces");
    }

    public string DatasetDirectory { get; }
    public string DetectedFacesDirectory { get; }

    // Etiketleri ve her birinin örnek sayısını döndürür
    public IReadOnlyList<PersonLabel> GetLabels()
    {
        if (!Directory.Exists(DatasetDirectory))
            return Array.Empty<PersonLabel>();

        return Directory.GetDirectories(DatasetDirectory)
            .Select(dir => new PersonLabel(Path.GetFileName(dir),
                Directory.GetFiles(dir, "*.png").Length, dir))
            .OrderBy(l => l.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    // Kullanıcının girdiği etiket adını denetler; geçersizse açıklayıcı mesajla hata verir
    public static string ValidateLabel(string? input)
    {
        // Baştaki/sondaki ve art arda gelen boşluklar tek boşluğa indirilir ("baki  kapalı" → "baki kapalı")
        string label = string.Join(' ', (input ?? string.Empty)
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        if (label.Length == 0)
            throw new ArgumentException("Etiket adı boş olamaz.");
        if (label.Length > MaxLabelLength)
            throw new ArgumentException($"Etiket adı en fazla {MaxLabelLength} karakter olabilir.");
        if (label.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || label.StartsWith('.') || label.EndsWith('.'))
            throw new ArgumentException("Etiket adında \\ / : * ? \" < > | karakterleri bulunamaz ve nokta ile başlayıp bitemez.");
        if (string.Equals(label, RecognitionResult.UnknownLabel, StringComparison.CurrentCultureIgnoreCase) ||
            string.Equals(label, RecognitionResult.FaceLabel, StringComparison.CurrentCultureIgnoreCase))
            throw new ArgumentException($"\"{label}\" uygulama tarafından kullanılan ayrılmış bir addır.");
        return label;
    }

    // Hazırlanmış yüz örneğini etiket klasörüne PNG olarak kaydeder
    public string AddSample(string label, Mat face)
    {
        string dir = Path.Combine(DatasetDirectory, ValidateLabel(label));
        Directory.CreateDirectory(dir);
        string path = UniquePath(dir, DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"));
        WritePng(path, face);
        return path;
    }

    // Etiketi ve tüm örneklerini siler
    public void DeleteLabel(string label)
    {
        string dir = Path.Combine(DatasetDirectory, ValidateLabel(label));
        if (Directory.Exists(dir))
            Directory.Delete(dir, recursive: true);
    }

    // Tüm örnekleri gri ve FaceSize boyutunda yükler; etiket numaraları alfabetik sıraya göre verilir
    public TrainingSet LoadTrainingSet()
    {
        var set = new TrainingSet();
        try
        {
            foreach (var label in GetLabels().Where(l => l.SampleCount > 0))
            {
                int id = set.LabelNames.Count;
                int loaded = 0;
                foreach (string file in Directory.GetFiles(label.FolderPath, "*.png"))
                {
                    var face = ReadGray(file);
                    if (face == null)
                        continue; // okunamayan dosya atlanır

                    if (face.Width != FaceDetector.FaceSize || face.Height != FaceDetector.FaceSize)
                    {
                        var resized = face.Resize(new OpenCvSharp.Size(FaceDetector.FaceSize, FaceDetector.FaceSize));
                        face.Dispose();
                        face = resized;
                    }
                    set.Faces.Add(face);
                    set.LabelIds.Add(id);
                    loaded++;
                }

                if (loaded > 0)
                    set.LabelNames.Add(label.Name);
            }
            return set;
        }
        catch
        {
            set.Dispose();
            throw;
        }
    }

    // "Yüzleri Kaydet" açıkken algılanan yüzü DetectedFaces/<tarih>/ altına kaydeder
    public string SaveDetectedFace(Mat face)
    {
        string dir = Path.Combine(DetectedFacesDirectory, DateTime.Now.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(dir);
        string path = UniquePath(dir, DateTime.Now.ToString("HHmmss_fff"));
        WritePng(path, face);
        return path;
    }

    // Aynı milisaniyede birden fazla kayıt olursa dosya adına sıra numarası eklenir
    private static string UniquePath(string dir, string baseName)
    {
        string path = Path.Combine(dir, baseName + ".png");
        for (int i = 1; File.Exists(path); i++)
            path = Path.Combine(dir, $"{baseName}_{i}.png");
        return path;
    }

    // Kodlama bellekte yapılıp .NET ile yazılır; Türkçe karakterli yollarda da çalışır
    private static void WritePng(string path, Mat image)
    {
        Cv2.ImEncode(".png", image, out byte[] data);
        File.WriteAllBytes(path, data);
    }

    private static Mat? ReadGray(string path)
    {
        try
        {
            var mat = Cv2.ImDecode(File.ReadAllBytes(path), ImreadModes.Grayscale);
            if (!mat.Empty())
                return mat;
            mat.Dispose();
        }
        catch (Exception ex) when (ex is IOException or OpenCVException or UnauthorizedAccessException)
        {
            // Bozuk veya kilitli dosya eğitime alınmaz
        }
        return null;
    }
}
