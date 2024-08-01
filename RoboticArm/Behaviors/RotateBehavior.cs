using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Mathematics;
using RoboticArm.Models;
using RoboticArm.Services;
using System;
using System.Security.Cryptography.Xml;

namespace RoboticArm.Behaviors
{
    public class RotateBehavior : Behavior
    {
        [BindComponent(false)]
        public Transform3D Transform = null;
        public float velocity;
        public Vector3 axis;
        public bool rotationStarted;
        private int angleSign;
        private Quaternion initialRotation;
        public float minAngle, maxAngle;
        private float savedAngle;

        public RotateBehavior() { }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            velocity = 0.05f;
            rotationStarted = false;
            angleSign = 0;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            initialRotation = Transform.LocalOrientation;
        }

        public void SaveAngle()
        {
            var currentAngle = MathService.ToAngle(Transform.LocalOrientation, axis);
            savedAngle = MathService.SimpleAngle(currentAngle);
        }

        public void SaveAngle(float angle) => savedAngle = angle;

        public void RestoreAngle()
        {
            var currentAngle = MathService.ToAngle(Transform.LocalOrientation, axis);
            currentAngle = MathService.SimpleAngle(currentAngle);
            float difference = 0;
            if (currentAngle > savedAngle) difference = savedAngle - currentAngle;
            else if (currentAngle < savedAngle) difference = savedAngle - currentAngle;
            var correctedRotation = Quaternion.CreateFromAxisAngle(axis, difference);
            Transform.LocalOrientation = correctedRotation * Transform.LocalOrientation;
        }

        public void Clamp()
        {
            var currentAngle = MathService.ToAngle(Transform.LocalOrientation, axis);
            currentAngle = MathService.SimpleAngle(currentAngle);
            float difference = 0;
            if (currentAngle > maxAngle) difference = maxAngle - currentAngle;
            else if (currentAngle < minAngle) difference = minAngle - currentAngle;
            var correctedRotation = Quaternion.CreateFromAxisAngle(axis, difference);
            Transform.LocalOrientation = correctedRotation * Transform.LocalOrientation;
        }

        protected override void Update(TimeSpan gameTime)
        {
            //if (!rotationStarted) return;

            //var delta = gameTime.Milliseconds * velocity;
            //currentRotation += (delta * angleSign);
            //float angleInRadians = MathHelper.ToRadians(currentRotation);
            //var rotatation = Quaternion.CreateFromAxisAngle(axis, angleInRadians);
            //Transform.LocalOrientation = initialRotation * rotatation;

            //this.setRotation = rotation;
            //if (currentRotation == setRotation) return;
            //var deltaAngle = timerInterval * velocity;
            //currentRotation += deltaAngle;

            //if (currentRotation >= setRotation)
            //{
            //    currentRotation = setRotation;
            //    timer.Stop();
            //}
            //float desiredAngle = currentRotation;
            //float angleInRadians = MathHelper.ToRadians(desiredAngle);
            //transform3D.LocalOrientation = Quaternion.CreateFromAxisAngle(axis, angleInRadians);
        }
    }
}
