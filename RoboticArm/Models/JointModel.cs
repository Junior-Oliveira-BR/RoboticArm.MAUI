using BulletSharp.SoftBody;
using Evergine.Components.Graphics3D;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Mathematics;
using RoboticArm.Behaviors;
using RoboticArm.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoboticArm.Models
{
    public class JointModel
    {
        public RotateBehavior rotate;
        private CollisionBehavior collision;
        public BlinkBehavior blink;
        public Entity entity;

        public JointModel(Entity entity, Vector3 axis, float minAngle = 0, float maxAngle = 0)
        {
            this.entity = entity;
            rotate = entity.FindComponent<RotateBehavior>();
            blink = entity.FindComponent<BlinkBehavior>();
            collision = entity.FindComponent<CollisionBehavior>();
            if (rotate != null)
            {
                rotate.minAngle = minAngle * MathService.OneDegreeInRadians;
                rotate.maxAngle = maxAngle * MathService.OneDegreeInRadians;
                rotate.axis = axis;
            }
        }

        public float AddRotation(DIRECTION dir)
        {
            var currentAngle = MathService.ToAngle(rotate.Transform.LocalOrientation, rotate.axis);
            currentAngle = MathService.SimpleAngle(currentAngle);

            if (dir == DIRECTION.COUNTERCLOCKWISE)
            {
                if (currentAngle == rotate.maxAngle)
                {
                    blink.Colliding(true);
                    blink.StopBlinking();
                    return currentAngle * MathService.OneRadianInDegrees;
                }
                var addAngle = (float)Math.PI / 200.0f;
                if (currentAngle + addAngle > rotate.maxAngle)
                {
                    addAngle = rotate.maxAngle - currentAngle;
                    blink.Colliding(true);
                    blink.StopBlinking();
                }

                var addRotation = Quaternion.CreateFromAxisAngle(rotate.axis, addAngle);
                rotate.Transform.LocalOrientation = addRotation * rotate.Transform.LocalOrientation;

                return (currentAngle + addAngle) * MathService.OneRadianInDegrees;
            }
            else
            {
                if (currentAngle == rotate.minAngle)
                {
                    blink.Colliding(true);
                    blink.StopBlinking();
                    return currentAngle * MathService.OneRadianInDegrees;
                }
                var addAngle = ((float)Math.PI / 200.0f) * -1;
                if (currentAngle + addAngle < rotate.minAngle)
                {
                    addAngle = rotate.minAngle - currentAngle;
                    blink.Colliding(true);
                    blink.StopBlinking();
                }

                var addRotation = Quaternion.CreateFromAxisAngle(rotate.axis, addAngle);
                rotate.Transform.LocalOrientation = addRotation * rotate.Transform.LocalOrientation;

                return (currentAngle + addAngle) * MathService.OneRadianInDegrees;
            }
        }

        public bool IsColliding()
        {
            if (collision == null) return false;
            else return collision.isColliding;
        }
    }
}
