using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class GetLowStockProductsHandler
    : IRequestHandler<GetLowStockProductsQuery, List<ProductViewModel>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public GetLowStockProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public Task<List<ProductViewModel>> Handle(
        GetLowStockProductsQuery query,
        CancellationToken cancellationToken)
    {
        List<Product> products = _productRepository.GetAll();
        List<Product> lowStockProducts = new List<Product>();

        foreach (Product product in products)
        {
            if (!product.IsArchived &&
                product.QuantityInStock <= query.Threshold)
            {
                lowStockProducts.Add(product);
            }
        }
 
        var viewModel = _mapper.Map<List<ProductViewModel>>(lowStockProducts);

        return Task.FromResult(viewModel);
    }
}