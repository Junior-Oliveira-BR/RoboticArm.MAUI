using Evergine.Components.Graphics3D;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Framework.Services;
using System;

namespace RoboticArm.Behaviors
{
    public class BlinkBehavior : Behavior
    {
        [BindComponent(false)]
        public MaterialComponent materialComponent = null;

        private TimeSpan currentTime;
        private Material RedBody;
        private Material GreenBody;
        private Material Original;
        public bool isBlinking, isColliding;

        public BlinkBehavior() { }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            this.isBlinking = false;
            this.isColliding = false;
        }

        protected override bool OnAttached()
        {
            var assetsService = Application.Current.Container.Resolve<AssetsService>();
            this.RedBody = assetsService.Load<Material>(EvergineContent.Materials.RedBody);
            this.GreenBody = assetsService.Load<Material>(EvergineContent.Materials.GreenBody);
            Original = materialComponent.Material;
            return base.OnAttached();
        }

        public void Colliding(bool isColliding)
        {
            this.isColliding = isColliding;
            if(isColliding) materialComponent.Material = RedBody;
            else materialComponent.Material = Original;
        }

        public void StartBlinking()
        {
            if (isBlinking) return;
            currentTime = TimeSpan.Zero;
            isBlinking = true;
            isColliding = false;
            materialComponent.Material = GreenBody;
        }

        public void StopBlinking()
        {
            if (!isBlinking) return;
            currentTime = TimeSpan.Zero;
            isBlinking = false;
            isColliding = false;
            if (!materialComponent.Material.Equals(Original)) materialComponent.Material = Original;
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (isBlinking)
            {
                currentTime += gameTime;
                if(currentTime.Milliseconds >= 500)
                {
                    currentTime = TimeSpan.Zero;
                    if (materialComponent.Material.Equals(Original)) materialComponent.Material = GreenBody;
                    else materialComponent.Material = Original;
                }
            }
        }
    }
}
