using Avalonia;
using Avalonia.Media;
using System;

namespace lab_four_oop.Shapes;

public class RectangleShape : ShapeBase
{
    private readonly double aspectRatio;

    public RectangleShape(double x, double y, double width, double height) : base(x, y, width, height)
    {
        aspectRatio = width / height;
    }

    public override void Draw(DrawingContext context)
    {
        var borderPen = IsSelected ? new Pen(Brushes.Red, 3) : new Pen(Brushes.DarkGreen, 2);

        context.DrawRectangle(
            FillBrush,
            borderPen,
            new Rect(x, y, width, height)
        );
    }

    public override bool ContainsPoint(double px, double py)
    {
        return px >= x && px <= x + width && py >= y && py <= y + height;
    }

    public override void Resize(double dw, double dh, Rect bounds)
    {
        double delta = Math.Abs(dw) > Math.Abs(dh) ? dw : dh;

        double targetWidth = width + delta;
        if (targetWidth < 20)
            targetWidth = 20;

        double targetHeight = targetWidth / aspectRatio;
        if (targetHeight < 20)
        {
            targetHeight = 20;
            targetWidth = targetHeight * aspectRatio;
        }

        double maxWidth = bounds.Right - x;
        double maxHeight = bounds.Bottom - y;

        double scaleByWidth = maxWidth / targetWidth;
        double scaleByHeight = maxHeight / targetHeight;
        double scale = Math.Min(1.0, Math.Min(scaleByWidth, scaleByHeight));

        double finalWidth = targetWidth * scale;
        double finalHeight = targetHeight * scale;

        if (finalWidth < 20)
        {
            finalWidth = 20;
            finalHeight = finalWidth / aspectRatio;
        }

        if (finalHeight < 20)
        {
            finalHeight = 20;
            finalWidth = finalHeight * aspectRatio;
        }

        width = finalWidth;
        height = finalHeight;
    }
}