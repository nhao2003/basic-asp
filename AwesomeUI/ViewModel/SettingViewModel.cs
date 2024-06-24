using AwesomeUI.View;
using Prism.Commands;

namespace AwesomeUI.ViewModel;

public class SettingItem(string title, string description, string icon)
{
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public string Icon { get; set; } = icon;
}

public class SettingViewModel : BaseViewModel
{
    public SettingViewModel()
    {
        Title = "Settings";
        TappedCommand = new DelegateCommand<SettingItem>((setting) => OnTapped(setting));
    }
    
    private readonly List<SettingItem> _settings =
    [
        new SettingItem("Account", "Manage your account", "account_icon.svg"),
        new SettingItem("Notifications", "Control your notifications", "notification_icon.svg"),
        new SettingItem("Privacy", "Manage your privacy settings", "privacy_icon.svg"),
        new SettingItem("Security", "Manage your security settings", "security_icon.svg"),
        new SettingItem("About", "Learn more about the app", "about_icon.svg"),
        new SettingItem("Help", "Get help with the app", "help_icon.svg"),
        new SettingItem("Sign Out", "Sign out of the app", "sign_out_icon.svg")
    ];
    
    public List<SettingItem> Settings => _settings;
    public DelegateCommand<SettingItem> TappedCommand { get; set; }
    private async Task OnTapped(SettingItem setting)
    {
        switch (setting.Title)
        {
            case "Account":
                // Navigate to AccountPage
                await Shell.Current.Navigation.PushAsync(new AccountPage());
                return;
            case "Notifications":
                // Navigate to NotificationsPage
                break;
            case "Privacy":
                // Navigate to PrivacyPage
                break;
            case "Security":
                // Navigate to SecurityPage
                break;
            case "About":
                // Navigate to AboutPage
                break;
            case "Help":
                // Navigate to HelpPage
                break;
            case "Sign Out":
                var result = await Shell.Current.DisplayAlert("Sign Out", "Are you sure you want to sign out?", "Yes", "No");
                if (result)
                {
                    await Shell.Current.GoToAsync("//SignInPage");
                }
                return;
        }
        await Shell.Current.DisplayAlert("Tapped", $"You tapped on {setting.Title}", "OK");
    }
    
    
    
    
    
}