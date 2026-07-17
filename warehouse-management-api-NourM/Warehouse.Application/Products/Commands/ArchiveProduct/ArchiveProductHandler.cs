using AutoMapper;
using MediatR;
using Warehouse.Application.Exceptions;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;
namespace Warehouse.Application.Products.Commands;

public class ArchiveProductHandler
    : IRequestHandler<ArchiveProductCommand, ProductViewModel?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ArchiveProductHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductViewModel?> Handle(
        ArchiveProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(
            command.ProductId,
            cancellationToken);

        if (product is null)
            throw new NotFoundException("ProductNotFound");

        product.Archive();

        await _productRepository.UpdateAsync(product, cancellationToken);

        return _mapper.Map<ProductViewModel>(product);
    }
}