using AwesomeUI.Services;
using Prism.Commands;

namespace AwesomeUI.ViewModel;

public class SplashViewModel : BaseViewModel
{
    private readonly AuthService _authService;
    
    public SplashViewModel(AuthService authService)
    {
        _authService = authService;
        RefreshTokenCommand = new DelegateCommand(async () => await RefreshTokenAsync());
    }
    
    public DelegateCommand RefreshTokenCommand { get; set; }
    
    private async Task RefreshTokenAsync()
    {
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
}