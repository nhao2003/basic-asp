using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeUI.View;

public partial class SettingPage : ContentPage
{
    public SettingPage(SettingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}