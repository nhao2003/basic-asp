using AwesomeUI.Services;
using Prism.Commands;

namespace AwesomeUI.ViewModel;

public class SplashViewModel : BaseViewModel
{
    private readonly AuthService _authService;
    private readonly IConnectivity _connectivity;
    public SplashViewModel(AuthService authService, IConnectivity connectivity)
    {
        _authService = authService;
        _connectivity = connectivity;
        RefreshTokenCommand = new DelegateCommand(async () => await RefreshTokenAsync());
    }
    
    public DelegateCommand RefreshTokenCommand { get; set; }
    
    private async Task RefreshTokenAsync()
    {
        await RequestPermissionAsync();
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            var result = await Shell.Current.DisplayAlert("No connectivity!",
                $"Please check internet and try again.", "Try again", "Exit");
            if (!result)
            {
                Environment.Exit(0);
            }
            else
            {
                await RefreshTokenAsync();
            }
            return;
        }
        
        var success = await _authService.RefreshTokenAsync();
        if (success)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await Shell.Current.GoToAsync("//SignInPage");
        }
    }
    
    private async Task RequestPermissionAsync()
    {
        var cameraPermission = await Permissions.CheckStatusAsync<Permissions.Camera>();
        var storagePermission = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        var locationPermission = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        
        if (cameraPermission != PermissionStatus.Granted)
        {
             await Permissions.RequestAsync<Permissions.Camera>();
        }
        
        if (storagePermission != PermissionStatus.Granted)
        {
            await Permissions.RequestAsync<Permissions.StorageWrite>();
        }
        
        if (locationPermission != PermissionStatus.Granted)
        {
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }
    }
}