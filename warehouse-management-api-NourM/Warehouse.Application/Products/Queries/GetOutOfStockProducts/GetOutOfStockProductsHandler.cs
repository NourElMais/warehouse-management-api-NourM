using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class GetOutOfStockProductsHandler
    : IRequestHandler<GetOutOfStockProductsQuery, List<ProductViewModel>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetOutOfStockProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductViewModel>> Handle(GetOutOfStockProductsQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await _productRepository.GetAllAsync(cancellationToken);
        List<Product> outOfStockProducts = new List<Product>();

        foreach (Product product in products)
        {
            if (!product.IsArchived && product.QuantityInStock == 0)
            {
                outOfStockProducts.Add(product);
            }
        }

        return _mapper.Map<List<ProductViewModel>>(outOfStockProducts);
    }
}