using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Exceptions;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UpdateProductPriceHandler 
    : IRequestHandler<UpdateProductPriceCommand, ProductViewModel?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductPriceHandler> _logger;
    public UpdateProductPriceHandler(IProductRepository productRepository, IMapper mapper, ILogger<UpdateProductPriceHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductViewModel> Handle(UpdateProductPriceCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException("ProductNotFound");

        decimal oldPrice = product.Price;

        product.UpdatePrice(command.NewPrice);

        await _productRepository.UpdateAsync(product, cancellationToken);

        _logger.LogInformation("Product {ProductId} price is updated from {OldPrice} to {NewPrice}: ", command.ProductId, oldPrice, product.Price);

        return _mapper.Map<ProductViewModel>(product);
        
        }
}