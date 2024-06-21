using AwesomeUI.View;

namespace AwesomeUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(BlogDetailPage), typeof(BlogDetailPage));
    }
}