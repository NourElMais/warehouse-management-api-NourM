using MediatR;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Exceptions;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UploadProductImageHandler 
    : IRequestHandler<UploadProductImageCommand, UploadProductImageResult>
{
    private readonly IProductRepository _productRepository;
    private  readonly ILogger<UploadProductImageHandler> _logger;
    public UploadProductImageHandler(IProductRepository productRepository, ILogger<UploadProductImageHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<UploadProductImageResult> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId,cancellationToken);

        if (product is null)
            throw new NotFoundException("ProductNotFound");

        if (string.IsNullOrWhiteSpace(request.FileName))
            return UploadProductImageResult.EmptyImage;

        string extension = Path.GetExtension(request.FileName).ToLower();

        if (extension != ".jpg" && extension != ".png")
            return UploadProductImageResult.InvalidExtension;

        if (request.FileSize > 2 * 1024 * 1024)
            return UploadProductImageResult.FileTooLarge;

        _logger.LogInformation(
            "Image {FileName} uploaded for product {ProductId}",
            request.FileName,
            request.ProductId);
        return UploadProductImageResult.Success;
    }
}