using Avalonia.Controls;
using lab_four_oop.Controls;
using lab_four_oop.Storage;

namespace lab_four_oop;

public partial class MainWindow : Window
{
    private ShapeStorage storage = new ShapeStorage();

    public MainWindow()
    {
        InitializeComponent();

        var drawArea = this.FindControl<DrawingArea>("DrawArea");
        drawArea.Storage = storage;

        UpdateStatus();
    }

    private void UpdateStatus()
    {
        var statusTextBlock = this.FindControl<TextBlock>("StatusTextBlock");
        statusTextBlock.Text = $"Статус: фигур {storage.CountAll()}, выделено {storage.CountSelected()}";
    }
}