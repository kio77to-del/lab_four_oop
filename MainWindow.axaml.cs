using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using lab_four_oop.Controls;
using lab_four_oop.Shapes;
using lab_four_oop.Storage;

namespace lab_four_oop;

public partial class MainWindow : Window
{
    private ShapeStorage storage = new ShapeStorage();
    private EditorTool currentTool = EditorTool.Circle;

    public MainWindow()
    {
        InitializeComponent();

        var drawArea = this.FindControl<DrawingArea>("DrawArea");
        drawArea.Storage = storage;

        var circleButton = this.FindControl<Button>("CircleButton");
        var rectangleButton = this.FindControl<Button>("RectangleButton");
        var squareButton = this.FindControl<Button>("SquareButton");
        var ellipseButton = this.FindControl<Button>("EllipseButton");
        var triangleButton = this.FindControl<Button>("TriangleButton");
        var clearButton = this.FindControl<Button>("ClearButton");
        var deleteButton = this.FindControl<Button>("DeleteButton");
        var applyColorButton = this.FindControl<Button>("ApplyColorButton");

        circleButton.Click += OnCircleButtonClick;
        rectangleButton.Click += OnRectangleButtonClick;
        squareButton.Click += OnSquareButtonClick;
        ellipseButton.Click += OnEllipseButtonClick;
        triangleButton.Click += OnTriangleButtonClick;
        clearButton.Click += OnClearButtonClick;
        deleteButton.Click += OnDeleteButtonClick;
        applyColorButton.Click += OnApplyColorButtonClick;

        drawArea.PointerPressed += OnDrawAreaPointerPressed;
        this.KeyDown += OnWindowKeyDown;

        UpdateStatus();
    }

    private void OnCircleButtonClick(object? sender, RoutedEventArgs e)
    {
        currentTool = EditorTool.Circle;
        UpdateStatus("Выбран инструмент: круг");
    }

    private void OnRectangleButtonClick(object? sender, RoutedEventArgs e)
    {
        currentTool = EditorTool.Rectangle;
        UpdateStatus("Выбран инструмент: прямоугольник");
    }

    private void OnSquareButtonClick(object? sender, RoutedEventArgs e)
    {
        currentTool = EditorTool.Square;
        UpdateStatus("Выбран инструмент: квадрат");
    }

    private void OnEllipseButtonClick(object? sender, RoutedEventArgs e)
    {
        currentTool = EditorTool.Ellipse;
        UpdateStatus("Выбран инструмент: эллипс");
    }

    private void OnTriangleButtonClick(object? sender, RoutedEventArgs e)
    {
        currentTool = EditorTool.Triangle;
        UpdateStatus("Выбран инструмент: треугольник");
    }

    private void OnClearButtonClick(object? sender, RoutedEventArgs e)
    {
        storage.ClearAll();

        var drawArea = this.FindControl<DrawingArea>("DrawArea");
        drawArea.InvalidateVisual();

        UpdateStatus("Все фигуры удалены");
    }

    private void OnDeleteButtonClick(object? sender, RoutedEventArgs e)
    {
        DeleteSelectedShapes();
    }

    private void OnApplyColorButtonClick(object? sender, RoutedEventArgs e)
    {
        var drawArea = this.FindControl<DrawingArea>("DrawArea");
        var colorPicker = this.FindControl<ColorPicker>("ShapeColorPicker");
        var selectedShapes = storage.GetSelected();

        if (selectedShapes.Count == 0)
        {
            UpdateStatus("Нет выделенных фигур");
            return;
        }

        var selectedColor = colorPicker.Color;

        foreach (var shape in selectedShapes)
        {
            shape.SetColor(new SolidColorBrush(selectedColor));
        }

        drawArea.InvalidateVisual();
        UpdateStatus("Цвет выделенных фигур изменён");
    }

