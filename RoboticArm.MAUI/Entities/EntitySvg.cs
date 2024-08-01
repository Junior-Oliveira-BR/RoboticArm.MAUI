using System.IO;
using System.Reflection;

namespace RoboticArm.MAUI.Entities
{
    public class EntitySvg : SkiaSharp.Extended.Svg.SKSvg
    {
        public EntitySvg(string svgName)
        {
            if (!string.IsNullOrEmpty(svgName)) using (var stream = GetImageStream(svgName)) this.Load(stream);
        }

        private static Stream GetImageStream(string svgName)
        {
            var assembly = typeof(EntitySvg).GetTypeInfo().Assembly;
            var data = assembly.GetManifestResourceStream("RoboticArm.MAUI.Resources.Images." + svgName + ".svg");
            return data;
        }
    }
}
