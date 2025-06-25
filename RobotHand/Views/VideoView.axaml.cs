using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace RobotHand.Views;

public partial class VideoView : UserControl
{
    private double _centerX = 0;
    private double _centerY = 0;
    private double _currentRotate = 0;
    private bool _enableZoom = true;
    private List<Point> points = new List<Point>();

    private Canvas _canvas;
    private Image _image;
    private Image _image_e;
    private ScrollViewer _scroll;
    private Rectangle rectangle = new Rectangle();

    double deltaX = 0, deltaY = 0;
    double coifX = 1, coifY = 1;

    public VideoView()
    {
        InitializeComponent();
        _canvas = this.FindControl<Canvas>("canvas");
        _image = this.FindControl<Image>("image");
        _image_e = this.FindControl<Image>("image_e");
        _scroll = this.FindControl<ScrollViewer>("scroll");


        PointerMoved += OnPointerMoved;
        PointerWheelChanged += OnPointerWheelChanged;
        PointerPressed += OnPointerPressed;
        PointerReleased += OnPointerReleased;
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var position = e.GetPosition(_canvas);

        if (_enableZoom)
        {
            isPressed = false;
            coifX = _canvas.Width / (rectangle.Width);
            coifY = _canvas.Height / (rectangle.Height);
            var max = Math.Max(coifX, coifY);
            coifX = coifY = max;
            if (10 < coifY || 10 < coifX)
            {
                _canvas.Children.Remove(rectangle);
                return;
            }
            deltaX = point.X * coifX;
            deltaY = point.Y * coifY;

            _canvas.Width = coifX * _canvas.Width;
            _canvas.Height = coifY * _canvas.Height;

            _scroll.Offset = new Vector(deltaX, deltaY);
            _scroll.Offset = new Vector(deltaX, deltaY);

            _image.Width = _canvas.Width;
            _image.Height = _canvas.Height;
            _image_e.Width = _canvas.Width;
            _image_e.Height = _canvas.Height;

            _enableZoom = false;
            _canvas.Children.Remove(rectangle);
            points.Add(position);
        }
        else
        {
            isPressed = false;
        }
    }


    private bool isPressed = false;
    Point point;
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(_canvas);
        if (_enableZoom)
        {
            point = position;
            isPressed = true;
            rectangle = new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Red),
                ZIndex = 15,
                Height = 0,
                Width = 0,
                Opacity = 0.2,
            };
            Canvas.SetLeft(rectangle, position.X); Canvas.SetTop(rectangle, position.Y);
            _canvas.Children.Add(rectangle);
        }
        else
        {
            isPressed = true;


        }
    }

    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {

    }

    private async void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var position = e.GetPosition(_canvas);
        if (_enableZoom)
        {
            _centerX = position.X;
            _centerY = position.Y;
            if (isPressed)
            {
                var top = Canvas.GetTop(rectangle);
                var left = Canvas.GetLeft(rectangle);
                var width = _centerX - left;
                var height = _centerY - top;

                // Установка высоты и ширины
                rectangle.Width = Math.Abs(width);
                rectangle.Height = Math.Abs(height);
            }
        }
        else
        {
            if (isPressed)
            {
                var draggingX = position.X / _canvas.Width * _scroll.ScrollBarMaximum.X;
                var draggingY = position.Y / _canvas.Height * _scroll.ScrollBarMaximum.Y;
                _scroll.Offset = new Vector(draggingX, draggingY);
            }
        }
    }

    public void Reset(object sender, RoutedEventArgs e)
    {
        image.Clip = null;
        Canvas.SetLeft(_image, 0);
        Canvas.SetTop(_image, 0);
        _canvas.Height = 800;
        _canvas.Width = 800;
        _image.Height = 800;
        _image.Width = 800;
        _image_e.Height = 800;
        _image_e.Width = 800;
        _enableZoom = true;
    }

    public void Rotate0(object sender, RoutedEventArgs e)
    {
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(new RotateTransform(0));
        _scroll.Offset = new Vector(deltaX, deltaY);
        _scroll.Offset = new Vector(deltaX, deltaY);
        _image_e.RenderTransform = transformGroup;
        _image.RenderTransform = transformGroup;
        _currentRotate = 0;
    }

    public void Rotate90(object sender, RoutedEventArgs e)
    {
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(new RotateTransform(90));
        _scroll.Offset = new Vector(_scroll.ScrollBarMaximum.X - deltaY / coifY * coifX, deltaX / coifX * coifY);
        _scroll.Offset = new Vector(_scroll.ScrollBarMaximum.X - deltaY / coifY * coifX, deltaX / coifX * coifY);
        _image_e.RenderTransform = transformGroup;
        _image.RenderTransform = transformGroup;
        _currentRotate = 90;
    }

    public void Rotate180(object sender, RoutedEventArgs e)
    {
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(new RotateTransform(180));
        _scroll.Offset = new Vector(_scroll.ScrollBarMaximum.X - deltaX, _scroll.ScrollBarMaximum.Y - deltaY);
        _scroll.Offset = new Vector(_scroll.ScrollBarMaximum.X - deltaX, _scroll.ScrollBarMaximum.Y - deltaY);
        _image_e.RenderTransform = transformGroup;
        _image.RenderTransform = transformGroup;
        _currentRotate = 180;
    }

    public void Rotate270(object sender, RoutedEventArgs e)
    {
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(new RotateTransform(270));
        _image_e.RenderTransform = transformGroup;
        _image.RenderTransform = transformGroup;
        _scroll.Offset = new Vector(deltaY / coifY * coifX, _scroll.ScrollBarMaximum.Y - deltaX / coifX * coifY);
        _scroll.Offset = new Vector(deltaY / coifY * coifX, _scroll.ScrollBarMaximum.Y - deltaX / coifX * coifY);
        _currentRotate = 270;
    }
}