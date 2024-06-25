using AwesomeUI.Services;
using AwesomeUI.View;
using Microsoft.Extensions.Logging;

namespace AwesomeUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton(Connectivity.Current);
        builder.Services.AddSingleton(Geolocation.Default);
        builder.Services.AddSingleton(Map.Default);
        
		      
        builder.Services.AddSingleton<BlogService>();
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<HomePage>();
        
        builder.Services.AddTransient<BlogDetailViewModel>();
        builder.Services.AddTransient<BlogDetailPage>();
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddTransient<SignInViewModel>();
        builder.Services.AddTransient<SignInPage>();
        
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddTransient<AccountViewModel>();
        builder.Services.AddTransient<AccountPage>();
        
        builder.Services.AddTransient<SignUpViewModel>();
        
        builder.Services.AddSingleton<SettingViewModel>();
        builder.Services.AddSingleton<SettingPage>();
        
        builder.Services.AddSingleton<SplashViewModel>();
        builder.Services.AddSingleton<SplashPage>();

        return builder.Build();
    }
}