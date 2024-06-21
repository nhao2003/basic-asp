using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeUI.View;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
        Debug.Assert(IPlatformApplication.Current != null, "IPlatformApplication.Current != null");
        BindingContext = IPlatformApplication.Current.Services.GetService<SignUpViewModel>();
    }
}