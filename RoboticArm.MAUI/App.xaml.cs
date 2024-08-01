using System.Numerics;

namespace RoboticArm.MAUI
{
    public partial class App : Application
    {
        public bool ENGINEERING = true;

        public int SPEED
        {
            get => Preferences.Get(nameof(SPEED), 20);
            set => Preferences.Set(nameof(SPEED), value);
        }

        public string IDCURRENTTOOL
        {
            get => Preferences.Get(nameof(IDCURRENTTOOL), string.Empty);
            set => Preferences.Set(nameof(IDCURRENTTOOL), value);
        }

        public static int PULSEREV = 200;

        public static int MINIMUMMICROSECONDS = 50;

        public enum DATATYPE { BLOCK, MOVE, DELAY, IF, WHILE }

        public int analogInputOneValue = 0;
        public int analogInputTwoValue = 0;

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzExNjQ2M0AzMjM0MmUzMDJlMzBJZGl0WGFyTDB1aFZMRkdKU1l5ckZSckVseWdXUXBxSWgxSEU0UFlDSGhZPQ==");

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}