using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Warehouse.Infrastructure.Storage;

public class MinioStorageService:IStorageService
{
    private readonly IMinioClient _minioClient; //object that knows how to communicate with MinIO.
    private readonly string _bucketName;

    public MinioStorageService(IMinioClient minioClient, IConfiguration configuration)
    {
        _minioClient = minioClient;

        _bucketName = configuration["MinIO:BucketName"];
    }
    
    public async Task<string> UploadAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
    {
        var objectName = $"{Guid.NewGuid()}_{fileName}";
        await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length),
            cancellationToken);

        return objectName;
    }

    public async Task<Stream> DownloadAsync(string fileName, CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();

        await _minioClient.GetObjectAsync(
            new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memoryStream);
                }),
            cancellationToken);

        memoryStream.Position = 0;

        return memoryStream;
    }
}