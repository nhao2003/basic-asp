﻿using System.Text.RegularExpressions;
using AwesomeUI.DTO.Auth;
using AwesomeUI.Services;
using AwesomeUI.View;
using Prism.Commands;

namespace AwesomeUI.ViewModel;

public partial class SignUpViewModel: BaseViewModel
{
    
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;
    
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
    
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }
    
    public DelegateCommand SignUpCommand { get; }
    
    private readonly AuthService _authService;
    public SignUpViewModel(AuthService authService) 
    {
        SignUpCommand = new DelegateCommand(OnSignUp);
        _authService = authService;
    }
    
    private const string ErrorTitle = "Error!";
    
    private readonly Regex _passwordRegex = MyRegex(TimeSpan.FromSeconds(1));
    
    private async void OnSignUp()
    {
        if (IsBusy)
            return;
        
        try
        {
            IsBusy = true;
            
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Shell.Current.DisplayAlert(ErrorTitle, "Username, password and confirm password are required.", "OK");
                return;
            }
            
            if (!_passwordRegex.IsMatch(Password))
            {
                await Shell.Current.DisplayAlert(ErrorTitle, "Password must be 8-32 characters long and contain at least one lowercase letter, one uppercase letter, one digit and one special character.", "OK");
                return;
            }
            
            if (Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert(ErrorTitle, "Password and confirm password do not match.", "OK");
                return;
            }
            
            var error = await _authService.SignUpAsync(new AuthRequest(Username, Password));
            if (error != null)
            {
                await Shell.Current.DisplayAlert(ErrorTitle, error, "OK");
                return;
            }
            
            await Shell.Current.GoToAsync($"//MainPage/{nameof(HomePage)}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(ErrorTitle, ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static Regex MyRegex(TimeSpan matchTimeout)
    {
        return new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,32}$", RegexOptions.Compiled, matchTimeout);
    }
}