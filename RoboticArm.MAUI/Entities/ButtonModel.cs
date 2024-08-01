using SkiaSharp;
using System;
using System.Collections.Generic;

namespace RoboticArm.MAUI.Entities
{
    public class ButtonModel : Dictionary<string, EntitySvg>
    {
        public SKPoint Position;
        public SKSize Size;

        public ButtonModel(List<string> lstSvgName)
        {
            bool first = true;
            foreach (var svgName in lstSvgName)
            {
                var entity = new EntitySvg(svgName);
                if (entity.Picture == null)
                {
                    throw new Exception();
                }
                this.Add(svgName, entity);

                if (first)
                {
                    first = false;
                    Size = new SKSize(entity.Picture.CullRect.Width, entity.Picture.CullRect.Height);
                    Position = new SKPoint(0 - (Size.Width / 2), 0 - (Size.Height / 2));
                }
            }
        }
    }
}
