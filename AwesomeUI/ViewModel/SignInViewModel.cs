using AwesomeUI.DTO.Auth;
using AwesomeUI.Services;
using AwesomeUI.View;
using Prism.Commands;

namespace AwesomeUI.ViewModel;

public partial class SignInViewModel : BaseViewModel
{
    private readonly AuthService _authService;
    public DelegateCommand SignInCommand { get; private set; }
    public DelegateCommand GoToSignUpCommand { get; private set; }
    
    private IConnectivity _connectivity;
    public SignInViewModel(AuthService authService, IConnectivity connectivity)
    {
        _authService = authService;
        Title = "Sign In";
        SignInCommand = new DelegateCommand(async () => await SignInAsync());
        GoToSignUpCommand = new DelegateCommand(async () => await GoToSignUpAsync());
        _connectivity = connectivity;
    }

     private string _username = "abcd@abc.com";
     private string _password = "12345678";
     
     public string Username
     {
         get => _username;
         set => SetProperty(ref _username, value);
     }
     
     public string Password
     {
         get => _password;
         set => SetProperty(ref _password, value);
     }

    async Task SignInAsync()
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("No connectivity!",
                $"Please check internet and try again.", "OK");
            return;
        }

        
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error!", "Username and password are required.", "OK");
                return;
            }
            
            var isSignedIn = await _authService.SignInAsync(new AuthRequest(Username, Password));

            if (isSignedIn)
            {
                await Shell.Current.GoToAsync($"//MainPage/{nameof(HomePage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error!", "Invalid username or password.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to sign in: {ex.ToString()}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    async Task GoToSignUpAsync()
    {
        await Shell.Current.Navigation.PushAsync(new SignUpPage());
    }
    
}