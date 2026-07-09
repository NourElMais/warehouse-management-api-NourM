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

    public Task<UploadProductImageResult> Handle(
        UploadProductImageCommand request,
        CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(request.ProductId);

        if (product is null)
            return Task.FromResult(UploadProductImageResult.ProductNotFound);

        if (string.IsNullOrWhiteSpace(request.FileName))
            return Task.FromResult(UploadProductImageResult.EmptyImage);

        string extension = Path.GetExtension(request.FileName).ToLower();

        if (extension != ".jpg" && extension != ".png")
            return Task.FromResult(UploadProductImageResult.InvalidExtension);
        if (request.FileSize > 2 * 1024 * 1024)
            return Task.FromResult(UploadProductImageResult.FileTooLarge);
        return Task.FromResult(UploadProductImageResult.Success);
    }
}