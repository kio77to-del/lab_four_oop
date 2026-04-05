using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using lab_four_oop.Storage;

namespace lab_four_oop.Controls;

public class DrawingArea : Control
{
    public ShapeStorage? Storage { get; set; }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        context.FillRectangle(Brushes.White, new Rect(Bounds.Size));

        if (Storage != null)
        {
            foreach (var shape in Storage.GetAll())
            {
                shape.Draw(context);
            }
        }
    }
}