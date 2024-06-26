using Microsoft.Maui.Controls;

namespace AwesomeUI.Controls;

public partial class Entry : Grid
{
	public Entry()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        propertyName: nameof(Text),
        returnType: typeof(string),
        declaringType: typeof(Entry),
        defaultValue: null,
        defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
		propertyName: nameof(Placeholder),
		returnType: typeof(string),
		declaringType: typeof(Entry),
		defaultValue: null,
		defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        propertyName: nameof(IsPassword),
        returnType: typeof(bool),
        declaringType: typeof(Entry),
        defaultValue: null,
        defaultBindingMode: BindingMode.TwoWay);

    public string Text 
	{ 
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
	{
		LblPlaceholder.FontSize = 11;
		LblPlaceholder.TranslateTo(0, -26, 250, easing: Easing.SpringIn);
	}

	private void Entry_Unfocused(object sender, FocusEventArgs e)
	{
		if (!string.IsNullOrEmpty(Text))
		{
            LblPlaceholder.FontSize = 11;
            LblPlaceholder.TranslateTo(0, -26, 250, easing: Easing.SpringIn);
        }
		else {
            LblPlaceholder.FontSize = 15;
            LblPlaceholder.TranslateTo(0, 0, 500, easing: Easing.SpringOut);
        }

    }
}