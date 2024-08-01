using RoboticArm.MAUI.Models;
using RoboticArm.Models;
using RoboticArm.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using RoboticArm.MAUI.Helpers;

namespace RoboticArm.MAUI.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<Models.JointModel> LstJoints
        {
            get
            {
                return this.lstJoints;
            }

            set
            {
                if (this.lstJoints == value)
                {
                    return;
                }
                this.lstJoints = value;
                this.NotifyPropertyChanged();
            }
        }
        private ObservableCollection<Models.JointModel> lstJoints;

        public ObservableCollection<InfoToolModel> LstTInfoTools
        {
            get
            {
                return this.lstTInfoTools;
            }

            set
            {
                if (this.lstTInfoTools == value)
                {
                    return;
                }
                this.lstTInfoTools = value;
                this.NotifyPropertyChanged();
            }
        }
        private ObservableCollection<InfoToolModel> lstTInfoTools;

        public InfoToolModel ToolInfoSelected
        {
            get => toolInfoSelected;
            set
            {
                if (toolInfoSelected != value)
                {
                    toolInfoSelected = value;
                    this.NotifyPropertyChanged();
                    string newID = value?.ID;
                    controllerService.SetToolSelected(newID, ((App)App.Current).IDCURRENTTOOL);
                    ((App)App.Current).IDCURRENTTOOL = newID;
                }
            }
        }
        private InfoToolModel toolInfoSelected;

        public int SelectedCoord
        {
            get
            {
                return this.selectedCoord;
            }
            set
            {
                if (this.selectedCoord == value) return;
                this.selectedCoord = value;
                if(this.selectedCoord == 0)
                {
                    BaseSelected = true;
                    ToolSelected = false;
                    controllerService.MovementReference(ControllerService.MOVEMENT.BASE);
                }
                else
                {
                    BaseSelected = false;
                    ToolSelected = true;
                    controllerService.MovementReference(ControllerService.MOVEMENT.TOOL);
                }

                this.NotifyPropertyChanged();
            }
        }
        private int selectedCoord;

        public bool BaseSelected
        {
            get
            {
                return this.baseSelected;
            }
            set
            {
                if (this.baseSelected == value) return;
                this.baseSelected = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool baseSelected;

        public bool ToolSelected
        {
            get
            {
                return this.toolSelected;
            }
            set
            {
                if (this.toolSelected == value) return;
                this.toolSelected = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool toolSelected;

        public int SelectedMovement
        {
            get
            {
                return this.selectedMovement;
            }
            set
            {
                if (this.selectedMovement == value) return;
                this.selectedMovement = value;
                if (this.selectedMovement == 0)
                {
                    JointsSelected = true;
                    CoordsSelected = false;
                    
                }
                else
                {
                    JointsSelected = false;
                    CoordsSelected = true;
                }
                this.NotifyPropertyChanged();
            }
        }
        private int selectedMovement;

        public bool JointsSelected
        {
            get
            {
                return this.jointsSelected;
            }
            set
            {
                if (this.jointsSelected == value) return;
                this.jointsSelected = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool jointsSelected;

        public bool CoordsSelected
        {
            get
            {
                return this.coordsSelected;
            }
            set
            {
                if (this.coordsSelected == value) return;
                this.coordsSelected = value;
                this.NotifyPropertyChanged();
            }
        }
        private bool coordsSelected;

        public bool ProgramPressed
        {
            get { return programPressed; }
            set
            {
                if (this.programPressed != value)
                {
                    this.programPressed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }
        private bool programPressed;

        public bool ControllerPressed
        {
            get { return controllerPressed; }
            set
            {
                if (this.controllerPressed != value)
                {
                    this.controllerPressed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }
        private bool controllerPressed;

        public bool ExecutePressed
        {
            get { return executePressed; }
            set
            {
                if (this.executePressed != value)
                {
                    this.executePressed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }
        private bool executePressed;

        public int SelectedMove
        {
            get
            {
                return this.selectedMove;
            }
            set
            {
                if (this.selectedMove == value) return;
                this.selectedMove = value;
                this.NotifyPropertyChanged();
            }
        }
        private int selectedMove;

        public bool AnalogButtonOne
        {
            get { return analogButtonOne; }
            set
            {
                if (this.analogButtonOne != value)
                {
                    this.analogButtonOne = value;
                    this.NotifyPropertyChanged();
                }
            }
        }
        private bool analogButtonOne;

        public bool AnalogButtonTwo
        {
            get { return analogButtonTwo; }
            set
            {
                if (this.analogButtonTwo != value)
                {
                    this.analogButtonTwo = value;
                    this.NotifyPropertyChanged();
                }
            }
        }
        private bool analogButtonTwo;

        IDispatcherTimer timerRotateAxis, timerCartesian;

        Models.JointModel selectedJoint;

        private ControllerService controllerService;

        CancellationTokenSource cancelTimerRotateAxis, cancelTimerCartesian;

        public Command CWPressedCommand { get; set; }

        public Command CWReleaseCommand { get; set; }

        public Command CCWPressedCommand { get; set; }

        public Command CCWReleaseCommand { get; set; }

        public Command CartesianPressedCommand { get; set; }

        public Command CartesianReleasedCommand { get; set; }

        public Command ViewCommand { get; set; }

        public ICommand CommandTooglePressed
        {
            get
            {
                return new Command((value) =>
                {
                    switch ((string)value)
                    {
                        case "MenuMove":
                            CLearMainMenu();
                            ControllerPressed = true;
                            break;

                        case "MenuProgram":
                            CLearMainMenu();
                            ProgramPressed = true;
                            break;

                        case "MenuExecute":
                            CLearMainMenu();
                            ExecutePressed = true;
                            break;
                    }
                });
            }
        }

        public void SetControllerService(ControllerService controllerService) => this.controllerService = controllerService;

        private void CLearMainMenu()
        {
            if (ProgramPressed) ProgramPressed = false;
            if (ControllerPressed) ControllerPressed = false;
            if (ExecutePressed) ExecutePressed = false;
        }

        public MainPageViewModel() 
        {
            CWPressedCommand = new Command<Models.JointModel>(CWPressedClicked);
            CWReleaseCommand = new Command<Models.JointModel>(CWReleaseClicked);
            CCWPressedCommand = new Command<Models.JointModel>(CCWPressedTapped);
            CCWReleaseCommand = new Command<Models.JointModel>(CCWReleaseTapped);
            CartesianPressedCommand = new Command<string>(CartesianPressed);
            CartesianReleasedCommand = new Command<string>(CartesianRelease);
            ViewCommand = new Command<string>(ViewPressed);

            LstJoints = new ObservableCollection<Models.JointModel>
            {
                new Models.JointModel { Name = "J1", Joint = ROBOT.AXIS1 },
                new Models.JointModel { Name = "J2", Joint = ROBOT.AXIS2 },
                new Models.JointModel { Name = "J3", Joint = ROBOT.AXIS3 },
                new Models.JointModel { Name = "J4", Joint = ROBOT.AXIS4 },
                new Models.JointModel { Name = "J5", Joint = ROBOT.AXIS5 }
            };

            LstTInfoTools = new ObservableCollection<InfoToolModel>()
            {
                new InfoToolModel
                {
                    ID = "CLAWBCN3D0001",
                    Name = "Printed Claw",
                    Description = "Garra projetada pela BCN3D Technologies para impressão.\n\n" +
                    "Garra aberta: 15cm",
                    Image = "claw_front.png"
                }
            };

            SelectedCoord = 0;
            BaseSelected = true;
            SelectedMovement = 0;
            JointsSelected = true;
            cancelTimerRotateAxis = new CancellationTokenSource();

            ((App)App.Current).IDCURRENTTOOL = "";

        }

        private void ViewPressed(string direction) => controllerService.CameraControl.SetView(direction);

        private void CWPressedClicked(Models.JointModel selectedJoint)
        {
            this.selectedJoint = selectedJoint;
            cancelTimerRotateAxis = new CancellationTokenSource();
            RotateAxis(DIRECTION.CLOCKWISE);
            float currentAngle = controllerService.Rotation(selectedJoint.Joint, DIRECTION.CLOCKWISE);
            selectedJoint.Angle = currentAngle;
            selectedJoint.IsLimited = controllerService.IsColliding(selectedJoint.Joint);
        }

        private void CWReleaseClicked(Models.JointModel selectedJoint)
        {
            this.selectedJoint = selectedJoint;
            cancelTimerRotateAxis.Cancel();
            controllerService.Colliding(selectedJoint.Joint, false);
            if (timerRotateAxis != null && timerRotateAxis.IsRunning) timerRotateAxis.Stop();
            if (controllerService.IsBlinking(selectedJoint.Joint)) controllerService.StopBlinking(selectedJoint.Joint);
            if (selectedJoint.IsLimited) selectedJoint.IsLimited = false;
        }

        private void CCWPressedTapped(Models.JointModel selectedJoint)
        {
            this.selectedJoint = selectedJoint;
            cancelTimerRotateAxis = new CancellationTokenSource();
            RotateAxis(DIRECTION.COUNTERCLOCKWISE);
            float currentAngle = controllerService.Rotation(selectedJoint.Joint, DIRECTION.COUNTERCLOCKWISE);
            selectedJoint.Angle = currentAngle;
            selectedJoint.IsLimited = controllerService.IsColliding(selectedJoint.Joint);
        }

        private void CCWReleaseTapped(Models.JointModel selectedJoint)
        {
            this.selectedJoint = selectedJoint;
            cancelTimerRotateAxis.Cancel();
            controllerService.Colliding(selectedJoint.Joint, false);
            if (timerRotateAxis != null && timerRotateAxis.IsRunning) timerRotateAxis.Stop();
            if (controllerService.IsBlinking(selectedJoint.Joint)) controllerService.StopBlinking(selectedJoint.Joint);
            if (selectedJoint.IsLimited) selectedJoint.IsLimited = false;
        }

        private void CartesianPressed(string movement)
        {
            cancelTimerCartesian = new CancellationTokenSource();
            CartesianMovement(movement);
            controllerService.CartesianMovement(movement, ToolSelected);
        }

        private void CartesianRelease(string movement)
        {
            cancelTimerCartesian.Cancel();
            if (timerCartesian != null && timerCartesian.IsRunning) timerCartesian.Stop();
        }

        private async void RotateAxis(DIRECTION dir)
        {
            try { await Task.Delay(1000, cancelTimerRotateAxis.Token); }
            catch { return; }

            controllerService.StartBlinking(selectedJoint.Joint);
            var period = ((App.MINIMUMMICROSECONDS * 100) / ((App)App.Current).SPEED);
            var ts = TimeSpan.FromMilliseconds(period);
            var dispatcher = Application.Current.Dispatcher;
            timerRotateAxis = dispatcher.CreateTimer();
            timerRotateAxis.Interval = ts;
            timerRotateAxis.Tick += (s, e) =>
            {
                float currentAngle;
                if (dir == DIRECTION.COUNTERCLOCKWISE) currentAngle = controllerService.Rotation(selectedJoint.Joint, DIRECTION.COUNTERCLOCKWISE);
                else currentAngle = controllerService.Rotation(selectedJoint.Joint, DIRECTION.CLOCKWISE);
                selectedJoint.Angle = currentAngle;
                selectedJoint.IsLimited = controllerService.IsColliding(selectedJoint.Joint);
            };
            timerRotateAxis.Start();
        }

        private async void CartesianMovement(string movement)
        {
            try { await Task.Delay(1000, cancelTimerCartesian.Token); }
            catch { return; }

            var period = ((App.MINIMUMMICROSECONDS * 100) / ((App)App.Current).SPEED);
            var ts = TimeSpan.FromMilliseconds(period);
            var dispatcher = Application.Current.Dispatcher;
            timerCartesian = dispatcher.CreateTimer();
            timerCartesian.Interval = ts;
            timerCartesian.Tick += (s, e) =>
            {
                controllerService.CartesianMovement(movement, ToolSelected);
            };
            timerCartesian.Start();
        }
    }
}
