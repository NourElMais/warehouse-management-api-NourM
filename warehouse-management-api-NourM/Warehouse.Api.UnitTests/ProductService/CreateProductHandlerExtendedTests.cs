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

public class CreateProductHandlerExtendedTests
{
   private readonly Mock<IProductRepository> _productRepositoryMock;
   private readonly Mock<IMapper> _mapperMock;
   private readonly Mock<ILogger<CreateProductHandler>> _loggerMock;
   private readonly Mock<IRabbitMqPublisher> _publisherMock;
   private readonly CreateProductHandler _handler;

   public CreateProductHandlerExtendedTests()
   {
      _productRepositoryMock = new Mock<IProductRepository>();
      _mapperMock = new Mock<IMapper>();
      _loggerMock = new Mock<ILogger<CreateProductHandler>>();
      _publisherMock = new Mock<IRabbitMqPublisher>();

      _handler = new CreateProductHandler(
         _productRepositoryMock.Object,
         _mapperMock.Object,
         _loggerMock.Object,
         _publisherMock.Object);
   }

   [Fact]
   public async Task CreateProduct_ValidProduct_ShouldPersistProduct()
   {
      var command = BuildValidCommand();
      Product? savedProduct = null;

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync((Product?)null);

      _productRepositoryMock
         .Setup(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
         .Callback<Product, CancellationToken>((product, _) => savedProduct = product)
         .Returns(Task.CompletedTask);

      _publisherMock
         .Setup(publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _mapperMock
         .Setup(mapper => mapper.Map<ProductViewModel>(It.IsAny<Product>()))
         .Returns(new ProductViewModel());

      await _handler.Handle(command, CancellationToken.None);

      savedProduct.Should().NotBeNull();
      savedProduct!.Name.Should().Be(command.Name);
      savedProduct.SKU.Should().Be(command.SKU);
      savedProduct.Description.Should().Be(command.Description);
      savedProduct.Price.Should().Be(command.Price);
      savedProduct.QuantityInStock.Should().Be(command.QuantityInStock);
      savedProduct.SupplierId.Should().Be(command.SupplierId);
      savedProduct.ExpiryDate.Should().Be(command.ExpiryDate);
      savedProduct.IsArchived.Should().BeFalse();
   }

   [Fact]
   public async Task CreateProduct_ValidProduct_ShouldPublishProductCreatedEvent()
   {
      var command = BuildValidCommand();

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync((Product?)null);

      _productRepositoryMock
         .Setup(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _publisherMock
         .Setup(publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _mapperMock
         .Setup(mapper => mapper.Map<ProductViewModel>(It.IsAny<Product>()))
         .Returns(new ProductViewModel());

      await _handler.Handle(command, CancellationToken.None);

      _publisherMock.Verify(
         publisher => publisher.PublishAsync(
            "product.created",
            It.Is<ProductCreatedEvent>(createdEvent =>
               createdEvent.ProductName == command.Name &&
               createdEvent.SKU == command.SKU &&
               createdEvent.ProductId != Guid.Empty),
            It.IsAny<CancellationToken>()),
         Times.Once);
   }

   [Fact]
   public async Task CreateProduct_ValidProduct_ShouldPassCancellationTokenToDependencies()
   {
      var command = BuildValidCommand();
      using var cancellationTokenSource = new CancellationTokenSource();
      CancellationToken cancellationToken = cancellationTokenSource.Token;

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, cancellationToken))
         .ReturnsAsync((Product?)null);

      _productRepositoryMock
         .Setup(repository => repository.AddAsync(It.IsAny<Product>(), cancellationToken))
         .Returns(Task.CompletedTask);

      _publisherMock
         .Setup(publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), cancellationToken))
         .Returns(Task.CompletedTask);

      _mapperMock
         .Setup(mapper => mapper.Map<ProductViewModel>(It.IsAny<Product>()))
         .Returns(new ProductViewModel());

      await _handler.Handle(command, cancellationToken);

      _productRepositoryMock.Verify(repository => repository.GetBySkuAsync(command.SKU, cancellationToken), Times.Once);
      _productRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Product>(), cancellationToken), Times.Once);
      _publisherMock.Verify(
         publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), cancellationToken),
         Times.Once);
   }

   [Fact]
   public async Task CreateProduct_DuplicateSku_ShouldFail()
   {
      var command = BuildValidCommand();
      var existingProduct = new Product(
         "Ipad",
         command.SKU,
         "Ipad 11",
         400,
         10,
         command.SupplierId,
         command.ExpiryDate);

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync(existingProduct);

      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      await action.Should()
         .ThrowAsync<BusinessRuleException>()
         .WithMessage("Cannot create product, as this SKU already exists");

      _productRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
      _publisherMock.Verify(
         publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()),
         Times.Never);
   }

   [Fact]
   public async Task CreateProduct_AddAsyncThrows_ShouldPropagateException_AndNotPublishEvent()
   {
      var command = BuildValidCommand();

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync((Product?)null);

      _productRepositoryMock
         .Setup(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
         .ThrowsAsync(new InvalidOperationException("Database failure"));

      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database failure");

      _publisherMock.Verify(
         publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()),
         Times.Never);
   }

   [Fact]
   public async Task CreateProduct_PublishAsyncThrows_ShouldPropagateException_AfterPersistingProduct()
   {
      var command = BuildValidCommand();

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync((Product?)null);

      _productRepositoryMock
         .Setup(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _publisherMock
         .Setup(publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
         .ThrowsAsync(new InvalidOperationException("Broker unavailable"));

      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("Broker unavailable");

      _productRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
   }

   [Fact]
   public async Task CreateProduct_MapperThrows_ShouldPropagateException_AfterSuccessfulPersistenceAndPublishing()
   {
      var command = BuildValidCommand();

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync((Product?)null);

      _productRepositoryMock
         .Setup(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _publisherMock
         .Setup(publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _mapperMock
         .Setup(mapper => mapper.Map<ProductViewModel>(It.IsAny<Product>()))
         .Throws(new InvalidOperationException("Mapping failure"));

      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("Mapping failure");
   }

   [Theory]
   [InlineData(null)]
   [InlineData("")]
   [InlineData("   ")]
   public async Task CreateProduct_InvalidName_ShouldThrowArgumentException(string? invalidName)
   {
      var command = BuildValidCommand();
      command.Name = invalidName!;

      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      await action.Should().ThrowAsync<ArgumentException>().WithMessage("Product name is required.");
   }

   [Theory]
   [InlineData(null)]
   [InlineData("")]
   [InlineData("   ")]
   public async Task CreateProduct_InvalidSkuFormat_ShouldThrowArgumentException(string? invalidSku)
   {
      var command = BuildValidCommand();
      command.SKU = invalidSku!;

      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      await action.Should().ThrowAsync<ArgumentException>().WithMessage("SKU is required.");
   }

   [Fact]
   public async Task CreateProduct_ZeroPrice_ShouldThrowArgumentException()
   {
      var command = BuildValidCommand();
      command.Price = 0;

      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      await action.Should().ThrowAsync<ArgumentException>().WithMessage("Price must be greater than zero.");
   }

   [Fact]
   public async Task CreateProduct_NegativeQuantity_ShouldThrowArgumentException()
   {
      var command = BuildValidCommand();
      command.QuantityInStock = -1;

      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      await action.Should().ThrowAsync<ArgumentException>().WithMessage("Quantity cannot be negative.");
   }

   [Theory]
   [InlineData(null)]
   [InlineData("")]
   [InlineData("   ")]
   public async Task CreateProduct_InvalidSupplierId_ShouldThrowArgumentException(string? invalidSupplierId)
   {
      var command = BuildValidCommand();
      command.SupplierId = invalidSupplierId!;

      Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

      await action.Should().ThrowAsync<ArgumentException>().WithMessage("Supplier Id is required.");
   }

   [Fact]
   public async Task CreateProduct_MaximumIntegerQuantity_ShouldSucceed()
   {
      var command = BuildValidCommand();
      command.QuantityInStock = int.MaxValue;

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync((Product?)null);

      _productRepositoryMock
         .Setup(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _publisherMock
         .Setup(publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _mapperMock
         .Setup(mapper => mapper.Map<ProductViewModel>(It.IsAny<Product>()))
         .Returns(new ProductViewModel { QuantityInStock = int.MaxValue });

      var result = await _handler.Handle(command, CancellationToken.None);

      result.QuantityInStock.Should().Be(int.MaxValue);
   }

   [Fact]
   public async Task CreateProduct_MinimumFutureDateBoundary_ShouldPreserveExactExpiryDate()
   {
      DateTime boundaryExpiryDate = DateTime.UtcNow.AddTicks(1);
      var command = BuildValidCommand();
      command.ExpiryDate = boundaryExpiryDate;
      Product? savedProduct = null;

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync((Product?)null);

      _productRepositoryMock
         .Setup(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
         .Callback<Product, CancellationToken>((product, _) => savedProduct = product)
         .Returns(Task.CompletedTask);

      _publisherMock
         .Setup(publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _mapperMock
         .Setup(mapper => mapper.Map<ProductViewModel>(It.IsAny<Product>()))
         .Returns(new ProductViewModel { ExpiryDate = boundaryExpiryDate });

      var result = await _handler.Handle(command, CancellationToken.None);

      savedProduct.Should().NotBeNull();
      savedProduct!.ExpiryDate.Should().Be(boundaryExpiryDate);
      result.ExpiryDate.Should().Be(boundaryExpiryDate);
   }

   [Fact]
   public async Task CreateProduct_MaximumStringLengthsWithinHandler_ShouldSucceed()
   {
      var command = BuildValidCommand();
      command.Name = new string('N', 50);
      command.SKU = new string('S', 200);
      command.Description = new string('D', 500);
      command.SupplierId = new string('X', 500);

      _productRepositoryMock
         .Setup(repository => repository.GetBySkuAsync(command.SKU, It.IsAny<CancellationToken>()))
         .ReturnsAsync((Product?)null);

      _productRepositoryMock
         .Setup(repository => repository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _publisherMock
         .Setup(publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
         .Returns(Task.CompletedTask);

      _mapperMock
         .Setup(mapper => mapper.Map<ProductViewModel>(It.IsAny<Product>()))
         .Returns(new ProductViewModel
         {
            Name = command.Name,
            SKU = command.SKU,
            SupplierId = command.SupplierId
         });

      var result = await _handler.Handle(command, CancellationToken.None);

      result.Name.Should().Be(command.Name);
      result.SKU.Should().Be(command.SKU);
      result.SupplierId.Should().Be(command.SupplierId);
   }

   private static CreateProductCommand BuildValidCommand()
   {
      return new CreateProductCommand
      {
         Name = "Ipad",
         SKU = "ipad/123",
         Description = "Ipad 10 air",
         Price = 900,
         QuantityInStock = 8,
         SupplierId = "e641e362-a1a1-44b0-bb25-f2d7cb296d31",
         ExpiryDate = new DateTime(2026, 7, 27)
      };
   }
}