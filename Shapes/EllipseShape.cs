using Avalonia;
using Avalonia.Media;

namespace lab_four_oop.Shapes;

public class EllipseShape : ShapeBase
{
    public EllipseShape(double x, double y, double width, double height)
        : base(x, y, width, height)
    {
    }

    public override void Draw(DrawingContext context)
    {
        var borderPen = IsSelected
            ? new Pen(Brushes.Red, 3)
            : new Pen(Brushes.DarkMagenta, 2);

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
        double radiusX = width / 2;
        double radiusY = height / 2;

        if (radiusX <= 0 || radiusY <= 0)
            return false;

        double dx = (px - centerX) / radiusX;
        double dy = (py - centerY) / radiusY;

        return dx * dx + dy * dy <= 1;
    }
}