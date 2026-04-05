using System.Collections.Generic;
using System.Linq;
using lab_four_oop.Shapes;

namespace lab_four_oop.Storage;

public class ShapeStorage
{
    private List<ShapeBase> shapes = new List<ShapeBase>();

    public void Add(ShapeBase shape)
    {
        shapes.Add(shape);
    }

    public List<ShapeBase> GetAll()
    {
        return shapes;
    }

    public void ClearSelection()
    {
        foreach (var shape in shapes)
        {
            shape.IsSelected = false;
        }
    }

    public void RemoveSelected()
    {
        shapes.RemoveAll(shape => shape.IsSelected);
    }

    public void ClearAll()
    {
        shapes.Clear();
    }

    public List<ShapeBase> FindAllShapesAt(double x, double y)
    {
        List<ShapeBase> foundShapes = new List<ShapeBase>();

        foreach (var shape in shapes)
        {
            if (shape.ContainsPoint(x, y))
            {
                foundShapes.Add(shape);
            }
        }

        return foundShapes;
    }

    public int CountSelected()
    {
        return shapes.Count(shape => shape.IsSelected);
    }

    public int CountAll()
    {
        return shapes.Count;
    }

    public List<ShapeBase> GetSelected()
    {
        return shapes.Where(shape => shape.IsSelected).ToList();
    }
}