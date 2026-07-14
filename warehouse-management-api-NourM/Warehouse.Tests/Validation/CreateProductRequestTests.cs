using System.ComponentModel.DataAnnotations;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Tests.Validation;

public class CreateProductRequestTests
{
    //Test1
    [Fact]
    public void NegativePrice_ShouldFailValidation() {
        //step1: Arrange -> Prepare evrything that the test needs
        var product = new CreateProductRequest()
        {
            Name = "Laptop",
            SKU = "lapp/1234",
            Description = "Gaming laptop",
            Price = -10,   
            QuantityInStock = 5,
            SupplierId = Guid.NewGuid().ToString(),
            ExpiryDate = DateTime.Today.AddDays(10)
        };
        
        //step2: Act -> Validate the request using its Data Annotations.
        var context = new ValidationContext(product);
        var validationResults = new List<ValidationResult>();
        
        bool isValid = Validator.TryValidateObject(
            product,
            context,
            validationResults,
            true);

        // step3: Assert -> verify that the validation failed and that at least one validation error was returned.
        Assert.False(isValid);
        Assert.NotEmpty(validationResults);
    }
    
    //Test2
    [Fact]
    public void EmptyName_ShouldFailValidation() {
        //step1: Arrange -> Prepare evrything that the test needs
        var product = new CreateProductRequest()
        {
            Name = "",
            SKU = "lapp/1234",
            Description = "Gaming laptop",
            Price = 10,   
            QuantityInStock = 5,
            SupplierId = Guid.NewGuid().ToString(),
            ExpiryDate = DateTime.Today.AddDays(10)
        };
        
        //step2: Act -> Validate the request using its Data Annotations.
        var context = new ValidationContext(product);
        var validationResults = new List<ValidationResult>();
        
        bool isValid = Validator.TryValidateObject(
            product,
            context,
            validationResults,
            true);

        // step3: Assert -> verify that the validation failed and that at least one validation error was returned.
        Assert.False(isValid);
        Assert.NotEmpty(validationResults);
    }
    
    //Test3
    [Fact]
    public void ExpiryDateLessThanCurrentDate_ShouldFailValidation()
    {
        var product = new CreateProductRequest()
        {

            Name = "Laptop",
            SKU = "lapp/1234",
            Description = "Gaming laptop",
            Price = 10,
            QuantityInStock = 5,
            SupplierId = Guid.NewGuid().ToString(),
            ExpiryDate = DateTime.Today.AddDays(-10)
        };
        var context = new ValidationContext(product);
        var validationResults = new List<ValidationResult>();
        
        bool isValid = Validator.TryValidateObject(
            product,
            context,
            validationResults,
            true);
        Assert.False(isValid);
        Assert.NotEmpty(validationResults);
    }
    
    
    
}