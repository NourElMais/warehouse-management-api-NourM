using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Warehouse.Api.IntegrationTests.TestUtilities.Builders;
using Warehouse.Api.IntegrationTests.TestUtilities.TestData;
using Warehouse.Application.ViewModels;

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
        var request = SupplierBuilder.Create();

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
        var response = await _client.DeleteAsync($"/api/suppliers/{TestData.SupplierId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var getResponse = await _client.GetAsync($"/api/suppliers/{TestData.SupplierId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);//if it succeeds then its still in the list
        
        // var content = await getResponse.Content.ReadAsStringAsync();
        // Assert.Contains("\"isActive\":false", content);
        
        var supplier = await getResponse.Content.ReadFromJsonAsync<SupplierViewModel>();
        Assert.NotNull(supplier);
        supplier.IsActive.Should().Be(false);
    }
    
    //Test4: assign supplier to product 
    [Fact]
    public async Task  AssignSupplierToProduct_ShouldReturnOK()
    {

        const string productId = TestData.ProductId;

        const string supplierId = TestData.SupplierId;

        var response = await _client.PostAsync($"/api/products/{productId}/assign-supplier/{supplierId}", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var getResponse = await _client.GetAsync($"/api/products/{productId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        // var content = await getResponse.Content.ReadAsStringAsync();
        //
        // Assert.Contains(supplierId, content);
        var product = await getResponse.Content.ReadFromJsonAsync<ProductViewModel>();

        Assert.NotNull(product);
        product.SupplierId.Should().Be(TestData.SupplierId);
    }
}
