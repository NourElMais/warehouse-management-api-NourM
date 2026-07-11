using AutoMapper;
using MediatR;
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

    public Task<ProductViewModel?> Handle(
        ArchiveProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return Task.FromResult<ProductViewModel?>(null);

        product.Archive();

        _productRepository.Update(product);

        var viewModel = _mapper.Map<ProductViewModel>(product);

        return Task.FromResult<ProductViewModel?>(viewModel);
    }
}