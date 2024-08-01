using RoboticArm.MAUI.Helpers;
using RoboticArm.Models;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace RoboticArm.MAUI.Models
{
    public class JointModel : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public float Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                OnPropertyChanged();
            }
        }
        private float angle;

        public bool IsLimited
        {
            get { return isLimited; }
            set
            {
                isLimited = value;
                OnPropertyChanged();
            }
        }
        private bool isLimited;

        public ROBOT Joint { get; set; }

        public JointModel() { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
