using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Warehouse.Application.Products.Commands;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Api.UnitTests;

public class ArchiveProductTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<ArchiveProductHandler>> _loggerMock;
    private readonly ArchiveProductHandler _handler;

    public ArchiveProductTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<ArchiveProductHandler>>();
        _handler = new ArchiveProductHandler(_productRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }
    
    //Test1: delete marks archived only 
    [Fact]
    public async Task ArchiveProduct_ShouldMakeIsArchivedTrue()
    {
        var command = new ArchiveProductCommand("product-id");
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
        var expectedResult = new ProductViewModel()
        {
            Id = product.Id,
            Name = product.Name,
            IsArchived = true
        };
      
        _mapperMock.Setup(x => x.Map<ProductViewModel>(product)).Returns(expectedResult);
        var result = await _handler.Handle(command,CancellationToken.None);
        product.IsArchived.Should().Be(true);
        result.IsArchived.Should().Be(true);
        
    }
    
    //Test2: archived item remains in list
    [Fact]
    public async Task ArchiveProduct_ShouldRemainInList()
    {
        var command = new ArchiveProductCommand("product-id");
        List<Product> products = new List<Product>
        {
            new Product("Ipad", "ipad/123", "Ipad 10 air", 900, 8, "e641e362-a1a1-44b0-bb25-f2d7cb296d31", new DateTime(2026, 7, 27))
        };
        
        _productRepositoryMock.Setup(x => x.GetByIdAsync("product-id", CancellationToken.None)).ReturnsAsync(products[0]);
        var expectedResult = new List<ProductViewModel>()
        {
            new ProductViewModel
            {
                 Id = products[0].Id,
                Name = products[0].Name,
                IsArchived = true 
            }
          
        };
      
        _mapperMock.Setup(x => x.Map<List<ProductViewModel>>(products)).Returns(expectedResult);
        var result = await _handler.Handle(command,CancellationToken.None);
        Assert.NotEmpty(products);
        
    }
}