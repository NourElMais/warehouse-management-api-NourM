using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class RestoreProductHandler 
    : IRequestHandler<RestoreProductCommand, ProductViewModel?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public RestoreProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductViewModel?> Handle(
        RestoreProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            return null;

        product.Restore();
        _productRepository.UpdateAsync(product,cancellationToken);

        return _mapper.Map<ProductViewModel>(product);
    }
}