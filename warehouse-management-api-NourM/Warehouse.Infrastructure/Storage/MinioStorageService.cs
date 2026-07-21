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
        await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length),
            cancellationToken);

        return $"{_bucketName}/{fileName}";
    }
}