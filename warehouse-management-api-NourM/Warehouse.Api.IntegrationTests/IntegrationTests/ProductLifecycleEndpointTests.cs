using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Api.IntegrationTests.TestUtilities.Builders;
using Warehouse.Api.IntegrationTests.TestUtilities.Helpers;
using Warehouse.Api.IntegrationTests.TestUtilities.TestData;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;

namespace Warehouse.Api.IntegrationTests.IntegrationTests;

public class ProductLifecycleEndpointTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ProductLifecycleEndpointTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Fact]
    public async Task CreateProduct_FullInitializationFlow_ShouldReturnOk_WithJsonHeaders_AndPersistProduct()
    {
        var request = ProductBuilder.Create();

        var response = await _client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");

        var createdProduct = await response.Content.ReadFromJsonAsync<ProductViewModel>();

        createdProduct.Should().NotBeNull();
        createdProduct!.Id.Should().NotBeNullOrWhiteSpace();
        createdProduct.Name.Should().Be(request.Name);
        createdProduct.SKU.Should().Be(request.SKU);
        createdProduct.Price.Should().Be(request.Price);
        createdProduct.QuantityInStock.Should().Be(request.QuantityInStock);
        createdProduct.ExpiryDate.Should().BeCloseTo(request.ExpiryDate, TimeSpan.FromSeconds(1));
        createdProduct.SupplierId.Should().Be(request.SupplierId);
        createdProduct.IsArchived.Should().BeFalse();

        var getResponse = await _client.GetAsync($"/api/products/{createdProduct.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Content.Headers.ContentType.Should().NotBeNull();
        getResponse.Content.Headers.ContentType!.MediaType.Should().Be("application/json");

        var persistedProduct = await getResponse.Content.ReadFromJsonAsync<ProductViewModel>();

        persistedProduct.Should().NotBeNull();
        persistedProduct!.Id.Should().Be(createdProduct.Id);
        persistedProduct.Name.Should().Be(request.Name);
        persistedProduct.SKU.Should().Be(request.SKU);
        persistedProduct.Price.Should().Be(request.Price);
        persistedProduct.QuantityInStock.Should().Be(request.QuantityInStock);
        persistedProduct.SupplierId.Should().Be(request.SupplierId);
        persistedProduct.IsArchived.Should().BeFalse();
    }

    [Fact]
    public async Task UploadProductImage_BinaryImageStreamProcessing_ShouldReturnOk_AndPersistServerSideImageRecord()
    {
        const string productId = TestData.ProductId;
        byte[] imageBytes = [137, 80, 78, 71, 13, 10, 26, 10, 1, 2, 3, 4, 5];
        using var form = MultiPartFormHelper.Create(imageBytes, "test-image.png");

        var response = await _client.PostAsync($"/api/products/{productId}/image", form);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/plain");

        var body = await response.Content.ReadAsStringAsync();
        body.Should().NotBeNullOrWhiteSpace();

        using var scope = _factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

        var storedImage = await repository.GetImageAsync(productId, CancellationToken.None);

        storedImage.Should().NotBeNull();
        storedImage!.ProductId.Should().Be(productId);
        storedImage.FileName.Should().Be("test-image.png");
        storedImage.FilePath.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task DeleteProduct_ResourceTeardown_ShouldReturnOk_AndArchivePersistedProduct()
    {
        const string productId = TestData.ProductId;

        var response = await _client.DeleteAsync($"/api/products/{productId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/plain");

        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().NotBeNullOrWhiteSpace();

        var getResponse = await _client.GetAsync($"/api/products/{productId}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Content.Headers.ContentType.Should().NotBeNull();
        getResponse.Content.Headers.ContentType!.MediaType.Should().Be("application/json");

        var archivedProduct = await getResponse.Content.ReadFromJsonAsync<ProductViewModel>();

        archivedProduct.Should().NotBeNull();
        archivedProduct!.Id.Should().Be(productId);
        archivedProduct.IsArchived.Should().BeTrue();
        archivedProduct.Name.Should().NotBeNullOrWhiteSpace();
        archivedProduct.SKU.Should().NotBeNullOrWhiteSpace();
    }
}