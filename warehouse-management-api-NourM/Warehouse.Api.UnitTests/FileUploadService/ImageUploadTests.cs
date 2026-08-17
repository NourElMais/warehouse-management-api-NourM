using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Moq;
using Warehouse.Application.IntegrationEvents;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Products.Commands;
using Warehouse.Domain.ProductImages;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Infrastructure.Storage;

namespace Warehouse.Api.UnitTests.FileUploadService;

public class ImageUploadTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ILogger<UploadProductImageHandler>> _loggerMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly Mock<IRabbitMqPublisher> _publisherMock;
    private readonly UploadProductImageHandler _handler;

    public ImageUploadTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<UploadProductImageHandler>>();
        _storageServiceMock = new Mock<IStorageService>();
        _publisherMock = new Mock<IRabbitMqPublisher>();
        _handler = new UploadProductImageHandler(_productRepositoryMock.Object, _loggerMock.Object,
            _storageServiceMock.Object, _publisherMock.Object);
    }

    //Test1: valid jpg upload
    [Fact]
    public async Task UploadImage_ValidJpg_ShouldSucceed()
    {
        var product = new Product(
            "Ipad",
            "ipad/123",
            "Ipad 10 air",
            900,
            8,
            "supplier-id",
            new DateTime(2027, 7, 27),
            "product-id"
        );

        var fileStream = new MemoryStream(new byte[] { 1, 2, 3 });

        var command = new UploadProductImageCommand("product-id", "image.jpg", fileStream.Length, fileStream);

        _productRepositoryMock.Setup(x => x.GetByIdAsync("product-id", CancellationToken.None)).ReturnsAsync(product);

        _storageServiceMock.Setup(x => x.UploadAsync(fileStream, "image.jpg", CancellationToken.None))
            .ReturnsAsync("products/image.jpg");

        _productRepositoryMock.Setup(x => x.AddImageAsync(It.IsAny<ProductImage>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        _publisherMock.Setup(x => x.PublishAsync("file.uploaded", It.IsAny<WarehouseFileUploadedEvent>(),
            CancellationToken.None)).Returns(Task.CompletedTask);


        var result = await _handler.Handle(command, CancellationToken.None);


        Assert.Equal(UploadProductImageResult.Success, result);
        Assert.Equal(1, product.ProductImages.Count);
    }

    //Test2: valid png upload 
    [Fact]
    public async Task UploadImage_ValidPng_ShouldSucceed()
    {
        var product = new Product(
            "Ipad",
            "ipad/123",
            "Ipad 10 air",
            900,
            8,
            "supplier-id",
            new DateTime(2027, 7, 27),
            "product-id"
        );

        var fileStream = new MemoryStream(new byte[] { 1, 2, 3 });

        var command = new UploadProductImageCommand("product-id", "image.png", fileStream.Length, fileStream);

        _productRepositoryMock.Setup(x => x.GetByIdAsync("product-id", CancellationToken.None)).ReturnsAsync(product);

        _storageServiceMock.Setup(x => x.UploadAsync(fileStream, "image.png", CancellationToken.None))
            .ReturnsAsync("products/image.png");

        _productRepositoryMock.Setup(x => x.AddImageAsync(It.IsAny<ProductImage>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        _publisherMock.Setup(x => x.PublishAsync("file.uploaded", It.IsAny<WarehouseFileUploadedEvent>(),
            CancellationToken.None)).Returns(Task.CompletedTask);


        var result = await _handler.Handle(command, CancellationToken.None);


        Assert.Equal(UploadProductImageResult.Success, result);
        Assert.Equal(1, product.ProductImages.Count);
    }
    
    //Test3: invalid extension rejected
    [Fact]
    public async Task UploadImage_InvalidExtension_ShouldFail()
    {
        var product = new Product(
            "Ipad",
            "ipad/123",
            "Ipad 10 air",
            900,
            8,
            "supplier-id",
            new DateTime(2027, 7, 27),
            "product-id"
        );

        var fileStream = new MemoryStream(new byte[] { 1, 2, 3 });

        var command = new UploadProductImageCommand("product-id", "image.xxx", fileStream.Length, fileStream);

        _productRepositoryMock.Setup(x => x.GetByIdAsync("product-id", CancellationToken.None)).ReturnsAsync(product);

        var result = await _handler.Handle(command, CancellationToken.None);


        Assert.Equal(UploadProductImageResult.InvalidExtension, result);
        Assert.Empty(product.ProductImages);
    }
    
    //Test4: file > 2MB rejected
    [Fact]
    public async Task UploadImage_fileSizeGreaterThan2MB_ShouldFail()
    {
        using var fs = File.Create("image.ipg");
        fs.SetLength(3 * 1024 * 1024); //3 MB
        
        var product = new Product(
            "Ipad",
            "ipad/123",
            "Ipad 10 air",
            900,
            8,
            "supplier-id",
            new DateTime(2027, 7, 27),
            "product-id"
        );
        

        var command = new UploadProductImageCommand("product-id", "image.png", fs.Length, fs);

        _productRepositoryMock.Setup(x => x.GetByIdAsync("product-id", CancellationToken.None)).ReturnsAsync(product);


        var result = await _handler.Handle(command, CancellationToken.None);


        Assert.Equal(UploadProductImageResult.FileTooLarge, result);
        Assert.Empty(product.ProductImages);
    }
    
    //Test5: upload path generated correctly: the path is generated by MinIOSTorageService, by the UploadAsync method
    //It should generate:  var objectName = $"{Guid.NewGuid()}_{fileName}"
    [Fact]
    public async Task UploadImage_ShouldGenerateCorrectPath()
    {
        var minioClientMock = new Mock<IMinioClient>();

        var configurationValues =
            new Dictionary<string, string?>
            {
                ["MinIO:BucketName"] = "product-images"
            };

        //Like a fake appsettings.json, to put the MinIO configuration
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationValues).Build();

        var storageService = new MinioStorageService(
            minioClientMock.Object,
            configuration);

        using var fileStream = new MemoryStream(new byte[] { 1, 2, 3 });
        
        var result = await storageService.UploadAsync(
            fileStream,
            "image.jpg",
            CancellationToken.None);
        
        Assert.EndsWith("_image.jpg", result);

        var guidPart = result.Substring(0, result.Length - "_image.jpg".Length);

        Assert.True(Guid.TryParse(guidPart, out _));
    }
    
}