using AutoMapper;
using MediatR;
using Warehouse.Application.Exceptions;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UpdateProductPriceHandler 
    : IRequestHandler<UpdateProductPriceCommand, ProductViewModel?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public UpdateProductPriceHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductViewModel> Handle(
        UpdateProductPriceCommand command,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException("The product was not found");

        decimal oldPrice = product.Price;

        product.UpdatePrice(command.NewPrice);

        await _productRepository.UpdateAsync(product, cancellationToken);

        Console.WriteLine($"Product {product.Name} price changed from {oldPrice} to {product.Price}");

        return _mapper.Map<ProductViewModel>(product);
        
        }
}