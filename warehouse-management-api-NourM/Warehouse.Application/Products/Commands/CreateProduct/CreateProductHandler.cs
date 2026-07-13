using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public CreateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductViewModel> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = new Product(
            command.Name,
            command.SKU,
            command.Description,
            command.Price,
            command.QuantityInStock,
            command.SupplierId,
            command.ExpiryDate
        );

        await _productRepository.AddAsync(product, cancellationToken);
       return _mapper.Map<ProductViewModel>(product);
    }
}