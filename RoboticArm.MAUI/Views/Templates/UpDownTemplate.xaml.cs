namespace RoboticArm.MAUI.Views.Templates;

public partial class UpDownTemplate : Grid
{
	public UpDownTemplate()
	{
		InitializeComponent();
	}

    public string LblValue
    {
        get { return (string)GetValue(LblValueProperty); }
        set { SetValue(LblValueProperty, value); }
    }
    public static readonly BindableProperty LblValueProperty = BindableProperty.Create(nameof(LblValue), typeof(string), typeof(UpDownTemplate), string.Empty);
}