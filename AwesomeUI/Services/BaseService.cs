using Microsoft.Extensions.Configuration;

namespace AwesomeUI.Services;

public abstract class BaseService(HttpClient httpClient, IConnectivity connectivity, IConfiguration configuration)
{
    // private static string emulatedBaseUrl = "http://10.0.2.2:8000/api";
    // private static string realBaseUrl = "http://192.168.1.14:8000/api";

    private readonly string _emulatedBaseUrl = configuration["EmulatedBaseUrl"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string _realBaseUrl = configuration["RealBaseUrl"] ?? throw new ArgumentNullException(nameof(configuration));

    protected string GetBaseUrl() => DeviceInfo.DeviceType == DeviceType.Virtual ? _emulatedBaseUrl : _realBaseUrl;

    protected readonly HttpClient HttpClient = httpClient;
    protected readonly IConnectivity Connectivity = connectivity;
}