namespace Warehouse.Infrastructure.Storage;

public interface IStorageService
{
    //Any storage system we use must be able to upload and download a file
    Task<string> UploadAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);
    
    Task<Stream> DownloadAsync(string fileName, CancellationToken cancellationToken);
}