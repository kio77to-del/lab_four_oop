using Avalonia;

namespace lab_four_oop.Shapes;

public class SquareShape : RectangleShape
{
    public SquareShape(double x, double y, double size)
        : base(x, y, size, size)
    {
    }

    public override void Resize(double dw, double dh, Rect bounds)
    {
        double delta = dw;

        if (System.Math.Abs(dh) > System.Math.Abs(dw))
            delta = dh;

        base.Resize(delta, delta, bounds);
    }
}