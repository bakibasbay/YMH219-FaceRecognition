namespace FaceRecognition.Models;

// Karede bulunan tek bir yüzün sonucu: konum, etiket ve güven skoru
public sealed record RecognitionResult(Rectangle Bounds, string Label, double? Confidence, bool IsKnown)
{
    public const string UnknownLabel = "Bilinmiyor";
    public const string FaceLabel = "Yüz";

    // Tanıma kapalıyken yalnızca algılanan yüz
    public static RecognitionResult Detected(Rectangle bounds) => new(bounds, FaceLabel, null, false);

    // Tanıma yapıldı mı (güven skoru var mı)
    public bool IsRecognized => Confidence.HasValue;

    // Dikdörtgen üzerinde gösterilen yazı: "Etiket – güven"
    public string Caption => Confidence is double c ? $"{Label} – {c:0.0}" : Label;

    // Yüz alanı (en büyük yüzü seçmek için)
    public int Area => Bounds.Width * Bounds.Height;
}
