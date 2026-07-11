using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class SearchProductsHandler
    : IRequestHandler<SearchProductsQuery, List<ProductViewModel>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public SearchProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public Task<List<ProductViewModel>> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        List<Product> products = _productRepository.Search(request.Name, request.Supplier);

        var viewModel = _mapper.Map<List<ProductViewModel>>(products);

        return Task.FromResult(viewModel);
    }
}