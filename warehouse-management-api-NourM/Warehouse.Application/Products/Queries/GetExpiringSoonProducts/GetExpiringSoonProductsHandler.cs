using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class GetExpiringSoonProductsHandler : IRequestHandler<GetExpiringSoonProductsQuery, List<ProductViewModel>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetExpiringSoonProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductViewModel>> Handle(GetExpiringSoonProductsQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await _productRepository.GetExpiringSoonAsync(request.DaysAhead, cancellationToken);

        return _mapper.Map<List<ProductViewModel>>(products);
    }
}