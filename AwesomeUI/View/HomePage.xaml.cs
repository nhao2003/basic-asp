namespace AwesomeUI.View;

public partial class HomePage : ContentPage
{
    private HomeViewModel? ViewModel => BindingContext as HomeViewModel;
    public HomePage()
    {
        InitializeComponent();
        Debug.Assert(IPlatformApplication.Current != null, "IPlatformApplication.Current != null");
        BindingContext = IPlatformApplication.Current.Services.GetService<HomeViewModel>();
        
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel?.Blogs.Count == 0)
            await ViewModel.GetBlogsAsync();
    }
}

