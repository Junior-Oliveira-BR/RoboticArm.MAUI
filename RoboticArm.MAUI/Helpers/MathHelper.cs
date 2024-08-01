using RoboticArm.MAUI.Models;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using ExCSS;

namespace RoboticArm.MAUI.Helpers
{
    public class MathHelper
    {
        private const double OneRadianInDegrees = 57.2957795131;
        private const double OneDegreeInRadians = 0.01745329252;

        public static Vector3 QuaternionToEulerAngles(Matrix4x4 matrix)
        {
            // Extrai os ângulos de rotação em radianos
            float x = (float)Math.Atan2(matrix.M23, matrix.M33);
            float y = (float)Math.Asin(-matrix.M13);
            float z = (float)Math.Atan2(matrix.M12, matrix.M11);

            // Converte os ângulos de radianos para graus
            x = ToDegrees(x);
            y = ToDegrees(y);
            z = ToDegrees(z);

            return new Vector3(x, y, z);
        }

        public static float QuaternionToRadian(Quaternion rotation, Vector3 axis)
        {
            if (axis.Equals(Vector3.UnitX)) return (float)Math.Asin(2 * (rotation.W * rotation.X + rotation.Y * rotation.Z));
            else if (axis.Equals(Vector3.UnitY)) return (float)Math.Atan2(2 * (rotation.W * rotation.Y - rotation.Z * rotation.X), 1 - 2 * (rotation.Y * rotation.Y + rotation.X * rotation.X));
            else  return (float)Math.Atan2(2 * (rotation.W * rotation.Z - rotation.X * rotation.Y), 1 - 2 * (rotation.Z * rotation.Z + rotation.X * rotation.X));
        }

        public static Quaternion CreateFromTwoVectors(Vector3 u, Vector3 v)
        {
            if (u.Length() > 0) u = Vector3.Normalize(u);
            if (v.Length() > 0) v = Vector3.Normalize(v);

            //var result = Vector3.Dot(u, v);
            float result = Clamp(Vector3.Dot(u, v), -1f, 1f);
            var result2 = Vector3.Cross(u, v);

            if (result2.Length() > 0) result2 = Vector3.Normalize(result2);

            float angle = MathF.Acos(result);
            // Criação do quaternião
            Quaternion rotation = Quaternion.CreateFromAxisAngle(result2, angle);
            return rotation;
        }

        public static Vector3 QuaternionToEulerAngles(Quaternion quaternion)
        {
            Matrix4x4 matrix = Matrix4x4.CreateFromQuaternion(quaternion);

            // Extrai os ângulos de rotação em radianos
            float x = (float)Math.Atan2(matrix.M23, matrix.M33);
            float y = (float)Math.Asin(-matrix.M13);
            float z = (float)Math.Atan2(matrix.M12, matrix.M11);

            // Converte os ângulos de radianos para graus
            x = ToDegrees(x);
            y = ToDegrees(y);
            z = ToDegrees(z);

            return new Vector3(x, y, z);
        }

        public static float ToDegrees(float radians)
        {
            return (float)(radians * 57.2957795131);
        }

        public static float ToRadians(float degrees)
        {
            return (float)((double)degrees * 0.01745329252);
        }

        public static float Clamp(float value, float angleLimit)
        {
            if (value < angleLimit * -1) return angleLimit * -1;
            else if (value > angleLimit) return angleLimit;

            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        
    }
}
