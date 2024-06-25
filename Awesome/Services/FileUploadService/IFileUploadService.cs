namespace Awesome.Services.FileUploadService;

public interface IFileUploadService
{
    Task<string> UploadFileAsync(string folder, string fileName, Stream stream);
}