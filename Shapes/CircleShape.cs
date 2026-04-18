using Avalonia;
using Avalonia.Media;
using System;

namespace lab_four_oop.Shapes;

public class CircleShape : ShapeBase
{
    public CircleShape(double x, double y, double size) : base(x, y, size, size)
    {
    }

    public override void Draw(DrawingContext context)
    {
        var borderPen = IsSelected ? new Pen(Brushes.Red, 3) : new Pen(Brushes.DarkBlue, 2);

        context.DrawEllipse(
            FillBrush,
            borderPen,
            new Point(x + width / 2, y + height / 2),
            width / 2,
            height / 2
        );
    }

    public override bool ContainsPoint(double px, double py)
    {
        double centerX = x + width / 2;
        double centerY = y + height / 2;
        double radius = width / 2;

        if (radius <= 0)
            return false;

        double dx = px - centerX;
        double dy = py - centerY;

        return dx * dx + dy * dy <= radius * radius;
    }

    public override void Resize(double dw, double dh, Rect bounds)
    {
        double delta = Math.Abs(dw) > Math.Abs(dh) ? dw : dh;

        double targetSize = width + delta;
        double maxSizeByBounds = Math.Min(bounds.Right - x, bounds.Bottom - y);

        if (maxSizeByBounds < 20)
            maxSizeByBounds = 20;

        double finalSize = Math.Min(targetSize, maxSizeByBounds);
        if (finalSize < 20)
            finalSize = 20;

        width = finalSize;
        height = finalSize;
    }
}