using Evergine.Common.Graphics;
using Evergine.Components.Graphics3D;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Framework.Services;
using Evergine.Mathematics;
using RoboticArm.Services;
using System;

namespace RoboticArm
{
    public class MyScene : Scene
    {
        public override void RegisterManagers()
        {
            base.RegisterManagers();
            
            this.Managers.AddManager(new global::Evergine.Bullet.BulletPhysicManager3D());
            
        }

        protected override void Update(TimeSpan gameTime)
        {
            base.Update(gameTime);


        }

        protected override void CreateScene()
        {
        }
    }
}


