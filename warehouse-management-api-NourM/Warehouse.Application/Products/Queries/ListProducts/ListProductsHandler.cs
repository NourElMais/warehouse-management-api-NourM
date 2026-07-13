using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class ListProductsHandler 
    : IRequestHandler<ListProductsQuery, List<ProductViewModel>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ListProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductViewModel>> Handle(
        ListProductsQuery query,
        CancellationToken cancellationToken)
    {
        List<Product> products = await _productRepository.GetAllAsync(cancellationToken);

        if (!query.OnlyAvailable)
        {
            return _mapper.Map<List<ProductViewModel>>(products);
        }

        List<Product> availableProducts = new List<Product>();

        foreach (Product product in products)
        {
            if (!product.IsArchived)
            {
                availableProducts.Add(product);
            }
        }

       return _mapper.Map<List<ProductViewModel>>(availableProducts);
    }
}