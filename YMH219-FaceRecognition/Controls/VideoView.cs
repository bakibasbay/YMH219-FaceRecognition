using System.ComponentModel;
using FaceRecognition.UI;

namespace FaceRecognition.Controls;

// Kamera karesini / resmi gösteren PictureBox; görüntü yokken ortada bilgi yazısı çizer
public class VideoView : PictureBox
{
    private string _placeholderText = "Görüntü yok";

    public VideoView()
    {
        SizeMode = PictureBoxSizeMode.Zoom;
        BackColor = Theme.VideoBackground;
        DoubleBuffered = true;
    }

    [Category("Görünüm")]
    [DefaultValue("Görüntü yok")]
    public string PlaceholderText
    {
        get => _placeholderText;
        set
        {
            _placeholderText = value;
            Invalidate();
        }
    }

    // Yeni kareyi gösterir ve bir önceki Bitmap'i serbest bırakır (bellek sızıntısını önler)
    public void SetFrame(Bitmap? frame)
    {
        var old = Image;
        Image = frame;
        old?.Dispose();
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
        base.OnPaint(pe);
        if (Image == null && !string.IsNullOrEmpty(_placeholderText))
        {
            TextRenderer.DrawText(pe.Graphics, _placeholderText, Font, ClientRectangle, Theme.Idle,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            SetFrame(null);
        base.Dispose(disposing);
    }
}
