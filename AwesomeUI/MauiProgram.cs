using AwesomeUI.Data.Local;
using AwesomeUI.Data.Remote;
using AwesomeUI.Services;
using AwesomeUI.View;
using Microsoft.Extensions.Logging;
using SQLite;

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
                fonts.AddFont("Poppins-Black.ttf", "PoppinsBlack");
                fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
                fonts.AddFont("Inter-Regular.ttf", "Inter");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");

                //FontAwesome Icons
                fonts.AddFont("fa-brands-400.otf", "FaBrands");
                fonts.AddFont("fa-regular-400.otf", "FaRegular");
                fonts.AddFont("fa-solid-900.otf", "FaSolid");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        builder.Services.AddSingleton<SQLiteAsyncConnection>(_ =>
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AwesomeUI.db3");
            return new LocalDatabase(dbPath);
        });
        
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton(Geolocation.Default);
        builder.Services.AddSingleton(Map.Default);
        
        builder.Services.AddSingleton<IBlogLocalData, BlogLocalData>();
        builder.Services.AddSingleton<IBlogRemoteData, BlogRemoteData>();
        
		      
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