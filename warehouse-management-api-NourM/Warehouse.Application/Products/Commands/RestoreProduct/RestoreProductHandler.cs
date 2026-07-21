using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Exceptions;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class RestoreProductHandler 
    : IRequestHandler<RestoreProductCommand, ProductViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RestoreProductHandler> _logger;
    public RestoreProductHandler(IProductRepository productRepository, IMapper mapper, ILogger<RestoreProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        
    }

    public async Task<ProductViewModel> Handle(RestoreProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException("ProductNotFound");

        product.Restore();
        await _productRepository.UpdateAsync(product,cancellationToken);
        _logger.LogInformation("Restored Product {ProductId} successfully", command.ProductId);

        return _mapper.Map<ProductViewModel>(product);
    }
}