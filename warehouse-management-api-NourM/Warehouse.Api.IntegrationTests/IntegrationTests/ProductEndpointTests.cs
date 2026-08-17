using System.Net;
using System.Net.Http.Json;
using Warehouse.Api.IntegrationTests.TestUtilities.Builders;
using Warehouse.Api.IntegrationTests.TestUtilities.TestData;
using Warehouse.Application.ViewModels;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Api.IntegrationTests.IntegrationTests;

public class ProductEndpointTests : IDisposable
    {
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;//like postmann or swagger

    public ProductEndpointTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    //automatically called after each test finishes
    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    //Test1: GET /api/products returns seeded products
    [Fact]
    public async Task GetAllProducts_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/products");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test2: GET /api/products/{id} 
    [Fact]
    public async Task GetProductById_ShouldReturnOk()
    {
        var response = await _client.GetAsync($"/api/products/{TestData.LaptopId}");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Laptop", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test3: GET invalid id (not existing) returns 404 
    [Fact]
    public async Task GetProductByInvalidId_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync($"/api/products/{TestData.InvalidProductId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    //Test4: GET search works 
    [Fact]
    public async Task SearchByProductNameAndSupplierName_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/products/search?name=Laptop");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test5: GET low-stock works
    [Fact]
    public async Task GetLowStockProducts_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/products/low-stock?threshold=10");
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Mouse", content);
    }

    [Fact]
    public async Task GetOutOfStockProducts_ShouldReturnOnlyNonArchivedProductsWithZeroQuantity()
    {
        var response = await _client.GetAsync("/api/products/out-of-stock");
        var products = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(products);
        Assert.Single(products!);
        Assert.Equal("Headset", products[0].Name);
        Assert.Equal(0, products[0].QuantityInStock);
        Assert.False(products[0].IsArchived);
    }
    

    //Test6: create product returns 201 (in my case it is 200)
    [Fact]
    public async Task CreateProduct_ShouldReturnOk()
    {
        var request = ProductBuilder.Create();

        var response = await _client.PostAsJsonAsync("/api/products", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test7: duplicate SKU returns 409  (in my case it return 400-> BadRequest)
    [Fact]
    public async Task CreateProduct_DuplicateSKU_ShouldReturnBadRequest()
    {
        var request = ProductBuilder.Create();
        request.SKU = "lap/123";
        var response = await _client.PostAsJsonAsync("/api/products", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    //Test8: quantity update works
    [Fact]
    public async Task UpdateQuantity_ShouldReturnOk()
    {
        var request = new UpdateProductQuantityRequest()
        {
            QuantityInStock = 20
        };
        var response = await _client.PostAsJsonAsync($"/api/products/{TestData.ProductId}/quantity", request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var getResponse = await _client.GetAsync($"/api/products/{TestData.ProductId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var product = await getResponse.Content.ReadFromJsonAsync<ProductViewModel>();

        Assert.NotNull(product);
        Assert.Equal(20, product.QuantityInStock);
    }
    
    //Test9: price update works
    [Fact]
    public async Task UpdatePrice_ShouldReturnOk()
    {
        var request = new UpdateProductPriceRequest()
        {
            Price = 120
        };
        
        var response = await _client.PostAsJsonAsync($"/api/products/{TestData.ProductId}/price", request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var getResponse = await _client.GetAsync($"/api/products/{TestData.ProductId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var product = await getResponse.Content.ReadFromJsonAsync<ProductViewModel>();

        Assert.NotNull(product);
        Assert.Equal(120, product.Price);
    }
    
    //Test10: delete archives product and deleted product still exists but archived 
    [Fact]
    public async Task DeleteProduct_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync($"/api/products/{TestData.ProductId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var getResponse = await _client.GetAsync($"/api/products/{TestData.ProductId}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        // var content = await getResponse.Content.ReadAsStringAsync();
        //
        // Assert.Contains("\"isArchived\":true", content);
        var product = await getResponse.Content.ReadFromJsonAsync<ProductViewModel>();

        Assert.NotNull(product);
        Assert.True(product.IsArchived);
        
    }
    
    //Negative integration tests (for bonus 3)
    //Creating a product with missing name 
    [Fact]
    public async Task CreateProduct_MissingName_ShouldReturnBadRequest()
    {
        var request = ProductBuilder.Create();
        request.Name = null;

        var response = await _client.PostAsJsonAsync("/api/products", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    //Create a product with a past expiry date
    [Fact]
    public async Task CreateProduct_PastExpiryDate_ShouldReturnBadRequest()
    {
        var request = ProductBuilder.Create();
        request.ExpiryDate = DateTime.UtcNow.AddDays(-10);

        var response = await _client.PostAsJsonAsync("/api/products", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    
    

}