namespace Durga.Api.Application.Ports;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    Task<string> UploadProfileImageAsync(Stream imageStream, string fileName, Guid userId);
    Task<Stream> DownloadFileAsync(string fileUrl);
    Task<bool> DeleteFileAsync(string fileUrl);
    Task<bool> FileExistsAsync(string fileUrl);
    Task<string> GetFileUrlAsync(string filePath);
    Task<long> GetFileSizeAsync(string fileUrl);
}