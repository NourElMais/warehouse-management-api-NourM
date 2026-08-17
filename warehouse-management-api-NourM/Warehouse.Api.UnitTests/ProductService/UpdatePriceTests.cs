using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Warehouse.Application.Products.Commands;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Api.UnitTests;

public class UpdatePriceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UpdateProductPriceHandler>> _loggerMock;
    private readonly  UpdateProductPriceHandler _handler;

    public UpdatePriceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UpdateProductPriceHandler>>();
        _handler = new UpdateProductPriceHandler(_productRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        
    }
    
    //Test1: valid price updates 
    [Fact]
    public async Task UpdateProductPrice_ValidPrice_ShouldSucceed()
    {
        var command = new UpdateProductPriceCommand("product-id", 800);
        var product = new Product("Ipad",
            "ipad/123",
            "Ipad 10 air",
            900,
            8,
            "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
            new DateTime(2026, 7, 27)
        );

        var expectedResult = new ProductViewModel()
        {
            Id = product.Id,
            Name = product.Name,
            Price = 800
        };
        _productRepositoryMock.Setup(x => x.GetByIdAsync(command.ProductId, CancellationToken.None))
            .ReturnsAsync(product);
        _mapperMock.Setup(x => x.Map<ProductViewModel>(product)).Returns(expectedResult);
        var  result = await _handler.Handle(command, CancellationToken.None);
        result.Price.Should().Be(800);
        product.Price.Should().Be(800);
    }
    
    //Test2: invalid price rejected
    [Fact]
    public void UpdateProductPrice_NegativePrice_ShouldFail()
    {
        //Assuming the client sends this through swagger
        var request = new UpdateProductPriceRequest
        {
            Price = -5
        };

        var validationContext = new ValidationContext(request); //ValidationContext tells the validator which object it should validate.
        var validationResults = new List<ValidationResult>(); //List to store errors

        var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
        isValid.Should().BeFalse(); 
       
    }
    
}