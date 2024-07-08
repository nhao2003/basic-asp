namespace AwesomeUI.Config;

public abstract class AppConfig
{
    public static string EmulatedBaseUrl => "http://10.0.2.2:8000/api";
    public static string RealBaseUrl => "http://192.168.1.13:8000/api";
}