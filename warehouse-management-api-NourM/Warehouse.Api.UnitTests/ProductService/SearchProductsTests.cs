using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using Warehouse.Application.Products.Queries;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;
using Warehouse.Infrastructure.Storage;
using Warehouse.Presentation.Controllers;
using Warehouse.Presentation.Resources;

namespace Warehouse.Api.UnitTests;

public class SearchProductsTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly SearchProductsHandler _handler;
    private readonly Mock<IStringLocalizer<SharedResources>>_stringLocalizerMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly ProductsController _controller;

    public SearchProductsTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _mediatorMock = new Mock<IMediator>();
        _stringLocalizerMock = new Mock<IStringLocalizer<SharedResources>>();
        _storageServiceMock = new Mock<IStorageService>();
        _controller = new ProductsController(_mediatorMock.Object,_stringLocalizerMock.Object,_storageServiceMock.Object);
        _handler = new SearchProductsHandler(_productRepositoryMock.Object, _mapperMock.Object);
    }

    //Test1: search by name returns matches
    [Fact]
    public async Task SearchByName_ValidName_ShouldReturnAMatchingProduct()
    {
        var search = new SearchProductsQuery("Ipad", null);
        var products = new List<Product>
        {
            new Product(
                "Ipad",
                "ipad/123",
                "Ipad 10 air",
                900,
                8,
                "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
                new DateTime(2026, 7, 27)
            )
        };

        _productRepositoryMock.Setup(x => x.SearchAsync("Ipad", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var expectedResult = new List<ProductViewModel>
        {
            new ProductViewModel
            {
                Id = products[0].Id,
                Name = products[0].Name,
                SKU = products[0].SKU,
                Price = products[0].Price,
                QuantityInStock = products[0].QuantityInStock,
                ExpiryDate = products[0].ExpiryDate,
                IsArchived = false
            }
        };

        _mapperMock.Setup(x => x.Map<List<ProductViewModel>>(It.IsAny<List<Product>>())).Returns(expectedResult);
        var result = await _handler.Handle(search, CancellationToken.None);
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    //Test 2: search by supplier returns matches
    [Fact]
    public async Task SearchBySupplier_ValidSupplier_ShouldReturnAMatchingProduct()
    {
        var search = new SearchProductsQuery(null, "Nour");
        var products = new List<Product>
        {
            new Product(
                "Ipad",
                "ipad/123",
                "Ipad 10 air",
                900,
                8,
                "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
                new DateTime(2026, 7, 27)
            )
        };

        _productRepositoryMock.Setup(x => x.SearchAsync(null,"Nour",  It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var expectedResult = new List<ProductViewModel>
        {
            new ProductViewModel
            {
                Id = products[0].Id,
                Name = products[0].Name,
                SKU = products[0].SKU,
                Price = products[0].Price,
                QuantityInStock = products[0].QuantityInStock,
                ExpiryDate = products[0].ExpiryDate,
                IsArchived = false
            }
        };

        _mapperMock.Setup(x => x.Map<List<ProductViewModel>>(It.IsAny<List<Product>>())).Returns(expectedResult);
        var result = await _handler.Handle(search, CancellationToken.None);
        result.Should().BeEquivalentTo(expectedResult);
    }

    //Test3: search by both filters returns intersection 
    [Fact]
    public async Task SearchByBothFilters_ValidSupplierAndProductNames_ShouldReturnAMatchingProduct()
    {
        var search = new SearchProductsQuery("Ipad", "Nour");
        var products = new List<Product>
        {
            new Product(
                "Ipad",
                "ipad/123",
                "Ipad 10 air",
                900,
                8,
                "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
                new DateTime(2026, 7, 27)
            )
        };

        _productRepositoryMock.Setup(x => x.SearchAsync("Ipad","Nour",  It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var expectedResult = new List<ProductViewModel>
        {
            new ProductViewModel
            {
                Id = products[0].Id,
                Name = products[0].Name,
                SKU = products[0].SKU,
                Price = products[0].Price,
                QuantityInStock = products[0].QuantityInStock,
                ExpiryDate = products[0].ExpiryDate,
                IsArchived = false
            }
        };

        _mapperMock.Setup(x => x.Map<List<ProductViewModel>>(It.IsAny<List<Product>>())).Returns(expectedResult);
        var result = await _handler.Handle(search, CancellationToken.None);
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    //Test4: empty filters returns bad request exception
    //Note: the checking for empty filter is done in the controller, not the handler
    [Fact]
    public async Task EmptyFilters_ShouldReturnBadRequest()
    {
        var badRequestResult = await _controller.GetProductsBySearch(null,null,CancellationToken.None);
        Assert.IsType<BadRequestObjectResult>(badRequestResult);
    }
}

