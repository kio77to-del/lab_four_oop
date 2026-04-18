using Avalonia;
using Avalonia.Media;
using System;

namespace lab_four_oop.Shapes;

public class EllipseShape : ShapeBase
{
    private readonly double aspectRatio;

    public EllipseShape(double x, double y, double width, double height) : base(x, y, width, height)
    {
        aspectRatio = width / height;
    }

    public override void Draw(DrawingContext context)
    {
        var borderPen = IsSelected ? new Pen(Brushes.Red, 3) : new Pen(Brushes.DarkMagenta, 2);

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