using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Exceptions;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;
namespace Warehouse.Application.Products.Commands;

public class ArchiveProductHandler
    : IRequestHandler<ArchiveProductCommand, ProductViewModel?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ArchiveProductHandler> _logger;

    public ArchiveProductHandler(IProductRepository productRepository, IMapper mapper, ILogger<ArchiveProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductViewModel?> Handle(ArchiveProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(
            command.ProductId,
            cancellationToken);

        if (product is null)
            throw new NotFoundException("ProductNotFound");

        product.Archive();

        await _productRepository.UpdateAsync(product, cancellationToken);
        _logger.LogInformation( "Product {ProductId} archived successfully", command.ProductId);
        return _mapper.Map<ProductViewModel>(product);
    }
}