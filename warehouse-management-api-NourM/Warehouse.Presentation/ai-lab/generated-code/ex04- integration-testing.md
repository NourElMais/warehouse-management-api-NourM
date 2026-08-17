## AI summary 
1) POST /api/products
   Test: CreateProduct_FullInitializationFlow_ShouldReturnOk_WithJsonHeaders_AndPersistProduct
   Verifies:
   HTTP status code is 200 OK
   response content type is application/json
   returned model matches request values:
   ◦ Name
   ◦ SKU
   ◦ Price
   ◦ QuantityInStock
   ◦ ExpiryDate
   ◦ SupplierId
   ◦ IsArchived
   
   persistent server-side effect:
   ◦ follow-up GET /api/products/{id} returns the created product
   --> persisted product still matches expected values

2) POST /api/products/{id}/image
   Test: UploadProductImage_BinaryImageStreamProcessing_ShouldReturnOk_AndPersistServerSideImageRecord
   Verifies:
   HTTP status code is 200 OK
   response content type is text/plain
   response body is not empty
   persistent server-side effect:
   uploaded image record is stored in the fake integration repository
   
   stored image matches:
   ▪ ProductId
   ▪ FileName
   ▪ non-empty FilePath

3) DELETE /api/products/{id}
   Test: DeleteProduct_ResourceTeardown_ShouldReturnOk_AndArchivePersistedProduct
   Verifies:
   HTTP status code is 200 OK
   response content type is text/plain
   response body is not empty
   persistent server-side effect:
   follow-up GET /api/products/{id} still succeeds
   product is now archived:
   ▪ IsArchived == true

   This aligns with the project’s actual delete behavior, which is archive/soft-delete, not physical removal.
   Validation result
   I ran the targeted integration tests:
   dotnet test "C:\Users\HCES\RiderProjects\warehouse-management-api-NourM\warehouse-management-api-NourM\Warehouse.Api.IntegrationTests\Warehouse.Api.IntegrationTests.csproj" --filter "ProductLifecycleEndpointTests"
   Result
   • Passed: 3
   • Failed: 0
   
Summary
   I:
   • inspected the existing integration test structure
   • followed the same CustomWebApplicationFactory + HttpClient pattern
   • created a new separate file: ProductLifecycleEndpointTests.cs
   • added integration tests for: product creation, image upload, delete/archive behavior
   
   Added explicit assertions for:
   ◦ HTTP status codes
   ◦ response headers/content types
   ◦ returned model properties
   ◦ persistent server-side effects
   • updated the integration fake repository to persist uploaded image metadata so the image test could verify storage-side effects
   • ran the new tests successfully
  