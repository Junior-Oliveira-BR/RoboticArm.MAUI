using RoboticArm.MAUI.Helpers;
using System.Globalization;
using System.Numerics;

namespace RoboticArm.MAUI.Converters
{
    public class QuaternionToEulerAnglesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Quaternion quaternion)
            {
                Matrix4x4 matrix = Matrix4x4.CreateFromQuaternion(quaternion);
                Vector3 angle = MathHelper.QuaternionToEulerAngles(matrix);

                return $"Rotação:{{ X:{angle.X.ToString("0.00")} Y:{angle.Y.ToString("0.00")} Z:{angle.Z.ToString("0.00")}}}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
