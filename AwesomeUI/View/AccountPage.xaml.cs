using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeUI.View;

public partial class AccountPage : ContentPage
{
    private AccountViewModel? ViewModel => BindingContext as AccountViewModel;
    public AccountPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current?.Services.GetService<AccountViewModel>();
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel?.GetUser();
    }
}