using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
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
        var clearButton = this.FindControl<Button>("ClearButton");
        var deleteButton = this.FindControl<Button>("DeleteButton");

        circleButton.Click += OnCircleButtonClick;
        rectangleButton.Click += OnRectangleButtonClick;
        squareButton.Click += OnSquareButtonClick;
        clearButton.Click += OnClearButtonClick;
        deleteButton.Click += OnDeleteButtonClick;

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

                foreach (var shape in clickedShapes)
                {
                    shape.IsSelected = true;
                }
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

        ShapeBase newShape;

        if (currentTool == EditorTool.Circle)
        {
            newShape = new CircleShape(0, 0, 80);
        }
        else if (currentTool == EditorTool.Rectangle)
        {
            newShape = new RectangleShape(0, 0, 120, 70);
        }
        else
        {
            newShape = new SquareShape(0, 0, 80);
        }

        double newX = point.X - newShape.Width / 2;
        double newY = point.Y - newShape.Height / 2;

        if (newX < 0)
            newX = 0;

        if (newY < 0)
            newY = 0;

        if (newX + newShape.Width > drawArea.Bounds.Width)
            newX = drawArea.Bounds.Width - newShape.Width;

        if (newY + newShape.Height > drawArea.Bounds.Height)
            newY = drawArea.Bounds.Height - newShape.Height;

        if (currentTool == EditorTool.Circle)
        {
            newShape = new CircleShape(newX, newY, 80);
        }
        else if (currentTool == EditorTool.Rectangle)
        {
            newShape = new RectangleShape(newX, newY, 120, 70);
        }
        else
        {
            newShape = new SquareShape(newX, newY, 80);
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