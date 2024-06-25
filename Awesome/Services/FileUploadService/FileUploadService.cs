using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Awesome.Services.FileUploadService;

public class FileUploadService(Cloudinary cloudinary) : IFileUploadService
{
    private readonly string[] _imageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".ico"];
    private readonly string[] _videoExtensions = [".mp4", ".webm", ".ogg", ".avi", ".mov", ".flv", ".wmv", ".mkv"];
    public Task<string> UploadFileAsync(string folder, string fileName, Stream stream)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        var isImage = _imageExtensions.Contains(extension);
        var isVideo = _videoExtensions.Contains(extension);
        if (!isImage && !isVideo)
        {
            throw new BadHttpRequestException("Invalid file type.");
        }
        var uploadParams = new RawUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = folder,
            Overwrite = true
        };
        var result = cloudinary.Upload(uploadParams);
        return Task.FromResult(result.SecureUrl.AbsoluteUri);
    }
}