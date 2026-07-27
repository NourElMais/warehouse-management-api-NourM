using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Warehouse.Application.Products.Commands;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Api.UnitTests.SupplierService;

public class AssignSupplierToProductTest
{
    private readonly Mock<IProductRepository> _productRepository;
    private readonly Mock<ISupplierRepository> _supplierRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<ILogger<AssignSupplierToProductHandler>> _logger;
    private readonly AssignSupplierToProductHandler _handler;

    public AssignSupplierToProductTest()
    {
        _productRepository = new Mock<IProductRepository>();
        _supplierRepository = new Mock<ISupplierRepository>();
        _mapper = new Mock<IMapper>();
        _logger = new Mock<ILogger<AssignSupplierToProductHandler>>();
        _handler = new AssignSupplierToProductHandler(_productRepository.Object, _supplierRepository.Object,_mapper.Object, _logger.Object);
    }

    [Fact]
    public async Task AssignSupplierToProduct_ShouldSucceed()
    {
        var command = new AssignSupplierToProductCommand("product-id", "supplier-id");
        var product = new Product("Ipad", "ipad/123", "Ipad 10 air", 900, 8, "1e2120cd-8508-469c-8ea2-bbf35bc7a059", new DateTime(2026, 7, 27));
        var supplier = new Supplier("Nour", "Lebanon", "nour@mail.com", "03-421605", "e641e362-a1a1-44b0-bb25-f2d7cb296d31");
        _productRepository.Setup(x => x.GetByIdAsync("product-id", CancellationToken.None)).ReturnsAsync(product);
        _supplierRepository.Setup(x => x.GetByIdAsync("supplier-id", CancellationToken.None)).ReturnsAsync(supplier);
        var expectedResult = new ProductViewModel()
        {
            Id = product.Id,
            SupplierId = supplier.Id,
        };
        
        _mapper.Setup(x => x.Map<ProductViewModel>(product)).Returns(expectedResult);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(result.SupplierId, expectedResult.SupplierId);
        product.SupplierId.Should().Be(supplier.Id);
    }
}