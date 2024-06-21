namespace AwesomeUI.View;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        Debug.Assert(IPlatformApplication.Current != null, "IPlatformApplication.Current != null");
        BindingContext = IPlatformApplication.Current.Services.GetService<HomeViewModel>();
    }
}

