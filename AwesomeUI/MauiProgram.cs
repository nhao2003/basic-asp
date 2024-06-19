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
        builder.Services.AddSingleton<MonkeysViewModel>();
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddTransient<MonkeyDetailsViewModel>();
        builder.Services.AddTransient<DetailsPage>();

        return builder.Build();
    }
}