using AwesomeUI.Config;

namespace AwesomeUI.Services;

public abstract class BaseService(HttpClient httpClient, IConnectivity connectivity)
{
    private readonly string _emulatedBaseUrl = AppConfig.EmulatedBaseUrl;
    private readonly string _realBaseUrl = AppConfig.RealBaseUrl;

    protected string GetBaseUrl() => DeviceInfo.DeviceType == DeviceType.Virtual ? _emulatedBaseUrl : _realBaseUrl;

    protected readonly HttpClient HttpClient = httpClient;
    protected readonly IConnectivity Connectivity = connectivity;
}