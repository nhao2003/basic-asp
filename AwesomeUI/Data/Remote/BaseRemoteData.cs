using AwesomeUI.Config;
using Microsoft.Extensions.Configuration;

namespace AwesomeUI.Data.Remote;

public abstract class BaseRemoteData(HttpClient httpClient)
{
    private readonly string _emulatedBaseUrl = AppConfig.EmulatedBaseUrl;
    private readonly string _realBaseUrl = AppConfig.RealBaseUrl;

    protected string BaseUrl() => DeviceInfo.Current.DeviceType == DeviceType.Virtual ? _emulatedBaseUrl : _realBaseUrl;
    protected readonly HttpClient HttpClient = httpClient;
}