using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Framework.Services;
using Evergine.Mathematics;
using RoboticArm.Models;
using System.Collections.Generic;
using System.Linq;

namespace RoboticArm.Services
{
    public class ControllerService : Service
    {
        public enum MOVEMENT { BASE, TOOL };

        public Dictionary<ROBOT, JointModel> DictEntities;

        public EntityModel AxisTool, AxisRobot, Target;

        public List<ToolModel> LstTools;

        public CameraControlModel CameraControl;

        float tolerance = 0.001f;

        Vector3 axisToolInitPosition;

        protected override void Start()
        {
            base.Start();
            var screenContextManager = Application.Current.Container.Resolve<ScreenContextManager>();
            LstTools = new List<ToolModel>();

            screenContextManager.OnActivatingScene += (scene) =>
            {
                DictEntities = new Dictionary<ROBOT, JointModel>
                {
                    {ROBOT.BASE,   new JointModel(scene.Managers.EntityManager.FindAllByTag("Base").First(),  Vector3.UnitY)},
                    {ROBOT.AXIS1,  new JointModel(scene.Managers.EntityManager.FindAllByTag("Axis1").First(), Vector3.UnitY, -125, 125)},
                    {ROBOT.AXIS2,  new JointModel(scene.Managers.EntityManager.FindAllByTag("Axis2").First(), Vector3.UnitX, -110, 110)},
                    {ROBOT.AXIS3,  new JointModel(scene.Managers.EntityManager.FindAllByTag("Axis3").First(), Vector3.UnitX, -120, 120)},
                    {ROBOT.AXIS4,  new JointModel(scene.Managers.EntityManager.FindAllByTag("Axis4").First(), Vector3.UnitY, -180, 180)},
                    {ROBOT.AXIS5,  new JointModel(scene.Managers.EntityManager.FindAllByTag("Axis5").First(), Vector3.UnitX, -110, 110)}
                };
                
                LstTools.Add(new ToolModel( entity: scene.Managers.EntityManager.FindAllByTag("Claw").First(), "CLAWBCN3D0001", tipPosition: new Vector3(0, 0.042f, 0)));

                Target = new EntityModel(scene.Managers.EntityManager.FindAllByTag("Target").First());
                AxisRobot = new EntityModel(scene.Managers.EntityManager.FindAllByTag("Axis_Robot").First());
                AxisTool = new EntityModel(scene.Managers.EntityManager.FindAllByTag("Axis_Tool").First());
                axisToolInitPosition = AxisTool.rotate.Transform.LocalPosition;

                LstTools[0].entity.IsEnabled = false;
                AxisTool.entity.IsEnabled = false;
                AxisRobot.entity.IsEnabled = true;

                CameraControl = new CameraControlModel(scene.Managers.EntityManager.FindAllByTag("CameraControl").First());
            };        
        }

        public void StartBlinking(ROBOT axis) => DictEntities[axis].blink.StartBlinking();

        public void StopBlinking(ROBOT axis) => DictEntities[axis].blink.StopBlinking();

        public bool IsBlinking(ROBOT axis) => DictEntities[axis].blink.isBlinking;

        public void Colliding(ROBOT axis, bool colliding) => DictEntities[axis].blink.Colliding(colliding);

        public bool IsColliding(ROBOT axis) => DictEntities[axis].blink.isColliding;

        public float Rotation(ROBOT axis, DIRECTION dir) => DictEntities[axis].AddRotation(dir);

        public void TranslateAxisTool(Vector3 trans) => AxisTool.rotate.Transform.LocalPosition = Vector3.Zero + trans;

        public void SetToolSelected(string newID, string oldID)
        {
            if(string.IsNullOrEmpty(newID)) AxisTool.rotate.Transform.LocalPosition = axisToolInitPosition;
            else
            {
                var tool = LstTools.Where(t => t.ID == newID).FirstOrDefault();
                if (tool != null)
                {
                    tool.entity.IsEnabled = true;
                    AxisTool.rotate.Transform.LocalPosition = tool.tipPosition;
                }
            }
            

            var oldTool = LstTools.Where(t => t.ID == oldID).FirstOrDefault();
            if (oldTool != null) oldTool.entity.IsEnabled = false;
        }

