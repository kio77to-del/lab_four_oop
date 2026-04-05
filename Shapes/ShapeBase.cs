using Avalonia;
using Avalonia.Media;

namespace lab_four_oop.Shapes;

public abstract class ShapeBase
{
    protected double x;
    protected double y;
    protected double width;
    protected double height;

    public bool IsSelected { get; set; }

    public IBrush FillBrush { get; set; }

    public double X => x;
    public double Y => y;
    public double Width => width;
    public double Height => height;

    protected ShapeBase(double x, double y, double width, double height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;

        IsSelected = false;
        FillBrush = Brushes.LightBlue;
    }

    public abstract void Draw(DrawingContext context);

    public abstract bool ContainsPoint(double px, double py);

    public virtual void Move(double dx, double dy, Rect bounds)
    {
        double newX = x + dx;
        double newY = y + dy;

        if (newX < bounds.Left)
            newX = bounds.Left;

        if (newY < bounds.Top)
            newY = bounds.Top;

        if (newX + width > bounds.Right)
            newX = bounds.Right - width;

        if (newY + height > bounds.Bottom)
            newY = bounds.Bottom - height;

        x = newX;
        y = newY;
    }

    public virtual void Resize(double dw, double dh, Rect bounds)
    {
        double newWidth = width + dw;
        double newHeight = height + dh;

        if (newWidth < 20)
            newWidth = 20;

        if (newHeight < 20)
            newHeight = 20;

        if (x + newWidth > bounds.Right)
            newWidth = bounds.Right - x;

        if (y + newHeight > bounds.Bottom)
            newHeight = bounds.Bottom - y;

        width = newWidth;
        height = newHeight;
    }

    public void SetColor(IBrush newBrush)
    {
        FillBrush = newBrush;
    }
}