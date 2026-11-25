using System.Drawing.Drawing2D;
using FaceRecognition.UI;

namespace FaceRecognition.Controls;

// CheckBox yerine kullanılan açık/kapalı anahtarı
public class ToggleSwitch : CheckBox
{
    private const int TrackWidth = 40;
    private const int TrackHeight = 22;

    public ToggleSwitch()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Cursor = Cursors.Hand;
        AutoSize = false;
    }

    protected override void OnCheckedChanged(EventArgs e)
    {
        Invalidate();
        base.OnCheckedChanged(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        Invalidate();
        base.OnEnabledChanged(e);
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        var g = pevent.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Surface);

        // Ray
        int top = (Height - TrackHeight) / 2;
        var track = new Rectangle(0, top, TrackWidth, TrackHeight);
        Color trackColor = !Enabled ? Theme.DisabledBack : Checked ? Theme.Primary : Theme.Idle;
        using (var path = Theme.RoundedRect(track, TrackHeight / 2))
        using (var brush = new SolidBrush(trackColor))
            g.FillPath(brush, path);

        // Topuz
        int knob = TrackHeight - 6;
        int knobX = Checked ? track.Right - knob - 3 : track.X + 3;
        using (var brush = new SolidBrush(Color.White))
            g.FillEllipse(brush, knobX, top + 3, knob, knob);

        // Yazı
        var textRect = new Rectangle(TrackWidth + 10, 0, Width - TrackWidth - 10, Height);
        TextRenderer.DrawText(g, Text, Font, textRect, Enabled ? ForeColor : Theme.DisabledText,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

        if (Focused && ShowFocusCues)
        {
            using var pen = new Pen(Color.FromArgb(120, Theme.Primary)) { DashStyle = DashStyle.Dot };
            g.DrawRectangle(pen, textRect.X - 2, 1, Math.Min(textRect.Width, TextRenderer.MeasureText(Text, Font).Width + 4), Height - 3);
        }
    }
}
