using Microsoft.Extensions.Configuration;

namespace AwesomeUI.Services;

public abstract class BaseService(HttpClient httpClient, IConnectivity connectivity, IConfiguration configuration)
{
    private readonly string _emulatedBaseUrl = configuration["EmulatedBaseUrl"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string _realBaseUrl = configuration["RealBaseUrl"] ?? throw new ArgumentNullException(nameof(configuration));

    protected string GetBaseUrl() => DeviceInfo.DeviceType == DeviceType.Virtual ? _emulatedBaseUrl : _realBaseUrl;

    protected readonly HttpClient HttpClient = httpClient;
    protected readonly IConnectivity Connectivity = connectivity;
}