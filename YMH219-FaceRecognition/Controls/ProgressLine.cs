using System.ComponentModel;
using System.Drawing.Drawing2D;
using FaceRecognition.UI;

namespace FaceRecognition.Controls;

// İnce, köşeleri yuvarlatılmış ilerleme çubuğu
public class ProgressLine : Control
{
    private int _value;
    private int _maximum = 100;

    public ProgressLine()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        TabStop = false;
    }

    [Category("Davranış")]
    [DefaultValue(100)]
    public int Maximum
    {
        get => _maximum;
        set
        {
            _maximum = Math.Max(1, value);
            Invalidate();
        }
    }

    [Category("Davranış")]
    [DefaultValue(0)]
    public int Value
    {
        get => _value;
        set
        {
            _value = Math.Clamp(value, 0, _maximum);
            Invalidate();
        }
    }

    [Category("Görünüm")]
    public Color BarColor { get; set; } = Theme.Primary;

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Surface);

        var track = new Rectangle(0, 0, Width - 1, Height - 1);
        using (var path = Theme.RoundedRect(track, Height / 2))
        using (var brush = new SolidBrush(Theme.Border))
            g.FillPath(brush, path);

        int fill = (int)((Width - 1) * (double)_value / _maximum);
        if (fill >= Height)
        {
            using var path = Theme.RoundedRect(new Rectangle(0, 0, fill, Height - 1), Height / 2);
            using var brush = new SolidBrush(BarColor);
            g.FillPath(brush, path);
        }
    }
}
