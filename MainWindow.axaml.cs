using Avalonia.Controls;
using Avalonia.Input;
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

        circleButton.Click += OnCircleButtonClick;
        rectangleButton.Click += OnRectangleButtonClick;
        squareButton.Click += OnSquareButtonClick;
        clearButton.Click += OnClearButtonClick;

        drawArea.PointerPressed += OnDrawAreaPointerPressed;

        UpdateStatus();
    }

    private void OnCircleButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        currentTool = EditorTool.Circle;
        UpdateStatus("Выбран инструмент: круг");
    }

    private void OnRectangleButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        currentTool = EditorTool.Rectangle;
        UpdateStatus("Выбран инструмент: прямоугольник");
    }

    private void OnSquareButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        currentTool = EditorTool.Square;
        UpdateStatus("Выбран инструмент: квадрат");
    }

    private void OnClearButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        storage.ClearAll();

        var drawArea = this.FindControl<DrawingArea>("DrawArea");
        drawArea.InvalidateVisual();

        UpdateStatus("Все фигуры удалены");
    }

    private void OnDrawAreaPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var drawArea = this.FindControl<DrawingArea>("DrawArea");
        var point = e.GetPosition(drawArea);

        ShapeBase newShape;

        if (currentTool == EditorTool.Circle)
        {
            newShape = new CircleShape(point.X, point.Y, 80);
        }
        else if (currentTool == EditorTool.Rectangle)
        {
            newShape = new RectangleShape(point.X, point.Y, 120, 70);
        }
        else
        {
            newShape = new SquareShape(point.X, point.Y, 80);
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