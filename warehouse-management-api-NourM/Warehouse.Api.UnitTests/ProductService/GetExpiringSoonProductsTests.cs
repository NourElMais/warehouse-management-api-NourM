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
using Warehouse.Presentation.Contracts;
using Warehouse.Presentation.Resources;

namespace Warehouse.Api.UnitTests;

public class GetExpiringSoonProductsTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IStringLocalizer<SharedResources>> _stringLocalizerMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly GetExpiringSoonProductsHandler _handler;
    private readonly ProductsController _controller;

    public GetExpiringSoonProductsTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _mediatorMock = new Mock<IMediator>();
        _stringLocalizerMock = new Mock<IStringLocalizer<SharedResources>>();
        _storageServiceMock = new Mock<IStorageService>();
        _handler = new GetExpiringSoonProductsHandler(_productRepositoryMock.Object, _mapperMock.Object);
        _controller = new ProductsController(_mediatorMock.Object, _stringLocalizerMock.Object, _storageServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnMappedProducts()
    {
        var query = new GetExpiringSoonProductsQuery(30);
        var products = new List<Product>
        {
            new(
                "Milk",
                "milk/123",
                "Fresh milk",
                3,
                12,
                "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
                DateTime.Today.AddDays(10))
        };

        var expectedResult = new List<ProductViewModel>
        {
            new()
            {
                Id = products[0].Id,
                Name = products[0].Name,
                SKU = products[0].SKU,
                Price = products[0].Price,
                QuantityInStock = products[0].QuantityInStock,
                ExpiryDate = products[0].ExpiryDate,
                IsArchived = products[0].IsArchived
            }
        };

        _productRepositoryMock
            .Setup(repository => repository.GetExpiringSoonAsync(30, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        _mapperMock
            .Setup(mapper => mapper.Map<List<ProductViewModel>>(products))
            .Returns(expectedResult);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetExpiringSoonProducts_ValidRequest_ShouldReturnOkResult()
    {
        var request = new GetExpiringSoonProductsRequest();
        var expectedProducts = new List<ProductViewModel>
        {
            new()
            {
                Id = "product-id",
                Name = "Milk",
                SKU = "milk/123",
                Price = 3,
                QuantityInStock = 12,
                ExpiryDate = DateTime.Today.AddDays(10),
                IsArchived = false
            }
        };

        _mediatorMock
            .Setup(mediator => mediator.Send(It.Is<GetExpiringSoonProductsQuery>(query => query.DaysAhead == 30), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProducts);

        var actionResult = await _controller.GetExpiringSoonProducts(request, CancellationToken.None);

        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public async Task GetExpiringSoonProducts_InvalidRequestModel_ShouldReturnBadRequest()
    {
        _controller.ModelState.AddModelError(nameof(GetExpiringSoonProductsRequest.DaysAhead), "DaysAhead must be greater than zero.");

        var actionResult = await _controller.GetExpiringSoonProducts(new GetExpiringSoonProductsRequest { DaysAhead = 0 }, CancellationToken.None);

        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }
}