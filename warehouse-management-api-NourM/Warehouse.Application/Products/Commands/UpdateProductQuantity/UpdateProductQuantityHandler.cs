using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Exceptions;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UpdateProductQuantityHandler 
    : IRequestHandler<UpdateProductQuantityCommand, ProductViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductQuantityHandler> _logger;

    public UpdateProductQuantityHandler(IProductRepository productRepository, IMapper mapper, ILogger<UpdateProductQuantityHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductViewModel> Handle(UpdateProductQuantityCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId,cancellationToken);

        if (product is null)
            throw new NotFoundException("ProductNotFound");
        
        int oldQuantity = product.QuantityInStock;
        product.UpdateQuantity(command.NewQuantity);
        await _productRepository.UpdateAsync(product, cancellationToken);

        _logger.LogInformation(
            "Product {ProductId} quantity updated from {OldQuantity} to {NewQuantity}", command.ProductId, oldQuantity, product.QuantityInStock);
        return _mapper.Map<ProductViewModel>(product);
    }
}