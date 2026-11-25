using System.Drawing.Drawing2D;
using System.Drawing.Text;
using FaceRecognition.Models;

namespace FaceRecognition.UI;

// Yüz dikdörtgenlerini ve "Etiket – güven" yazısını görüntünün üzerine çizer.
// OpenCV'nin putText fonksiyonu Türkçe karakterleri desteklemediği için GDI+ kullanılır.
public static class FaceOverlayRenderer
{
    public static void Draw(Bitmap image, IReadOnlyList<RecognitionResult> faces, Func<RecognitionResult, Color> colorOf)
    {
        if (faces.Count == 0)
            return;

        using var g = Graphics.FromImage(image);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

        // Çizgi ve yazı boyutu görüntü çözünürlüğüne göre ölçeklenir
        float scale = Math.Max(1f, Math.Max(image.Width, image.Height) / 640f);
        using var font = new Font("Segoe UI Semibold", 13f * scale, GraphicsUnit.Pixel);

        foreach (var face in faces)
        {
            var color = colorOf(face);
            var box = face.Bounds;

            using (var pen = new Pen(color, 2.5f * scale))
            using (var path = Theme.RoundedRect(box, (int)(6 * scale)))
                g.DrawPath(pen, path);

            // Etiket kutucuğu dikdörtgenin üstüne, yer yoksa içine yerleştirilir
            var textSize = g.MeasureString(face.Caption, font);
            float padX = 7 * scale, padY = 3 * scale;
            float tagW = textSize.Width + padX * 2;
            float tagH = textSize.Height + padY * 2;
            float tagY = box.Y - tagH - 4 * scale;
            if (tagY < 0)
                tagY = box.Y + 4 * scale;
            float tagX = Math.Clamp(box.X, 0, Math.Max(0, image.Width - tagW));

            var tag = Rectangle.Round(new RectangleF(tagX, tagY, tagW, tagH));
            using (var path = Theme.RoundedRect(tag, (int)(5 * scale)))
            using (var brush = new SolidBrush(Color.FromArgb(230, color)))
                g.FillPath(brush, path);
            g.DrawString(face.Caption, font, Brushes.White, tagX + padX, tagY + padY);
        }
    }
}
