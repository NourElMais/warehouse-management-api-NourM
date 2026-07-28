using System.Net;
using System.Net.Http.Json;
using Warehouse.Domain.Products;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Api.IntegrationTests.IntegrationTests;

public class ProductEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
    private readonly HttpClient _client; //like postmann or swagger

    public ProductEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
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
        var response = await _client.GetAsync("/api/products/35feb37b-05e6-4b53-bb7b-264ecc8714c1");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Laptop", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test3: GET invalid id (not existing) returns 404 
    [Fact]
    public async Task GetProductByInvalidId_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/products/39feb37b-05e6-4b53-bb7b-264ecc8714c1");
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
    

    //Test6: create product returns 201 (in my case it is 200)
    [Fact]
    public async Task CreateProduct_ShouldReturnOk()
    {
        var request = new CreateProductRequest
        {
            Name = "Ipad",
            SKU = "ipad123",
            Description = "Ipad 10 Air",
            Price = 900,
            QuantityInStock = 8,
            SupplierId = "supplier-id1",
            ExpiryDate = new DateTime(2028, 7, 27)
        };

        var response = await _client.PostAsJsonAsync("/api/products", request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        // We verify it was created
        var productsResponse = await _client.GetAsync("/api/products");
        var content = await productsResponse.Content.ReadAsStringAsync();

        Assert.Contains("Ipad", content);
    }
    
    //Test7: duplicate SKU returns 409  (in my case it return 400-> BadRequest)
    [Fact]
    public async Task CreateProduct_DuplicateSKU_ShouldReturnBadRequest()
    {
        var prod = new Product("name", "lap/123", "desc", 1200, 23, "supplier-id1", DateTime.UtcNow.AddYears(2),
            "35feb37b-05e6-4b53-bb7b-264ecc8904c1");
        var response = await _client.PostAsJsonAsync("/api/products", prod);
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
        var response = await _client.PostAsJsonAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0/quantity", request);
        var getResponse = await _client.GetAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0");
        var content = await getResponse.Content.ReadAsStringAsync();

        Assert.Contains("20", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test9: price update works
    [Fact]
    public async Task UpdatePrice_ShouldReturnOk()
    {
        var request = new UpdateProductPriceRequest()
        {
            Price = 120
        };
        var response = await _client.PostAsJsonAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0/price", request);
        var getResponse = await _client.GetAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0");
        var content = await getResponse.Content.ReadAsStringAsync();

        Assert.Contains("120", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test10: delete archives product and deleted product still exists but archived 
    [Fact]
    public async Task DeleteProduct_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var getResponse = await _client.GetAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var content = await getResponse.Content.ReadAsStringAsync();

        Assert.Contains("\"isArchived\":true", content);
        
    }
    

}