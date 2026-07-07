using MediatR;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UploadProductImageHandler : IRequestHandler<UploadProductImageCommand, string>
{
    private readonly IProductRepository _productRepository;

    public UploadProductImageHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<string> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(request.ProductId);

        if (product is null)
            return Task.FromResult("not found");

        if (string.IsNullOrWhiteSpace(request.FileName))
            return Task.FromResult("empty image");

        string extension = Path.GetExtension(request.FileName).ToLower();

        if (extension != ".jpg" && extension != ".png")
            return Task.FromResult("invalid extension");

        if (request.FileSize > 2 * 1024 * 1024)
            return Task.FromResult("too large");

        return Task.FromResult("success");
    }
}