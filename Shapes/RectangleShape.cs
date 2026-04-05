using Avalonia;
using Avalonia.Media;

namespace lab_four_oop.Shapes;

public class RectangleShape : ShapeBase
{
    public RectangleShape(double x, double y, double width, double height)
        : base(x, y, width, height)
    {
    }

    public override void Draw(DrawingContext context)
    {
        var borderPen = IsSelected
            ? new Pen(Brushes.Red, 3)
            : new Pen(Brushes.DarkGreen, 2);

        context.DrawRectangle(
            FillBrush,
            borderPen,
            new Rect(x, y, width, height)
        );
    }

    public override bool ContainsPoint(double px, double py)
    {
        return px >= x && px <= x + width &&
               py >= y && py <= y + height;
    }
}