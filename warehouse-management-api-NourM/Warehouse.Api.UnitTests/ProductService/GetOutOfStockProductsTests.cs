using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using Warehouse.Application.Products.Queries;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Infrastructure.Storage;
using Warehouse.Presentation.Controllers;
using Warehouse.Presentation.Resources;

namespace Warehouse.Api.UnitTests;

public class GetOutOfStockProductsTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IStringLocalizer<SharedResources>> _stringLocalizerMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly GetOutOfStockProductsHandler _handler;
    private readonly ProductsController _controller;

    public GetOutOfStockProductsTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _mediatorMock = new Mock<IMediator>();
        _stringLocalizerMock = new Mock<IStringLocalizer<SharedResources>>();
        _storageServiceMock = new Mock<IStorageService>();
        _handler = new GetOutOfStockProductsHandler(_productRepositoryMock.Object, _mapperMock.Object);
        _controller = new ProductsController(_mediatorMock.Object, _stringLocalizerMock.Object, _storageServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOnlyNonArchivedProductsWithZeroQuantity()
    {
        var inStockProduct = new Product("Laptop", "lap/123", "Gaming laptop", 1200, 5, "supplier-id1", DateTime.UtcNow.AddYears(1));
        var outOfStockProduct = new Product("Mouse", "mouse/123", "Wireless mouse", 100, 0, "supplier-id2", DateTime.UtcNow.AddYears(1));
        var archivedOutOfStockProduct = new Product("Keyboard", "key/123", "Mechanical keyboard", 150, 0, "supplier-id3", DateTime.UtcNow.AddYears(1));
        archivedOutOfStockProduct.Archive();

        var products = new List<Product>
        {
            inStockProduct,
            outOfStockProduct,
            archivedOutOfStockProduct
        };

        var expectedResult = new List<ProductViewModel>
        {
            new()
            {
                Id = outOfStockProduct.Id,
                Name = outOfStockProduct.Name,
                SKU = outOfStockProduct.SKU,
                Price = outOfStockProduct.Price,
                QuantityInStock = outOfStockProduct.QuantityInStock,
                ExpiryDate = outOfStockProduct.ExpiryDate,
                IsArchived = outOfStockProduct.IsArchived,
                SupplierId = outOfStockProduct.SupplierId
            }
        };

        _productRepositoryMock
            .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(mapper => mapper.Map<List<ProductViewModel>>(It.Is<List<Product>>(mappedProducts => mappedProducts.Count == 1 && mappedProducts[0].Id == outOfStockProduct.Id)))
            .Returns(expectedResult);

        var result = await _handler.Handle(new GetOutOfStockProductsQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetOutOfStockProducts_ShouldReturnOkResult()
    {
        var expectedProducts = new List<ProductViewModel>
        {
            new()
            {
                Id = "product-id",
                Name = "Mouse",
                SKU = "mouse/123",
                Price = 100,
                QuantityInStock = 0,
                ExpiryDate = DateTime.Today.AddDays(10),
                IsArchived = false,
                SupplierId = "supplier-id"
            }
        };

        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<GetOutOfStockProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProducts);

        var actionResult = await _controller.GetOutOfStockProducts(CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedProducts);
    }
}