namespace AwesomeUI.Services;

public abstract class BaseService(HttpClient httpClient)
{
    protected readonly string BaseUrl = "http://10.0.2.2:8000/api";
    protected readonly HttpClient HttpClient = httpClient;
}