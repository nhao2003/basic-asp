namespace AwesomeUI.Data.Remote;

public abstract class BaseRemoteData(HttpClient httpClient)
{
    protected readonly string BaseUrl = "http://10.0.2.2:8000/api";
    protected readonly HttpClient HttpClient = httpClient;
}