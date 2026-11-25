using System.Text.Json;
using FaceRecognition.Models;
using OpenCvSharp;
using OpenCvSharp.Face;

namespace FaceRecognition.Services;

// Model dosyası okunamadığında veya eğitim yapılamadığında fırlatılır
public sealed class ModelException(string message) : Exception(message);

// LBPH yüz tanıyıcı: eğitme, model.yml olarak kaydetme/yükleme ve tahmin
public sealed class FaceRecognizerService : IDisposable
{
    public const double DefaultThreshold = 70;

    // Etiket adları model.yml'de Türkçe karakterleri bozulduğu için ayrı UTF-8 dosyada tutulur
    private sealed record ModelInfo(string[] Labels, int[] SampleCounts, DateTime TrainedAt);

    private readonly string _modelPath;
    private readonly string _infoPath;
    private readonly object _lock = new();
    private LBPHFaceRecognizer? _recognizer;
    private ModelInfo? _info;

    public FaceRecognizerService(string rootDirectory)
    {
        _modelPath = Path.Combine(rootDirectory, "model.yml");
        _infoPath = Path.Combine(rootDirectory, "model.labels.json");
    }

    // Bu uzaklığın üzerindeki tahminler "Bilinmiyor" sayılır (LBPH: düşük = daha benzer)
    public double Threshold { get; set; } = DefaultThreshold;

    public bool IsTrained => _recognizer != null;
    public IReadOnlyList<string> Labels => _info?.Labels ?? Array.Empty<string>();
    public int SampleCount => _info?.SampleCounts.Sum() ?? 0;
    public DateTime? TrainedAt => _info?.TrainedAt;

    // Model en son eğitildiği veri setiyle aynı mı (etiketler ve örnek sayıları)
    public bool MatchesDataset(IReadOnlyList<PersonLabel> labels)
    {
        var current = labels.Where(l => l.SampleCount > 0).ToList();
        return _info != null &&
               current.Select(l => l.Name).SequenceEqual(_info.Labels) &&
               current.Select(l => l.SampleCount).SequenceEqual(_info.SampleCounts);
    }

    // Kayıtlı model varsa yükler; yoksa false döner
    public bool TryLoad()
    {
        if (!File.Exists(_modelPath))
            return false;
        if (!File.Exists(_infoPath))
            throw new ModelException("Model dosyası var ancak etiket dosyası (model.labels.json) eksik. Modeli yeniden eğitin.");

        try
        {
            var info = JsonSerializer.Deserialize<ModelInfo>(File.ReadAllText(_infoPath))
                       ?? throw new ModelException("Etiket dosyası boş.");

            // Model bellekten okunur; Türkçe karakterli klasör yolları sorun çıkarmaz
            var recognizer = LBPHFaceRecognizer.Create();
            try
            {
                using var storage = new FileStorage(File.ReadAllText(_modelPath), FileStorage.Modes.Read | FileStorage.Modes.Memory);
                recognizer.Read(storage.Root() ?? throw new ModelException("Model dosyası boş."));
            }
            catch
            {
                recognizer.Dispose();
                throw;
            }
            Replace(recognizer, info);
            return true;
        }
        catch (Exception ex) when (ex is not ModelException)
        {
            throw new ModelException("Model dosyası okunamadı, modeli yeniden eğitin.\n\n" + ex.Message);
        }
    }

    // Veri setiyle yeni model eğitir ve diske kaydeder
    public void Train(TrainingSet set)
    {
        if (set.LabelNames.Count < 2)
            throw new ModelException("Eğitim için en az iki etiket gerekli. Eğitim ekranından ikinci bir etiket için örnek toplayın.");

        var counts = set.LabelNames.Select((_, id) => set.LabelIds.Count(x => x == id)).ToArray();
        var info = new ModelInfo(set.LabelNames.ToArray(), counts, DateTime.Now);

        // Varsayılan LBPH ayarları: yarıçap 1, 8 komşu, 8x8 ızgara
        var recognizer = LBPHFaceRecognizer.Create(1, 8, 8, 8);
        try
        {
            recognizer.Train(set.Faces, set.LabelIds);

            string yaml;
            using (var storage = new FileStorage(".yml", FileStorage.Modes.Write | FileStorage.Modes.Memory))
            {
                recognizer.Write(storage);
                yaml = storage.ReleaseAndGetString();
            }
            File.WriteAllText(_modelPath, yaml);
            File.WriteAllText(_infoPath, JsonSerializer.Serialize(info, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex)
        {
            recognizer.Dispose();
            throw new ModelException("Model eğitilemedi: " + ex.Message);
        }
        Replace(recognizer, info);
    }

    // Hazırlanmış yüz için etiket ve güven (LBPH uzaklığı) döndürür; eşik üstü "Bilinmiyor"
    public (string Label, double Confidence) Predict(Mat face)
    {
        lock (_lock)
        {
            if (_recognizer == null || _info == null)
                throw new ModelException("Model eğitilmemiş. Önce eğitim ekranından veri toplayıp modeli eğitin.");

            _recognizer.Predict(face, out int id, out double distance);
            bool known = id >= 0 && id < _info.Labels.Length && distance <= Threshold;
            return (known ? _info.Labels[id] : RecognitionResult.UnknownLabel, distance);
        }
    }

    private void Replace(LBPHFaceRecognizer recognizer, ModelInfo info)
    {
        lock (_lock)
        {
            _recognizer?.Dispose();
            _recognizer = recognizer;
            _info = info;
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            _recognizer?.Dispose();
            _recognizer = null;
        }
    }
}
