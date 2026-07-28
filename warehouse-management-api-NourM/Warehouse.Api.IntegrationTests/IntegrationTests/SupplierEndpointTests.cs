using System.Net;
using System.Net.Http.Json;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Api.IntegrationTests.IntegrationTests;

public class SupplierEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client; //like postmann or swagger

    public SupplierEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    //Test1: create supplier
    [Fact]
    public async Task CreateSupplier_ShouldReturnOk()
    {
        var request = new CreateSupplierRequest()
        {
            Name = "Nour",
            Country = "Lebanon",
            ContactEmail = "nour@email.com",
            PhoneNumber = "03-421605"
        };

        var response = await _client.PostAsJsonAsync("/api/suppliers", request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test2: get supplier 
    [Fact]
    public async Task GetSupplier_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/suppliers");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    //Test3: deactivate supplier 
    [Fact]
    public async Task DeactivateSupplier_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync("/api/suppliers/ba0d85a1-3913-4753-aeea-6504270e3ab1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var getResponse = await _client.GetAsync("/api/suppliers/ba0d85a1-3913-4753-aeea-6504270e3ab1");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);//if it succeeds then its still in the list
        var content = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains("\"isActive\":false", content);
    }
    
    //Test4: assign supplier to product 
    [Fact]
    public async Task  AssignSupplierToProduct_ShouldReturnOK()
    {

        const string productId = "c50d9e28-60be-407d-a163-1af84755c3e0";

        const string supplierId = "ba0d85a1-3913-4753-aeea-6504270e3ab1";

        var response = await _client.PostAsync($"/api/products/{productId}/assign-supplier/{supplierId}", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var getResponse = await _client.GetAsync($"/api/products/{productId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var content = await getResponse.Content.ReadAsStringAsync();

        Assert.Contains(supplierId, content);
    }
}
