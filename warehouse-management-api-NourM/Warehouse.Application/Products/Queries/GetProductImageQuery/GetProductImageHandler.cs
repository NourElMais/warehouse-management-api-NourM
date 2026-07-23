using MediatR;
using Warehouse.Domain.ProductImages;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries.GetProductImageQuery;

public class GetProductImageHandler : IRequestHandler<GetProductImageQuery, ProductImage?>
{
    private readonly IProductRepository _productRepository;

    public GetProductImageHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductImage?> Handle(
        GetProductImageQuery request,
        CancellationToken cancellationToken)
    {
        return await _productRepository.GetImageAsync(
            request.ProductId,
            cancellationToken);
    }
}