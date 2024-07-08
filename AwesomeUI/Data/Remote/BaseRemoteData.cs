using Microsoft.Extensions.Configuration;

namespace AwesomeUI.Data.Remote;

public abstract class BaseRemoteData(HttpClient httpClient, IConfiguration configuration)
{
    private readonly string _emulatedBaseUrl = configuration["EmulatedBaseUrl"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string _realBaseUrl = configuration["RealBaseUrl"] ?? throw new ArgumentNullException(nameof(configuration));

    protected string BaseUrl() => DeviceInfo.Current.DeviceType == DeviceType.Virtual ? _emulatedBaseUrl : _realBaseUrl;
    protected readonly HttpClient HttpClient = httpClient;
}