    private void OnWindowKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete)
        {
            DeleteSelectedShapes();
            return;
        }

        if (e.Key == Key.Left)
        {
            MoveSelectedShapes(-10, 0);
            return;
        }

        if (e.Key == Key.Right)
        {
            MoveSelectedShapes(10, 0);
            return;
        }

        if (e.Key == Key.Up)
        {
            MoveSelectedShapes(0, -10);
            return;
        }

        if (e.Key == Key.Down)
        {
            MoveSelectedShapes(0, 10);
            return;
        }

        if (e.Key == Key.OemPlus || e.Key == Key.Add)
        {
            ResizeSelectedShapes(10, 10);
            return;
        }

        if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
        {
            ResizeSelectedShapes(-10, -10);
        }
    }

    private void MoveSelectedShapes(double dx, double dy)
    {
        var drawArea = this.FindControl<DrawingArea>("DrawArea");
        var selectedShapes = storage.GetSelected();

        if (selectedShapes.Count == 0)
        {
            UpdateStatus("Нет выделенных фигур");
            return;
        }

        Rect bounds = new Rect(0, 0, drawArea.Bounds.Width, drawArea.Bounds.Height);

        foreach (var shape in selectedShapes)
        {
            shape.Move(dx, dy, bounds);
        }

        drawArea.InvalidateVisual();
        UpdateStatus("Фигуры перемещены");
    }

    private void ResizeSelectedShapes(double dw, double dh)
    {
        var drawArea = this.FindControl<DrawingArea>("DrawArea");
        var selectedShapes = storage.GetSelected();

        if (selectedShapes.Count == 0)
        {
            UpdateStatus("Нет выделенных фигур");
            return;
        }

        Rect bounds = new Rect(0, 0, drawArea.Bounds.Width, drawArea.Bounds.Height);

        foreach (var shape in selectedShapes)
        {
            shape.Resize(dw, dh, bounds);
        }

        drawArea.InvalidateVisual();
        UpdateStatus("Размер фигур изменён");
    }

    private void DeleteSelectedShapes()
    {
        var drawArea = this.FindControl<DrawingArea>("DrawArea");

        storage.RemoveSelected();
        drawArea.InvalidateVisual();

        UpdateStatus("Выделенные фигуры удалены");
    }

    private void OnDrawAreaPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var drawArea = this.FindControl<DrawingArea>("DrawArea");
        drawArea.Focus();

        var point = e.GetPosition(drawArea);
        bool ctrlPressed = e.KeyModifiers.HasFlag(KeyModifiers.Control);

        var clickedShapes = storage.FindAllShapesAt(point.X, point.Y);

        if (clickedShapes.Count > 0)
        {
            if (!ctrlPressed)
            {
                storage.ClearSelection();

                var topShape = clickedShapes[clickedShapes.Count - 1];
                topShape.IsSelected = true;
            }
            else
            {
                foreach (var shape in clickedShapes)
                {
                    shape.IsSelected = !shape.IsSelected;
                }
            }

            drawArea.InvalidateVisual();
            UpdateStatus("Выделение изменено");
            return;
        }

        if (!ctrlPressed)
        {
            storage.ClearSelection();
        }

        ShapeBase tempShape;

        if (currentTool == EditorTool.Circle)
        {
            tempShape = new CircleShape(0, 0, 80);
        }
        else if (currentTool == EditorTool.Rectangle)
        {
            tempShape = new RectangleShape(0, 0, 120, 70);
        }
        else if (currentTool == EditorTool.Square)
        {
            tempShape = new SquareShape(0, 0, 80);
        }
        else if (currentTool == EditorTool.Ellipse)
        {
            tempShape = new EllipseShape(0, 0, 120, 80);
        }
        else
        {
            tempShape = new TriangleShape(0, 0, 100, 90);
        }

        double newX = point.X - tempShape.Width / 2;
        double newY = point.Y - tempShape.Height / 2;

        if (newX < 0)
            newX = 0;

        if (newY < 0)
            newY = 0;

        if (newX + tempShape.Width > drawArea.Bounds.Width)
            newX = drawArea.Bounds.Width - tempShape.Width;

        if (newY + tempShape.Height > drawArea.Bounds.Height)
            newY = drawArea.Bounds.Height - tempShape.Height;

        ShapeBase newShape;

        if (currentTool == EditorTool.Circle)
        {
            newShape = new CircleShape(newX, newY, 80);
        }
        else if (currentTool == EditorTool.Rectangle)
        {
            newShape = new RectangleShape(newX, newY, 120, 70);
        }
        else if (currentTool == EditorTool.Square)
        {
            newShape = new SquareShape(newX, newY, 80);
        }
        else if (currentTool == EditorTool.Ellipse)
        {
            newShape = new EllipseShape(newX, newY, 120, 80);
        }
        else
        {
            newShape = new TriangleShape(newX, newY, 100, 90);
        }

        storage.Add(newShape);
        drawArea.InvalidateVisual();

        UpdateStatus("Фигура добавлена");
    }

    private void UpdateStatus(string message = "")
    {
        var statusTextBlock = this.FindControl<TextBlock>("StatusTextBlock");

        string toolName = "круг";

        if (currentTool == EditorTool.Rectangle)
            toolName = "прямоугольник";
        else if (currentTool == EditorTool.Square)
            toolName = "квадрат";
        else if (currentTool == EditorTool.Ellipse)
            toolName = "эллипс";
        else if (currentTool == EditorTool.Triangle)
            toolName = "треугольник";

        if (message == "")
        {
            statusTextBlock.Text =
                $"Статус: фигур {storage.CountAll()}, выделено {storage.CountSelected()}, инструмент: {toolName}";
        }
        else
        {
            statusTextBlock.Text =
                $"Статус: {message}. Фигур {storage.CountAll()}, выделено {storage.CountSelected()}, инструмент: {toolName}";
        }
    }
}