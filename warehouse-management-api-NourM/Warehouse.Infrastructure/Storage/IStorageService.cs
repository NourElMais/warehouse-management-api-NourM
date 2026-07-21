namespace Warehouse.Infrastructure.Storage;

public interface IStorageService
{
    //Any storage system we use must be able to upload a file
    Task<string> UploadAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);
}