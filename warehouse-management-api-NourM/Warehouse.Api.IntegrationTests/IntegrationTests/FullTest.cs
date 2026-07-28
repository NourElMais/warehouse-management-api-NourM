using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Warehouse.Api.IntegrationTests.TestUtilities.Builders;
using Warehouse.Api.IntegrationTests.TestUtilities.Helpers;
using Warehouse.Api.IntegrationTests.TestUtilities.TestData;
using Warehouse.Application.ViewModels;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Api.IntegrationTests.IntegrationTests;

public class FullTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public FullTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullBusinessFlow_ShouldCompleteSuccessfully()
        {
            //1- Create Supplier
            var supplierRequest = SupplierBuilder.Create();

            var supplierResponse = await _client.PostAsJsonAsync("/api/suppliers", supplierRequest);
            Assert.Equal(HttpStatusCode.OK, supplierResponse.StatusCode);
            //we get the created supplier's id
            var supplier = await supplierResponse.Content.ReadFromJsonAsync<SupplierViewModel>();
            Assert.NotNull(supplier);
            var supplierId = supplier.Id;
            
            //2- Create product
            var productRequest = ProductBuilder.Create();
            productRequest.SupplierId = TestData.SupplierId;
            var productResponse = await _client.PostAsJsonAsync("/api/products", productRequest);
            Assert.Equal(HttpStatusCode.OK, productResponse.StatusCode);
            var product = await productResponse.Content.ReadFromJsonAsync<ProductViewModel>();
            
            Assert.NotNull(product);
            
            var productId = product.Id;

            
            //3- assign supplier
            var response = await _client.PostAsync($"/api/products/{productId}/assign-supplier/{supplierId}", null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var getResponse = await _client.GetAsync($"/api/products/{productId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            // var content = await getResponse.Content.ReadAsStringAsync();
            // Assert.Contains(supplierId, content);
            var assignedProduct = await getResponse.Content.ReadFromJsonAsync<ProductViewModel>();
            Assert.NotNull(assignedProduct);
            assignedProduct.SupplierId.Should().Be(supplierId);
            
            // 4- Upload image
            using var form =  MultiPartFormHelper.Create(new byte[] { 1, 2, 3 }, "ipad.jpg");

            var imageResponse = await _client.PostAsync($"/api/products/{productId}/image", form);
            Assert.Equal(HttpStatusCode.OK, imageResponse.StatusCode);
            
            //5- update quantity 
            var quantityRequest = new UpdateProductQuantityRequest
            {
                QuantityInStock = 20
            };

            var quantityResponse = await _client.PostAsJsonAsync($"/api/products/{productId}/quantity", quantityRequest);
            Assert.Equal(HttpStatusCode.OK, quantityResponse.StatusCode);
            
            // 6- Update price
            var priceRequest = new UpdateProductPriceRequest
            {
                Price = 1000
            };

            var priceResponse = await _client.PostAsJsonAsync($"/api/products/{productId}/price", priceRequest);
            Assert.Equal(HttpStatusCode.OK, priceResponse.StatusCode);
            
            
            // 7. Archive product 
            var archiveResponse = await _client.DeleteAsync($"/api/products/{productId}");
            Assert.Equal(HttpStatusCode.OK, archiveResponse.StatusCode);


            // 8. verify archived state
            var res = await _client.GetAsync($"/api/products/{productId}");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);

            var archivedProduct = await res.Content.ReadFromJsonAsync<ProductViewModel>();

            Assert.NotNull(archivedProduct);
            archivedProduct.IsArchived.Should().Be(true);

        }
    }
