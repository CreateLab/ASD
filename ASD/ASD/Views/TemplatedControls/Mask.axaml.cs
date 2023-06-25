using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ASD.Views.TemplatedControls;

public class Mask : TemplatedControl
{
     private Canvas _canvas;
    private Image _image;
    private List<List<Point>> _figuresList = new();
    private bool IsDrawing { get; set; }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _canvas = e.NameScope.Find<Canvas>("DrawCanvas");
        _image = e.NameScope.Find<Image>("Image");
        _canvas.PointerPressed += Canvas_TouchDown;
        _canvas.PointerMoved += Canvas_TouchMove;
        _canvas.PointerReleased += Canvas_TouchUp;
        Thickness = 10;
    }


    /*public static readonly AvaloniaProperty<int> TestWidthProperty =
        AvaloniaProperty.Register<Mask, int>(nameof(TestWidth));

    public int TestWidth
    {
        get => (int)GetValue(TestWidthProperty);
        set => SetValue(TestWidthProperty, value);
    }*/


    public static readonly AvaloniaProperty<int> ThicknessProperty =
        AvaloniaProperty.Register<Mask, int>(nameof(Thickness));

    public int Thickness
    {
        get => (int)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }

    public static readonly AvaloniaProperty<Bitmap> ImageBackgroundProperty =
        AvaloniaProperty.Register<Mask, Bitmap>(nameof(ImageBackground));

    public Bitmap ImageBackground
    {
        get => (Bitmap)GetValue(ImageBackgroundProperty);
        set => SetValue(ImageBackgroundProperty, value);
    }

    public static readonly StyledProperty<Stretch> StretchProperty =
        AvaloniaProperty.Register<Image, Stretch>(nameof(Stretch), Stretch.Uniform);

    public Stretch Stretch
    {
        get { return GetValue(StretchProperty); }
        set { SetValue(StretchProperty, value); }
    }

    private void Canvas_TouchDown(object sender, PointerPressedEventArgs e)
    {
        // Start a new stroke by adding the touch point to the list
        IsDrawing = true;
        _figuresList.Add(new List<Point>());
        _figuresList.Last().Add(e.GetCurrentPoint(_canvas).Position);
    }

// Handle TouchMove event
    private void Canvas_TouchMove(object sender, PointerEventArgs e)
    {
        if (!IsDrawing) return;
        // Add the touch point to the list
        var pos = e.GetCurrentPoint(_canvas).Position;
        if (_canvas.Bounds.Width < pos.X + Thickness || _canvas.Bounds.Height < pos.Y + Thickness|| pos.X -Thickness < 0 || pos.Y - Thickness < 0) return;
        _figuresList.Last().Add(pos);
        //TestWidth = (int)e.GetCurrentPoint(_canvas).Position.X;
        // Redraw the strokes on the canvas
        RedrawStrokes();
    }

// Handle TouchUp event
    private void Canvas_TouchUp(object sender, PointerEventArgs e)
    {
        // Add the final touch point to the list
        _figuresList.Last().Add(e.GetCurrentPoint(_canvas).Position);

        // Redraw the strokes on the canvas
        RedrawStrokes();

        // Clear the touch points for the next stroke
        //touchPoints.Clear();
        IsDrawing = false;
    }

// Redraw the strokes on the canvas
    private void RedrawStrokes()
    {
        // Clear the canvas
        _canvas.Children.Clear();

        // Draw each stroke as a polyline
        foreach (var polyline in _figuresList.Select(touchPoints => new Polyline
                 {
                     Points = new List<Point>(touchPoints),
                     Stroke = Brushes.Black,
                     StrokeThickness = Thickness,
                 }))
        {
            _canvas.Children.Add(polyline);
        }
    }

    public void Clear()
    {
        _canvas.Children.Clear();
        _figuresList.Clear();
    }

    public string GetMask()
    {
        var pixelSize = new Size(_image.Source.Size.Width, _image.Source.Size.Height);
        using RenderTargetBitmap bitmap = new RenderTargetBitmap(pixelSize, new Vector(96, 96));
        using var ms = new MemoryStream();
        bitmap.Render(_canvas);
        bitmap.Save(ms);
        ms.Position = 0;
        var array = ms.ToArray();
        return Convert.ToBase64String(array);
    }
}