using Avalonia;
using System;

namespace lab_four_oop.Shapes;

public class SquareShape : RectangleShape
{
    public SquareShape(double x, double y, double size) : base(x, y, size, size)
    {
    }

    public override void Resize(double dw, double dh, Rect bounds)
    {
        double delta = Math.Abs(dw) > Math.Abs(dh) ? dw : dh;

        double targetSize = Width + delta;
        double maxSizeByBounds = Math.Min(bounds.Right - X, bounds.Bottom - Y);

        if (maxSizeByBounds < 20)
            maxSizeByBounds = 20;

        double finalSize = Math.Min(targetSize, maxSizeByBounds);
        if (finalSize < 20)
            finalSize = 20;

        width = finalSize;
        height = finalSize;
    }
}