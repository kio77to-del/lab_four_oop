using Avalonia;
using Avalonia.Media;
using System;

namespace lab_four_oop.Shapes;

public class TriangleShape : ShapeBase
{
    private readonly double aspectRatio;

    public TriangleShape(double x, double y, double width, double height) : base(x, y, width, height)
    {
        aspectRatio = width / height;
    }

    public override void Draw(DrawingContext context)
    {
        var borderPen = IsSelected ? new Pen(Brushes.Red, 3) : new Pen(Brushes.DarkOrange, 2);

        Point p1 = new Point(x + width / 2, y);
        Point p2 = new Point(x, y + height);
        Point p3 = new Point(x + width, y + height);

        var geometry = new StreamGeometry();

        using (var geometryContext = geometry.Open())
        {
            geometryContext.BeginFigure(p1, true);
            geometryContext.LineTo(p2);
            geometryContext.LineTo(p3);
            geometryContext.EndFigure(true);
        }

        context.DrawGeometry(FillBrush, borderPen, geometry);
    }

    public override bool ContainsPoint(double px, double py)
    {
        Point p1 = new Point(x + width / 2, y);
        Point p2 = new Point(x, y + height);
        Point p3 = new Point(x + width, y + height);
        Point p = new Point(px, py);

        double area = TriangleArea(p1, p2, p3);
        double area1 = TriangleArea(p, p2, p3);
        double area2 = TriangleArea(p1, p, p3);
        double area3 = TriangleArea(p1, p2, p);

        return Math.Abs(area - (area1 + area2 + area3)) < 0.5;
    }

    private double TriangleArea(Point a, Point b, Point c)
    {
        return Math.Abs(
            (a.X * (b.Y - c.Y) +
             b.X * (c.Y - a.Y) +
             c.X * (a.Y - b.Y)) / 2.0
        );
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