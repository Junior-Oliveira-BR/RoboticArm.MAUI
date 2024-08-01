using Evergine.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoboticArm.Services
{
    public class MathService
    {
        public const float OneRadianInDegrees = 57.2957795131f;
        public const float OneDegreeInRadians = 0.01745329252f;

        public static Quaternion CreateFromTwoVectors(Vector3 u, Vector3 v)
        {
            if (u.Length() > 0) u = Vector3.Normalize(u);
            if (v.Length() > 0) v = Vector3.Normalize(v);

            //var result = Vector3.Dot(u, v);
            float result = MathHelper.Clamp(Vector3.Dot(u, v), -1f, 1f);
            var result2 = Vector3.Cross(u, v);

            if (result2.Length() > 0) result2 = Vector3.Normalize(result2);

            float angle = (float)Math.Acos(result);
            // Criação do quaternião
            Quaternion rotation = Quaternion.CreateFromAxisAngle(result2, angle);
            return rotation;
        }

        public static float ToAngle(Quaternion orientation, Vector3 axis)
        {
            Quaternion.ToAngleAxis(ref orientation, out var axisOfRotation, out var angle);
            if (Vector3.Dot(axis, axisOfRotation) < 0) angle = -angle;
            return angle;
        }

        public static float SimpleAngle(float theta)
        {
            theta = theta % (2.0f * (float)Math.PI);
            if (theta < -(float)Math.PI)
                theta += 2.0f * (float)Math.PI;
            else if (theta > (float)Math.PI)
                theta -= 2.0f * (float)Math.PI;
            return theta;
        }
    }
}
