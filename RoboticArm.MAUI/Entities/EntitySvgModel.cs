using SkiaSharp;
using System.IO;
using System.Reflection;

namespace RoboticArm.MAUI.Entities
{
    public class EntitySvgModel : SkiaSharp.Extended.Svg.SKSvg
    {
        public SKPoint Position;
        public float Rotation = 0;
        public SKSize Size;

        public EntitySvgModel(string svgName) : base()
        {
            if (!string.IsNullOrEmpty(svgName)) using (var stream = GetImageStream(svgName)) this.Load(stream);
            Size = new SKSize(Picture.CullRect.Width, Picture.CullRect.Height);
            Position = new SKPoint(0 - (Size.Width / 2), 0 - (Size.Height / 2));
        }

        public SKRect GetRect() => new SKRect(Position.X, Position.Y, Position.X + Size.Width, Position.Y + Size.Height);

        private static Stream GetImageStream(string svgName)
        {
            var assembly = typeof(EntitySvgModel).GetTypeInfo().Assembly;
            var data = assembly.GetManifestResourceStream("RoboticArm.MAUI.Resources.Images." + svgName + ".svg");
            return data;
        }

        public virtual void Draw(SKCanvas canvas)
        {
            using (new SKAutoCanvasRestore(canvas, true))
            {
                canvas.RotateDegrees(Rotation, GetRect().MidX, GetRect().MidY);
                canvas.DrawPicture(Picture, Position.X, Position.Y);
            }
        }
    }
}
