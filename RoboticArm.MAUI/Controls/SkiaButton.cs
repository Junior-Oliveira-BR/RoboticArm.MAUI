using RoboticArm.MAUI.Entities;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Windows.Input;

namespace RoboticArm.MAUI.Controls
{
    public class SkiaButton : ContentView
    {
        public SKCanvasView canvasView;
        private SKMatrix _m = SKMatrix.CreateIdentity();

        public bool Flip
        {
            get => (bool)GetValue(FlipProperty);
            set => SetValue(FlipProperty, value);
        }
        public static readonly BindableProperty FlipProperty =
            BindableProperty.Create(nameof(Flip), typeof(bool), typeof(SkiaButton), false);

        public int Rotate
        {
            get => (int)GetValue(RotateProperty);
            set => SetValue(RotateProperty, value);
        }
        public static readonly BindableProperty RotateProperty =
            BindableProperty.Create(nameof(Rotate), typeof(int), typeof(SkiaButton), 0);

        public bool IsToogle
        {
            get => (bool)GetValue(IsToogleProperty);
            set => SetValue(IsToogleProperty, value);
        }
        public static readonly BindableProperty IsToogleProperty =
            BindableProperty.Create(nameof(IsToogle), typeof(bool), typeof(SkiaButton), false);

        public bool Pressed
        {
            get => (bool)GetValue(PressedProperty);
            set => SetValue(PressedProperty, value);
        }
        public static readonly BindableProperty PressedProperty =
            BindableProperty.Create(nameof(Pressed), typeof(bool), typeof(SkiaButton), false, BindingMode.OneWay, propertyChanged: OnPropertyChanged); 

        public string ReleaseImage
        {
            get => (string)GetValue(ReleaseImageProperty);
            set => SetValue(ReleaseImageProperty, value);
        }
        public static readonly BindableProperty ReleaseImageProperty =
            BindableProperty.Create(nameof(ReleaseImage), typeof(string), typeof(SkiaButton), string.Empty, propertyChanged: OnPropertyChanged);

        public string PressedImage
        {
            get => (string)GetValue(PresseImageProperty);
            set => SetValue(PresseImageProperty, value);
        }
        public static readonly BindableProperty PresseImageProperty =
            BindableProperty.Create(nameof(PressedImage), typeof(string), typeof(SkiaButton), string.Empty, propertyChanged: OnPropertyChanged);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(SkiaButton), null);

        public ICommand CommandPressed
        {
            get { return (ICommand)GetValue(CommandPressedProperty); }
            set { SetValue(CommandPressedProperty, value); }
        }
        public static readonly BindableProperty CommandPressedProperty = BindableProperty.Create(nameof(CommandPressed), typeof(ICommand), typeof(SkiaButton), null);

        public ICommand CommandLoose
        {
            get { return (ICommand)GetValue(CommandLooseProperty); }
            set { SetValue(CommandLooseProperty, value); }
        }
        public static readonly BindableProperty CommandLooseProperty = BindableProperty.Create(nameof(CommandLoose), typeof(ICommand), typeof(SkiaButton), null);

        ButtonModel button;

        public SkiaButton()
        {
            canvasView = new SKCanvasView();

            canvasView.PaintSurface += CanvasView_PaintSurface; ;
            canvasView.EnableTouchEvents = true;
            canvasView.InputTransparent = false;
            canvasView.Touch += SkiaButton_Touch;
            Content = canvasView;
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if(button == null)
            {
                if (!string.IsNullOrEmpty(ReleaseImage) && !string.IsNullOrEmpty(PressedImage))
                    button = new ButtonModel(new List<string> { ReleaseImage, PressedImage });
                else return;
            }

            var screenWidth = e.Info.Width;
            var screenHeight = e.Info.Height;
            var scaleX = screenWidth / button.Size.Width;
            var scaleY = screenHeight / button.Size.Height;
            float scale = scaleX;
            if (scaleX > scaleY) scale = scaleY;
            _m.ScaleX = scale;
            _m.ScaleY = scale;

            var canvas = e.Surface.Canvas;
            canvas.Clear();

            canvas.SetMatrix(_m);
            canvas.Translate((screenWidth / 2 / scale), (screenHeight / 2 / scale));
            if (Flip) canvas.Scale(-1, 1);
            canvas.RotateDegrees(Rotate);

            if (Pressed)
            {
                var but = button[PressedImage].Picture;
                canvas.DrawPicture(but, button.Position);
            }
            else
            {
                var but = button[ReleaseImage].Picture;
                canvas.DrawPicture(but, button.Position);
            }
        }

        private void SkiaButton_Touch(object sender, SKTouchEventArgs args)
        {
            switch (args.ActionType)
            {
                case SKTouchAction.Pressed:
                    if (IsToogle)
                    {
                        Pressed = !Pressed;
                        if(Pressed) CommandPressed?.Execute(CommandParameter);
                        else CommandLoose?.Execute(CommandParameter);
                    }
                    else
                    {
                        CommandPressed?.Execute(CommandParameter);
                        Pressed = true;
                    }
                    break;
                case SKTouchAction.Released:
                    if (!IsToogle)
                    {
                        Pressed = false;
                        CommandLoose?.Execute(CommandParameter);
                    }
                    break;
            }
            args.Handled = true;
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var button = bindable as SkiaButton;
            button?.canvasView.InvalidateSurface();
        }
    }
}
