using Evergine.Framework;
using RoboticArm.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoboticArm.Models
{
    public class CameraControlModel
    {
        public CameraBehavior camera;

        public CameraControlModel(Entity entity) 
        {
            camera = entity.FindComponent<CameraBehavior>();
        }

        public void SetView(string direction) => camera.LookAt(direction);

    }
}
