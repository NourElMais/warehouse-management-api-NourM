using MediatR;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Exceptions;
using Warehouse.Application.IntegrationEvents;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.ProductImages;
using Warehouse.Domain.Repositories;
using Warehouse.Infrastructure.Storage;

namespace Warehouse.Application.Products.Commands;

public class UploadProductImageHandler 
    : IRequestHandler<UploadProductImageCommand, UploadProductImageResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UploadProductImageHandler> _logger;
    private readonly IStorageService _storageService;
    private readonly IRabbitMqPublisher _publisher;

    public UploadProductImageHandler(IProductRepository productRepository, ILogger<UploadProductImageHandler> logger, IStorageService storageService, IRabbitMqPublisher publisher)
    {
        _productRepository = productRepository;
        _logger = logger;
        _storageService = storageService;
        _publisher = publisher;
    }

    public async Task<UploadProductImageResult> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException("ProductNotFound");

        if (string.IsNullOrWhiteSpace(request.FileName))
            return UploadProductImageResult.EmptyImage;

        string extension = Path.GetExtension(request.FileName).ToLower();

        if (extension != ".jpg" && extension != ".png")
            return UploadProductImageResult.InvalidExtension;

        if (request.FileSize > 2 * 1024 * 1024)
            return UploadProductImageResult.FileTooLarge;
        
        var imagePath = await _storageService.UploadAsync(request.FileStream, request.FileName, cancellationToken);
        var productImage = new ProductImage(request.ProductId, request.FileName, imagePath);
        product.AddImage(productImage);
        await _productRepository.AddImageAsync(productImage, cancellationToken);
        var fileUploadedEvent = new WarehouseFileUploadedEvent
        {
            RelatedEntityId = request.ProductId.ToString(),
            RelatedEntityType = "Product",
            FileName = request.FileName,
        };

        await _publisher.PublishAsync("file.uploaded", fileUploadedEvent, cancellationToken);
        
        _logger.LogInformation(
            "Image {FileName} uploaded for product {ProductId}",
            request.FileName,
            request.ProductId);

        return UploadProductImageResult.Success;
    }
}