using RoboticArm.MAUI.ViewModels;
using RoboticArm.Services;

namespace RoboticArm.MAUI
{
    public partial class MainPage
    {
        private readonly MyApplication evergineApplication;
        private ControllerService controllerService;
        private MainPageViewModel viewModel;
        private double width;
        private double height;

        public MainPage()
        {
            viewModel = new MainPageViewModel();
            BindingContext = viewModel;

            InitializeComponent();
            evergineApplication = new MyApplication();
            
            SliderSpeed.Value = ((App)App.Current).SPEED;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            this.width = width;
            this.height = height;
            GridController.WidthRequest = width * 0.3f;
            MainEvergineView.WidthRequest = width * 0.3f;
            CodeLayout.WidthRequest = width * 0.4f;

            MainEvergineView.Application = evergineApplication;

            controllerService = MainEvergineView.Application.Container.Resolve<ControllerService>();
            viewModel.SetControllerService(this.controllerService);
        }

        private void ShowRobot_Clicked(object sender, EventArgs e)
        {
            if (!RobotLayout.IsVisible)
            {
                RobotLayout.IsVisible = true;
                CodeLayout.WidthRequest = width * 0.4f;
            }
            else
            {
                CodeLayout.IsVisible = false;
                MainEvergineView.WidthRequest = width * 0.7f;
            }
        }

        private void HideRobot_Clicked(object sender, EventArgs e)
        {
            if (!CodeLayout.IsVisible)
            {
                CodeLayout.IsVisible = true;
                MainEvergineView.WidthRequest = width * 0.3f;
                CodeLayout.WidthRequest = width * 0.4f;
            }
            else
            {
                RobotLayout.IsVisible = false;
                CodeLayout.WidthRequest = width * 0.7f;
            }
        }

        private void SliderSpeed_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int value = (int)e.NewValue;
            ((App)App.Current).SPEED = value;
            lblSpeed.Text = String.Format("Speed: {0}", value);
        }
    }
}