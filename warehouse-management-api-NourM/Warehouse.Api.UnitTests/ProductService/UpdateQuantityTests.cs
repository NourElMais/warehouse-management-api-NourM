using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Products.Commands;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Api.UnitTests;

public class UpdateQuantityTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IRabbitMqPublisher> _publisherMock;
    private readonly Mock<ILogger<UpdateProductQuantityHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateProductQuantityHandler _handler;

    public UpdateQuantityTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<UpdateProductQuantityHandler>>();
        _publisherMock =  new Mock<IRabbitMqPublisher>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateProductQuantityHandler(_productRepositoryMock.Object,_mapperMock.Object, _loggerMock.Object, _publisherMock.Object);
    }
    //Test1: valid quantity updates stock 
    [Fact]
    public async Task UpdateQuantity_ValidQuantity_ShouldSucceed()
    {
        var command = new UpdateProductQuantityCommand("product-id", 10);
        var product = new Product(
            "Ipad",
            "ipad/123",
            "Ipad 10 air",
            900,
            8,
            "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
            new DateTime(2026, 7, 27)
        );
        _productRepositoryMock.Setup(x => x.GetByIdAsync("product-id", CancellationToken.None)).ReturnsAsync(product);
        var expectedResult = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            QuantityInStock = 10
        };

        _mapperMock.Setup(x => x.Map<ProductViewModel>(product))
            .Returns(expectedResult);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.QuantityInStock.Should().Be(10); //To check the returned ViewModel
        product.QuantityInStock.Should().Be(10); //To check thatthe actual product entity was updated

    }
    
    //Test2: negative quantity rejected
    //Note: The check is done by the DTO using the Range attribute
    [Fact]
    public void UpdateQuantity_NegativeQuantity_ShouldBeRejected()
    {
        //Assuming the client sends this through swagger
        var request = new UpdateProductQuantityRequest()
        {
            QuantityInStock = -1
        };

        var validationContext = new ValidationContext(request); //ValidationContext tells the validator which object it should validate.
        var validationResults = new List<ValidationResult>(); //List to store errors

        var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
        isValid.Should().BeFalse(); 

    }
    
    //Test3: last updated changes 
    [Fact]
    public async Task UpdateQuantity_ValidQuantity_LastUpdatedShouldChange()
    {
        var command = new UpdateProductQuantityCommand("product-id", 10);
        var product = new Product(
            "Ipad",
            "ipad/123",
            "Ipad 10 air",
            900,
            8,
            "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
            new DateTime(2026, 7, 27)
        );
        var lastUpd1 = product.LastUpdatedAt; 
        _productRepositoryMock.Setup(x => x.GetByIdAsync("product-id", CancellationToken.None)).ReturnsAsync(product);
        var expectedResult = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            QuantityInStock = 10
        };

        _mapperMock.Setup(x => x.Map<ProductViewModel>(product))
            .Returns(expectedResult);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.NotEqual(lastUpd1, product.LastUpdatedAt);
        

    }
    
    

    

}