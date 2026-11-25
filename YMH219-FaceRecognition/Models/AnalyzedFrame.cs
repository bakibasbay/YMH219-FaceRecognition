namespace FaceRecognition.Models;

// İşlenmiş kare: gösterilecek görüntü ve bulunan yüzler.
// Bitmap'in sahipliği alan tarafa geçer; işi biten Dispose etmelidir.
public sealed class AnalyzedFrame : IDisposable
{
    public AnalyzedFrame(Bitmap image, IReadOnlyList<RecognitionResult> faces, int savedFaces = 0)
    {
        Image = image;
        Faces = faces;
        SavedFaces = savedFaces;
    }

    public Bitmap Image { get; }
    public IReadOnlyList<RecognitionResult> Faces { get; }

    // "Yüzleri Kaydet" açıkken bu karede diske yazılan yüz sayısı
    public int SavedFaces { get; }

    // Karedeki en büyük (kameraya en yakın) yüz
    public RecognitionResult? PrimaryFace => Faces.Count == 0 ? null : Faces.MaxBy(f => f.Area);

    public void Dispose() => Image.Dispose();
}
