using Microsoft.AspNetCore.Mvc;
using Warehouse.Presentation.Controllers;

namespace Warehouse.Tests.Reflection;

public class UnknownDto
{
    [Fact]
    public void UnknownDto_ShouldReturnNotFound()
    {
        // Arrange
        var controller = new ProductsController(null);

        // Act
        var result = controller.GetValidationMetadata("UnknownDto");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void CreateProductRequestDTO_ShouldWork()
    {
        // Arrange
        var controller = new ProductsController(null);

        // Act
        var result = controller.GetValidationMetadata("CreateProductRequest");

        // Assert
        Assert.IsType<OkObjectResult>(result); 
    }
}