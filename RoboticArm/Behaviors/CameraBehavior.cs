using Evergine.Common.Input;
using Evergine.Common.Input.Mouse;
using Evergine.Common.Input.Pointer;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Mathematics;
using System;
using System.Linq;

namespace RoboticArm.Behaviors
{
    public class CameraBehavior : Behavior
    {
        [BindComponent(false)]
        public Transform3D Transform = null;

        private bool isRotating;

        private const float OrbitScale = 0.005f;

        private float thetaX;
        private float thetaY;

        private bool isDirty;

        private MouseDispatcher mouseDispatcher;
        private Point currentMouseState;
        private Vector2 lastMousePosition;

        private PointerDispatcher touchDispatcher;
        private Point currentTouchState;
        private Vector2 lastTouchPosition;

        public CameraBehavior()
        {
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();

            this.thetaX = 0;
            this.thetaY = 0;

            this.isRotating = false;

            this.isDirty = true;
        }

        protected override bool OnAttached()
        {
            Reset();

            return base.OnAttached();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            var display = this.Owner.Scene.Managers.RenderManager.ActiveCamera3D?.Display;
            if (display != null)
            {
                this.mouseDispatcher = display.MouseDispatcher;
                this.touchDispatcher = display.TouchDispatcher;
            }
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.HandleInput();

            if (this.isDirty)
            {
                this.CommitChanges();
                this.isDirty = false;
            }
        }

        private void HandleInput()
        {
            if (Evergine.Platform.DeviceInfo.PlatformType == Evergine.Common.PlatformType.Windows)
            {
                this.HandleMouse();
            }
            else
            {
                this.HandleTouch();
            }
        }

        private void HandleMouse()
        {
            if (this.mouseDispatcher == null)
            {
                return;
            }
            
            if (this.mouseDispatcher.IsButtonDown(MouseButtons.Left))
            {
                this.currentMouseState = this.mouseDispatcher.Position;

                if (this.isRotating == false)
                {
                    this.isRotating = true;
                }
                else
                {
                    Vector2 delta = Vector2.Zero;

                    delta.X = -this.currentMouseState.X + this.lastMousePosition.X;
                    delta.Y = -this.currentMouseState.Y + this.lastMousePosition.Y;

                    delta = -delta;
                    this.Orbit(delta * OrbitScale);
                }

                this.lastMousePosition.X = this.currentMouseState.X;
                this.lastMousePosition.Y = this.currentMouseState.Y;
            }
            else
            {
                this.isRotating = false;
            }
        }

        private void HandleTouch()
        {
            if (this.touchDispatcher == null)
            {
                return;
            }

            var point = this.touchDispatcher.Points.FirstOrDefault();

            if (point == null)
            {
                return;
            }

            if (point.State == ButtonState.Pressed)
            {
                this.currentTouchState = point.Position;

                if (this.isRotating == false)
                {
                    this.isRotating = true;
                }
                else
                {
                    Vector2 delta = Vector2.Zero;
                    delta.X = -this.currentTouchState.X + this.lastTouchPosition.X;
                    delta.Y = -this.currentTouchState.Y + this.lastTouchPosition.Y;

                    delta = -delta;
                    this.Orbit(delta * OrbitScale);
                }

                this.lastTouchPosition.X = this.currentTouchState.X;
                this.lastTouchPosition.Y = this.currentTouchState.Y;
            }
            else
            {
                this.isRotating = false;
            }
        }

        public void Orbit(Vector2 delta)
        {
            this.thetaX += delta.X;
            this.thetaY += delta.Y;
            this.isDirty = true;
        }

        public void CommitChanges() => Transform.Rotation = new Vector3(-this.thetaY, -this.thetaX, 0);

        public void Reset()
        {
            this.thetaX = -2.3f;
            this.thetaY = -0.17f;
            CommitChanges();
        }

        public void LookAt(string direction)
        {
            switch (direction)
            {
                case "HOME":
                    Reset();
                    break;

                case "RIGHT":
                    thetaY = -0.37f;
                    thetaX = -1.57f;
                    break;

                case "LEFT":
                    thetaY = -0.37f;
                    thetaX = 1.57f;
                    break;

                case "BACK":
                    thetaY = -0.35f;
                    thetaX = -0.015f;
                    break;

                case "FRONT":
                    thetaY = -0.35f;
                    thetaX = -3.14f;
                    break;
            }
            CommitChanges();
        }
    }
}
