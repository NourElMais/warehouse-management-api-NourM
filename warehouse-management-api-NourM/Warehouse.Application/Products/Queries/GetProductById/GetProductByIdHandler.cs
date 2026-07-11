using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class GetProductByIdHandler 
    : IRequestHandler<GetProductByIdQuery, ProductViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public Task<ProductViewModel> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        Product? product = _productRepository.GetById(request.Id);

        var viewModel = _mapper.Map<ProductViewModel>(product);

        return Task.FromResult(viewModel);
    }
}