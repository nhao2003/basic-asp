namespace AwesomeUI.Data.Remote;

public abstract class BaseRemoteData(HttpClient httpClient)
{
    private static string emulatedBaseUrl = "http://10.0.2.2:8000/api";
    private static string realBaseUrl = "http://192.168.1.14:8000/api";
    protected readonly string BaseUrl = DeviceInfo.Current.DeviceType == DeviceType.Virtual ? emulatedBaseUrl : realBaseUrl;
    protected readonly HttpClient HttpClient = httpClient;
}