using System;
using System.Collections.Generic;
using System.Text;

namespace RoboticArm.Models
{
    public enum ROBOT
    {
        BASE, AXIS1, AXIS2, AXIS3, AXIS4, AXIS5, ENDEFFECTOR
    }

    public enum DIRECTION
    {
        CLOCKWISE, COUNTERCLOCKWISE
    }

    public static class Constants
    {
        public static float NULLANGLE = 400;

        public static float DELTATIME = 1.0f / 60.0f;
    }
}
