using MediatR;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UploadProductImageHandler 
    : IRequestHandler<UploadProductImageCommand, UploadProductImageResult>
{
    private readonly IProductRepository _productRepository;

    public UploadProductImageHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UploadProductImageResult> Handle(
        UploadProductImageCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId,cancellationToken);

        if (product is null)
            return UploadProductImageResult.ProductNotFound;

        if (string.IsNullOrWhiteSpace(request.FileName))
            return UploadProductImageResult.EmptyImage;

        string extension = Path.GetExtension(request.FileName).ToLower();

        if (extension != ".jpg" && extension != ".png")
            return UploadProductImageResult.InvalidExtension;

        if (request.FileSize > 2 * 1024 * 1024)
            return UploadProductImageResult.FileTooLarge;

        return UploadProductImageResult.Success;
    }
}