using BulletSharp.SoftBody;
using Evergine.Framework;
using Evergine.Framework.Physics3D;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoboticArm.Behaviors
{
    public class CollisionBehavior : Behavior
    {
        [BindComponent(false)]
        public RigidBody3D body = null;
        public bool isColliding = false;
        private int framesWithoutCollision = 0;
        private int requiredFramesWithoutCollision = 5;

        protected override void OnActivated()
        {
            base.OnActivated();

            if (body != null)
            {
                // Subscribe to the begin collision event...
                body.BeginCollision += Body_BeginCollision;
                body.EndCollision += Body_EndCollision;
                body.UpdateCollision += Body_UpdateCollision;
            }
        }      

        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            if (body != null)
            {
                // Unsubscribe to the begin collision event...
                body.BeginCollision -= Body_BeginCollision;
                body.EndCollision -= Body_EndCollision;
                body.UpdateCollision -= Body_UpdateCollision;
            }
        }

        public CollisionBehavior() { }

        private void Body_BeginCollision(object sender, CollisionInfo3D e) => isColliding = true;

        private void Body_EndCollision(object sender, CollisionInfo3D e)
        {
            //isColliding = false;
        }

        private void Body_UpdateCollision(object sender, CollisionInfo3D e)
        {
            isColliding = true;
            framesWithoutCollision = 0;
        }

        protected override void Update(TimeSpan gameTime) 
        {
            if (isColliding)
            {
                framesWithoutCollision++;
                if (framesWithoutCollision >= requiredFramesWithoutCollision)
                {
                    isColliding = false;
                }
            }
        }
    }
}
