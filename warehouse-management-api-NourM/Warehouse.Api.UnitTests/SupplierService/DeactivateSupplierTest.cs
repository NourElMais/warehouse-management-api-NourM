using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Warehouse.Application.Suppliers.Commands;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Api.UnitTests.SupplierService;

public class DeactivateSupplierTest
{
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private  readonly Mock<ILogger<DeactivateSupplierHandler>> _loggerMock;
    private readonly DeactivateSupplierHandler _handler;

    public DeactivateSupplierTest()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<DeactivateSupplierHandler>>();
        _handler = new DeactivateSupplierHandler(_supplierRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        
    }
    
    //Test1: Deactivate Supplier
    [Fact]
    public async Task DeactivateSupplier_ShouldMakeIsActiveFalse()
    {
        var command = new DeactivateSupplierCommand("supplier-id");
        var supplier = new Supplier("Nour", "Lebanon", "nour@mail.com", "03-421605", "supplier-id");
          
        _supplierRepositoryMock.Setup(x => x.GetByIdAsync("supplier-id", CancellationToken.None)).ReturnsAsync(supplier);
        var expectedResult = new SupplierViewModel()
        {
            Id = supplier.Id,
            IsActive = false
        };
        
        _mapperMock.Setup(x => x.Map<SupplierViewModel>(It.IsAny<Supplier>())).Returns(expectedResult);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.IsActive.Should().Be(false);
        supplier.IsActive.Should().Be(false);
    }
    
}