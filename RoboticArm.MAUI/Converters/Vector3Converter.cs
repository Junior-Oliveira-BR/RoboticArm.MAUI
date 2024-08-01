using System.Globalization;
using System.Numerics;

namespace RoboticArm.MAUI.Converters
{
    internal class Vector3Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Vector3 vector)
            {
                return $"Posição:{{ X:{vector.X.ToString("0.00")} Y:{vector.Y.ToString("0.00")} Z:{vector.Z.ToString("0.00")}}}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
