using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Warehouse.Application.IntegrationEvents;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Products.Commands;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Api.UnitTests;


public class CreateProductTests
{
   private readonly Mock<IProductRepository> _productRepositoryMock;
   private readonly Mock<IMapper> _mapperMock;
   private readonly Mock<ILogger<CreateProductHandler>> _loggerMock;
   private readonly Mock<IRabbitMqPublisher> _publisherMock;

   private readonly CreateProductHandler _handler;

   //Note: .Object: Gives the fake repository inside the mock
   //We create the Mock objects inside the constructor
   public CreateProductTests()
   {
      _productRepositoryMock = new Mock<IProductRepository>();
      _mapperMock = new Mock<IMapper>();
      _loggerMock = new Mock<ILogger<CreateProductHandler>>();
      _publisherMock = new Mock<IRabbitMqPublisher>();
      _handler = new CreateProductHandler(_productRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object,
         _publisherMock.Object
      );
   }

   //Test1: create valid product succeeds
   [Fact]
   public async Task CreateProduct_ValidProduct_Succeeds()
   {
      var product = new CreateProductCommand
      {
         Name = "Ipad",
         SKU = "ipad/123",
         Description = "Ipad 10 air",
         Price = 900,
         QuantityInStock = 8,
         SupplierId = "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
         ExpiryDate = new DateTime(2026, 7, 27)
      };

      // .SetUp: Define what the mock should do when the method is called.
      // x represents the the object that will be returned by _productRepositoryMock.Object.
      _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _publisherMock.Setup(x =>
            x.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      var expectedResult = new ProductViewModel
      {
         Id = "product-id",
         Name = product.Name,
         SKU = product.SKU,
         Price = product.Price,
         QuantityInStock = product.QuantityInStock,
         ExpiryDate = product.ExpiryDate,
         IsArchived = false
      };

      //Whenever the fake mapper receives any Product and is asked to convert it into a ProductViewModel, return expectedResult
      _mapperMock.Setup(x => x.Map<ProductViewModel>(It.IsAny<Product>())).Returns(expectedResult);
      var result = await _handler.Handle(product, CancellationToken.None);
      result.Should().BeEquivalentTo(expectedResult);
   }


   //Test2: duplicate SKU throws exception 
   [Fact]
   public async Task CreateProduct_duplicateSKU_ShouldFail()
   {
      var product = new CreateProductCommand
      {
         Name = "Ipad",
         SKU = "ipad/123",
         Description = "Ipad 10 air",
         Price = 900,
         QuantityInStock = 8,
         SupplierId = "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
         ExpiryDate = new DateTime(2026, 7, 27)
      };

      // .SetUp: Define what the mock should do when the method is called.
      // x represents the the object that will be returned by _productRepositoryMock.Object.

      var existingProduct = new Product("Ipad", "ipad/123", "Ipad 11", 400, 10, "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
         new DateTime(2026, 7, 27));
      _productRepositoryMock.Setup(x => x.GetBySkuAsync(product.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync(existingProduct);
      _publisherMock.Setup(x =>
            x.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      var expectedResult = new ProductViewModel
      {
         Id = "product-id",
         Name = product.Name,
         SKU = product.SKU,
         Price = product.Price,
         QuantityInStock = product.QuantityInStock,
         ExpiryDate = product.ExpiryDate,
         IsArchived = false
      };

      await Assert.ThrowsAsync<BusinessRuleException>(async () =>
         await _handler.Handle(product, CancellationToken.None));
   }

   //Test3: created date assigned 
   [Fact]
   public void CreateProduct_AssignCreatedDate_ShouldNotBeEqualToDefaultDateTimeValue()
   {
      var product = new Product(
         "Ipad",
         "ipad/123",
         "Ipad 10 air",
         900,
         8,
         "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
         new DateTime(2026, 7, 27)
      );

      Assert.NotEqual(default(DateTime), product.CreatedAt);
   }


   //Test4: generated id not empty 
   [Fact]
   public void CreateProduct_ValidProduct_IdShouldNotBeEmpty()
   {
      var product = new Product(
         "Ipad",
         "ipad/123",
         "Ipad 10 air",
         900,
         8,
         "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
         new DateTime(2026, 7, 27)
      );

      Assert.NotEmpty(product.Id);
   }

}
