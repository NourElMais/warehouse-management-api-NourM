using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UpdateProductPriceHandler 
    : IRequestHandler<UpdateProductPriceCommand, ProductViewModel?>
{
    private readonly IProductRepository _productRepository;
    private IMapper _mapper;
    public UpdateProductPriceHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public Task<ProductViewModel> Handle(
        UpdateProductPriceCommand command,
        CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return Task.FromResult<ProductViewModel?>(null);

        decimal oldPrice = product.Price;

        product.UpdatePrice(command.NewPrice);

        _productRepository.Update(product);

        Console.WriteLine(
            $"Product {product.Name} price changed from {oldPrice} to {product.Price}");

        var viewModel = _mapper.Map<ProductViewModel>(product);

        return Task.FromResult<ProductViewModel?>(viewModel);
        }
}