        public void CartesianMovement(string movement, bool toolReference)
        {
            Vector3 pos;

            if (toolReference)
            {
                pos = AxisTool.rotate.Transform.LocalPosition;
                var currentPos = new Vector3(pos.X, pos.Y, pos.Z);
                
                //I need to correct the rotation of the robotic arm to avoid doing this trick ¯\_(ツ)_/¯
                if (movement.Equals("XPositive")) pos.X -= 0.01f;
                else if (movement.Equals("XNegative")) pos.X += 0.01f;
                else if (movement.Equals("YPositive")) pos.Z -= 0.01f;
                else if (movement.Equals("YNegative")) pos.Z += 0.01f;
                else if (movement.Equals("ZPositive")) pos.Y += 0.01f;
                else if (movement.Equals("ZNegative")) pos.Y -= 0.01f;

                AxisTool.rotate.Transform.LocalPosition = pos;
                Target.rotate.Transform.Position = AxisTool.rotate.Transform.Position;
                AxisTool.rotate.Transform.LocalPosition = currentPos;

                bool result = CCD(true);
            }
            else
            {
                pos = AxisTool.rotate.Transform.Position;

                //I need to correct the rotation of the robotic arm to avoid doing this trick ¯\_(ツ)_/¯
                if (movement.Equals("XPositive")) pos.X -= 0.01f;
                else if (movement.Equals("XNegative")) pos.X += 0.01f;
                else if (movement.Equals("YPositive")) pos.Z -= 0.01f;
                else if (movement.Equals("YNegative")) pos.Z += 0.01f;
                else if (movement.Equals("ZPositive")) pos.Y += 0.01f;
                else if (movement.Equals("ZNegative")) pos.Y -= 0.01f;

                Target.rotate.Transform.Position = pos;
                bool result = CCD();
            }
        }

        public bool CCD(bool oriented = false)
        {
            //Store the original angles of each axis
            foreach (var axis in DictEntities) axis.Value.rotate.SaveAngle();
            
            for (var repeat = 0; repeat < 20; repeat++)
            {
                var result = IKSolve(oriented);
                if (result)
                {
                    if (DetectCollision())
                    {
                        break;
                    }
                    else return true;
                }
            }

            //Restore the original angles
            foreach (var axis in DictEntities) axis.Value.rotate.RestoreAngle();
            Target.rotate.Transform.Position = AxisTool.rotate.Transform.Position;

            return false;
        }

        private bool IKSolve(bool oriented = false)
        {
            Vector3 targetPosition = Target.rotate.Transform.Position;
            Vector3 toolDirectionSalved = AxisTool.rotate.Transform.Position - DictEntities[ROBOT.AXIS5].rotate.Transform.Position;
            toolDirectionSalved = Vector3.Normalize(toolDirectionSalved);

            for (var i = DictEntities.Count - 1; i > 0; i--)
            {
                Vector3 tooltipPosition = AxisTool.rotate.Transform.Position;
                var entity = DictEntities.ElementAt(i).Value;
                Transform3D transform = entity.rotate.Transform;
                Vector3 axis = entity.rotate.axis;

                if (oriented && i == DictEntities.Count - 1)
                {
                    // Rotate to align with a direction
                    Vector3 toolDirection = AxisTool.rotate.Transform.Position - DictEntities[ROBOT.AXIS5].rotate.Transform.Position;
                    transform.Orientation = MathService.CreateFromTwoVectors(toolDirection, toolDirectionSalved) * transform.Orientation;
                }

                //Rotate towards the Target
                else transform.Orientation = MathService.CreateFromTwoVectors(tooltipPosition - transform.Position, targetPosition - transform.Position) * transform.Orientation;

                //Only rotating with the hinge
                Transform3D parent = DictEntities.ElementAt(i - 1).Value.rotate.Transform;
                transform.Orientation = MathService.CreateFromTwoVectors(transform.Orientation * axis, parent.Orientation * axis) * transform.Orientation;

                //Limit angle to limits
                entity.rotate.Clamp();

                tooltipPosition = AxisTool.rotate.Transform.Position;
                if (Vector3.Distance(tooltipPosition, targetPosition) < tolerance) return true;
            }

            return false;
        }

        private bool DetectCollision()
        {
            /* I couldn't get collisions to work correctly  (ಥ_ಥ)
             * Two problems occurring:
             *          False start collision
             *          End Collision is not called sometimes
             *          I must be missing something ¯\_(ツ)_/¯
             */

            foreach (var axis in DictEntities)
                if (axis.Value.IsColliding()) return true;

            return false;
        }

        public void MovementReference(MOVEMENT mov)
        {
            if(mov == MOVEMENT.BASE)
            {
                AxisTool.entity.IsEnabled = false;
                AxisRobot.entity.IsEnabled = true;
            }
            else
            {
                AxisTool.entity.IsEnabled = true;
                AxisRobot.entity.IsEnabled = false;
            }
        }
    }
}
