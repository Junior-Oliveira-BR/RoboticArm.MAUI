using Evergine.Framework;
using Evergine.Mathematics;
using RoboticArm.Behaviors;
using System.Net.Http.Headers;

namespace RoboticArm.Models
{
    public class ToolModel
    {
        public string ID;

        public Vector3 tipPosition;

        public Entity entity;

        public RotateBehavior rotate;

        public ToolModel(Entity entity, string ID, Vector3 tipPosition)
        {
            this.entity = entity;
            this.ID = ID;
            this.tipPosition = tipPosition;
            rotate = entity.FindComponent<RotateBehavior>();
        }
    }
}
