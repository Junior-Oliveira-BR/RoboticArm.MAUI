using System.Windows.Input;

namespace RoboticArm.MAUI.Views.Templates;

public partial class ButtonMenu : Grid
{
	public ButtonMenu()
	{
		InitializeComponent();
        this.BackgroundColor = Color.FromRgb(51, 51, 51);
    }

    public string IconSource
    {
        get { return _IconSource; }
        set
        {
            _IconSource = value;
            OnPropertyChanged();
        }
    }
    private string _IconSource;
    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IconSource),
        typeof(string),
        typeof(ButtonMenu),
        string.Empty,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var ctrl = (ButtonMenu)bindable;
            ctrl.IconSource = (string)newValue;
        },
        defaultBindingMode: BindingMode.OneWay);

    public string LblDescription
    {
        get { return _LblDescription; }
        set
        {
            _LblDescription = value;
            OnPropertyChanged();
        }
    }
    private string _LblDescription;
    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
        nameof(LblDescription),
        typeof(string),
        typeof(ButtonMenu),
        string.Empty,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var ctrl = (ButtonMenu)bindable;
            ctrl.LblDescription = (string)newValue;
        },
        defaultBindingMode: BindingMode.OneWay);

    public bool Pressed
    {
        get { return _Pressed; }
        set
        {
            if (_Pressed != value)
            {
                _Pressed = value;
                if (Toogle && !value)
                {
                    this.BackgroundColor = Color.FromRgb(51, 51, 51);
                }
                else if (Toogle && value)
                {
                    this.BackgroundColor = Color.FromRgb(124, 180, 255);
                }
                OnPropertyChanged();
            }
        }
    }
    private bool _Pressed;
    public static readonly BindableProperty PressedProperty = BindableProperty.Create(
        nameof(Pressed),
        typeof(bool),
        typeof(ButtonMenu),
        false,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var ctrl = (ButtonMenu)bindable;
            ctrl.Pressed = (bool)newValue;
        },
        defaultBindingMode: BindingMode.OneWay);

    public bool Toogle
    {
        get { return _Toggle; }
        set
        {
            if (_Toggle != value)
            {
                _Toggle = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _Toggle;
    public static readonly BindableProperty ToogleProperty = BindableProperty.Create(
        nameof(Toogle),
        typeof(bool),
        typeof(ButtonMenu),
        false,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var ctrl = (ButtonMenu)bindable;
            ctrl.Toogle = (bool)newValue;
        },
        defaultBindingMode: BindingMode.OneWay);

    public string CommandParameter
    {
        get { return _commandParameter; }
        set
        {
            _commandParameter = value;
            OnPropertyChanged();
        }
    }
    private string _commandParameter;
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(string),
        typeof(ButtonMenu),
        string.Empty,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var ctrl = (ButtonMenu)bindable;
            ctrl.CommandParameter = (string)newValue;
        },
        defaultBindingMode: BindingMode.OneWay);

    public ICommand CommandTooglePressed
    {
        get { return _commandTooglePressed; }
        set
        {
            _commandTooglePressed = value;
            OnPropertyChanged();
        }
    }
    private ICommand _commandTooglePressed;
    public static readonly BindableProperty CommandTooglePressedProperty = BindableProperty.Create(
        "CommandTooglePressed",
        typeof(ICommand),
        typeof(ButtonMenu),
        null,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var ctrl = (ButtonMenu)bindable;
            ctrl.CommandTooglePressed = (ICommand)newValue;
        },
        defaultBindingMode: BindingMode.OneWay);

    public ICommand CommandToogleLoose
    {
        get { return _commandToogleLoose; }
        set
        {
            _commandToogleLoose = value;
            OnPropertyChanged();
        }
    }
    private ICommand _commandToogleLoose;
    public static readonly BindableProperty CommandToogleLooseProperty = BindableProperty.Create(
        "CommandToogleLoose",
        typeof(ICommand),
        typeof(ButtonMenu),
        null,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var ctrl = (ButtonMenu)bindable;
            ctrl.CommandToogleLoose = (ICommand)newValue;
        },
        defaultBindingMode: BindingMode.OneWay);

    public ICommand CommandPulse
    {
        get { return _commandPulse; }
        set
        {
            _commandPulse = value;
            OnPropertyChanged();
        }
    }
    private ICommand _commandPulse;
    public static readonly BindableProperty CommandPulseProperty = BindableProperty.Create(
        "CommandPulse",
        typeof(ICommand),
        typeof(ButtonMenu),
        null,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var ctrl = (ButtonMenu)bindable;
            ctrl.CommandPulse = (ICommand)newValue;
        },
        defaultBindingMode: BindingMode.OneWay);

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (Toogle && Pressed) CommandToogleLoose?.Execute(CommandParameter);

        else if (Toogle && !Pressed) CommandTooglePressed?.Execute(CommandParameter);
        else
        {
            this.BackgroundColor = Color.FromRgb(124, 180, 255);
            await Task.Delay(80);
            this.BackgroundColor = Color.FromRgb(51, 51, 51);
            CommandPulse.Execute(CommandParameter);
        }
    }
}