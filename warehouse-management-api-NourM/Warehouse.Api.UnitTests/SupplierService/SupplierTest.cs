using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Warehouse.Application.Suppliers.Commands;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Api.UnitTests.SupplierService;

public class SupplierTest
{
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<CreateSupplierHandler>>  _loggerMock;
    
    private readonly CreateSupplierHandler _handler;

    public SupplierTest()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreateSupplierHandler>>();
        _handler = new CreateSupplierHandler(_supplierRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }
    

    //Test1: create supplier
        [Fact]
        public async Task CreateSupplier_ShouldSucceed()
        {
            var command = new CreateSupplierCommand()
            {
                Name = "Nour",
                Country = "Lebanon",
                ContactEmail = "nour@mail.com",
                PhoneNumber = "03-421605"
            };
        
           _supplierRepositoryMock.Setup(x=> x.AddAsync(It.IsAny<Supplier>(),CancellationToken.None)).Returns(Task.CompletedTask);
           var expectedResult = new SupplierViewModel
           {
               Name = command.Name,
               Country = command.Country,
               ContactEmail = command.ContactEmail,
               PhoneNumber = command.PhoneNumber,
               IsActive = true
           };

           _mapperMock.Setup(x => x.Map<SupplierViewModel>(It.IsAny<Supplier>())).Returns(expectedResult);

           // Act
           var result = await _handler.Handle(command, CancellationToken.None);

           // Assert
           result.Should().BeEquivalentTo(expectedResult);

    }
        
    
}