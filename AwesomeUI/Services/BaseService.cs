namespace AwesomeUI.Services;

public abstract class BaseService(HttpClient httpClient)
{
    protected string BaseUrl = "http://192.168.1.30:8000/api";
    protected readonly HttpClient HttpClient = httpClient;
}