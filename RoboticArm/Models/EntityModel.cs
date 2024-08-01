using Evergine.Framework;
using RoboticArm.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoboticArm.Models
{
    public class EntityModel
    {
        public Entity entity;
        public RotateBehavior rotate;

        public EntityModel(Entity entity)
        {
            this.entity = entity;
            rotate = entity.FindComponent<RotateBehavior>();
        }
    }
}
