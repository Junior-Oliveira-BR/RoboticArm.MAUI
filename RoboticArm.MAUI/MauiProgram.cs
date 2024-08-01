using RoboticArm.MAUI.Evergine;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using MR.Gestures;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;

namespace RoboticArm.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseSkiaSharp()
                .ConfigureSyncfusionCore()
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiEvergine()
                .ConfigureMRGestures("ARUZ-YL8S-7HCT-Q5EX-VLKV-B94V-HZ4C-6VNY-HEUF-KH58-44ZU-Q6UG-ECZB")
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MyFont.ttf", "MyFontIcons");
                    fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "AwesomeRegular");
                    fonts.AddFont("ArmRobotic.ttf", "ArmRobotic");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}