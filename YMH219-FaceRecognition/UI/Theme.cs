using System.Drawing.Drawing2D;

namespace FaceRecognition.UI;

// Uygulama genelinde kullanılan renkler ve çizim yardımcıları
internal static class Theme
{
    public static readonly Color Background = Color.FromArgb(241, 245, 249);
    public static readonly Color Surface = Color.White;
    public static readonly Color Editor = Color.FromArgb(248, 250, 252);
    public static readonly Color Header = Color.FromArgb(15, 23, 42);
    public static readonly Color HeaderHover = Color.FromArgb(30, 41, 59);
    public static readonly Color Border = Color.FromArgb(226, 232, 240);
    public static readonly Color Text = Color.FromArgb(30, 41, 59);
    public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
    public static readonly Color TextOnDark = Color.FromArgb(203, 213, 225);

    public static readonly Color Primary = Color.FromArgb(79, 70, 229);
    public static readonly Color Success = Color.FromArgb(16, 185, 129);
    public static readonly Color Warning = Color.FromArgb(245, 158, 11);
    public static readonly Color Danger = Color.FromArgb(239, 68, 68);
    public static readonly Color Idle = Color.FromArgb(148, 163, 184);

    public static readonly Color DisabledBack = Color.FromArgb(226, 232, 240);
    public static readonly Color DisabledText = Color.FromArgb(148, 163, 184);
    public static readonly Color VideoBackground = Color.FromArgb(15, 23, 42);

    // Etiketlere sırayla verilen dikdörtgen renkleri ("Bilinmiyor" her zaman kırmızı)
    private static readonly Color[] LabelPalette =
    {
        Color.FromArgb(16, 185, 129),  // yeşil
        Color.FromArgb(14, 165, 233),  // mavi
        Color.FromArgb(168, 85, 247),  // mor
        Color.FromArgb(245, 158, 11),  // turuncu
        Color.FromArgb(236, 72, 153),  // pembe
        Color.FromArgb(20, 184, 166)   // turkuaz
    };

    public static Color LabelColor(int index) =>
        index < 0 ? Danger : LabelPalette[index % LabelPalette.Length];

    // Rengi verilen oranda koyulaştırır (üzerine gelme / basma efekti için)
    public static Color Darken(Color color, float amount) => Color.FromArgb(
        color.A,
        (int)(color.R * (1 - amount)),
        (int)(color.G * (1 - amount)),
        (int)(color.B * (1 - amount)));

    // Köşeleri yuvarlatılmış dikdörtgen yolu oluşturur
    public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        int d = Math.Max(1, Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height)));
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
