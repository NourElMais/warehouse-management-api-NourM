# Warehouse Management API

AI-generated API documentation based on the current codebase.

## API README

### Overview
This project is an ASP.NET Core Web API for warehouse operations. It manages:
- products
- suppliers
- inventory statistics
- product images
- cache statistics
- OData querying

The API follows a layered architecture with:
- `Warehouse.Presentation` for controllers, middleware, contracts, Swagger, and API concerns
- `Warehouse.Application` for commands, queries, handlers, view models, and use-case orchestration
- `Warehouse.Domain` for core entities and repository interfaces
- `Warehouse.Infrastructure` for EF Core, storage, messaging, caching, health checks, and repository implementations

### Base routing
- REST API base: `/api`
- OData base: `/odata`

### Authentication and authorization
The API uses JWT bearer authentication and role-based authorization.

Configured policies:
- `Admin`: requires role `admin`
- `UserOrAdmin`: requires role `user` or `admin`

Swagger is configured with a Bearer token security definition.

### Localization
Supported cultures configured in the app:
- `en`
- `fr`
- `ar`

The server-time endpoint specifically accepts:
- `en-US`
- `fr-FR`
- `ar-LB`

### Operational endpoints
The application also exposes:
- `/health`
- `/health-ui`
- `/hangfire`
- `/swagger`

## Common response shapes

### ProductViewModel
```json
{
  "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
  "name": "Laptop",
  "sku": "lap/123",
  "price": 1200.0,
  "quantityInStock": 23,
  "expiryDate": "2027-08-04T00:00:00Z",
  "isArchived": false,
  "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "supplierName": "Tech Supplier"
}
```

### SupplierViewModel
```json
{
  "id": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "name": "Tech Supplier",
  "country": "Lebanon",
  "contactEmail": "contact@techsupplier.com",
  "phoneNumber": "+96170123456",
  "isActive": true
}
```

### Error shape
The project defines an error contract:
```json
{
  "errorCode": "NOT_FOUND",
  "message": "Resource was not found.",
  "traceId": "00-abc123-def456-00"
}
```

Some controller actions also return plain localized error strings or model validation payloads for bad requests.

---

## Endpoint summaries

## Products API

Base route: `/api/products`

### 1) Get all products
- **Method:** `GET`
- **Route:** `/api/products`
- **Auth:** `UserOrAdmin`
- **Query params:**
  - `onlyAvailable` (`bool`, optional, default `false`)
- **Summary:** Returns all products, optionally filtering to available products only.

#### Request example
```http
GET /api/products?onlyAvailable=true HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
[
  {
    "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
    "name": "Laptop",
    "sku": "lap/123",
    "price": 1200.0,
    "quantityInStock": 23,
    "expiryDate": "2027-08-04T00:00:00Z",
    "isArchived": false,
    "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
    "supplierName": "Tech Supplier"
  }
]
```

### 2) Get product by ID
- **Method:** `GET`
- **Route:** `/api/products/{id}`
- **Auth:** `UserOrAdmin`
- **Summary:** Returns a single product by ID.

#### Request example
```http
GET /api/products/35feb37b-05e6-4b53-bb7b-264ecc8714c1 HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
{
  "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
  "name": "Laptop",
  "sku": "lap/123",
  "price": 1200.0,
  "quantityInStock": 23,
  "expiryDate": "2027-08-04T00:00:00Z",
  "isArchived": false,
  "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "supplierName": "Tech Supplier"
}
```

### 3) Search products
- **Method:** `GET`
- **Route:** `/api/products/search`
- **Auth:** `UserOrAdmin`
- **Query params:**
  - `name` (`string?`)
  - `supplier` (`string?`)
- **Summary:** Searches products by product name and/or supplier.

#### Request example
```http
GET /api/products/search?name=laptop&supplier=tech HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
[
  {
    "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
    "name": "Laptop",
    "sku": "lap/123",
    "price": 1200.0,
    "quantityInStock": 23,
    "expiryDate": "2027-08-04T00:00:00Z",
    "isArchived": false,
    "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
    "supplierName": "Tech Supplier"
  }
]
```

### 4) Get expiring-soon products
- **Method:** `GET`
- **Route:** `/api/products/expiring-soon`
- **Auth:** `UserOrAdmin`
- **Query params:**
  - `daysAhead` (`int`, range `1..365`, default `30`)
- **Summary:** Returns products expiring within the given time window.

#### Request example
```http
GET /api/products/expiring-soon?daysAhead=30 HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
[
  {
    "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
    "name": "Yogurt",
    "sku": "food/001",
    "price": 2.5,
    "quantityInStock": 50,
    "expiryDate": "2026-08-20T00:00:00Z",
    "isArchived": false,
    "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
    "supplierName": "Fresh Foods"
  }
]
```

### 5) Create product
- **Method:** `POST`
- **Route:** `/api/products`
- **Auth:** `Admin`
- **Summary:** Creates a new product.

#### Request body
```json
{
  "name": "Gaming Laptop",
  "sku": "lap/999",
  "description": "High-end laptop for gaming",
  "price": 1899.99,
  "quantityInStock": 10,
  "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "expiryDate": "2027-12-31T00:00:00Z"
}
```

#### Response example
```json
{
  "id": "9c9f2bb1-7b88-4df7-8a28-ef64f88f57f5",
  "name": "Gaming Laptop",
  "sku": "lap/999",
  "price": 1899.99,
  "quantityInStock": 10,
  "expiryDate": "2027-12-31T00:00:00Z",
  "isArchived": false,
  "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "supplierName": null
}
```

### 6) Update product quantity
- **Method:** `POST`
- **Route:** `/api/products/{id}/quantity`
- **Auth:** `Admin`
- **Summary:** Updates a product's stock quantity.

#### Request body
```json
{
  "quantityInStock": 42
}
```

#### Response example
```json
{
  "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
  "name": "Laptop",
  "sku": "lap/123",
  "price": 1200.0,
  "quantityInStock": 42,
  "expiryDate": "2027-08-04T00:00:00Z",
  "isArchived": false,
  "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "supplierName": "Tech Supplier"
}
```

### 7) Update product price
- **Method:** `POST`
- **Route:** `/api/products/{id}/price`
- **Auth:** `Admin`
- **Summary:** Updates a product's price.

#### Request body
```json
{
  "price": 1299.99
}
```

#### Response example
```json
{
  "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
  "name": "Laptop",
  "sku": "lap/123",
  "price": 1299.99,
  "quantityInStock": 23,
  "expiryDate": "2027-08-04T00:00:00Z",
  "isArchived": false,
  "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "supplierName": "Tech Supplier"
}
```

### 8) Archive product
- **Method:** `DELETE`
- **Route:** `/api/products/{id}`
- **Auth:** `Admin`
- **Summary:** Soft-deletes (archives) a product.

#### Request example
```http
DELETE /api/products/35feb37b-05e6-4b53-bb7b-264ecc8714c1 HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```text
Product archived successfully.
```

> Note: The exact message is localized through shared resources.

### 9) Assign supplier to product
- **Method:** `POST`
- **Route:** `/api/products/{id}/assign-supplier/{supplierId}`
- **Auth:** `Admin`
- **Summary:** Assigns an existing supplier to an existing product.

#### Request example
```http
POST /api/products/35feb37b-05e6-4b53-bb7b-264ecc8714c1/assign-supplier/ba0d85a1-3913-4753-aeea-6504270e3ab1 HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
{
  "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
  "name": "Laptop",
  "sku": "lap/123",
  "price": 1200.0,
  "quantityInStock": 23,
  "expiryDate": "2027-08-04T00:00:00Z",
  "isArchived": false,
  "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "supplierName": "Tech Supplier"
}
```

### 10) Restore archived product
- **Method:** `POST`
- **Route:** `/api/products/{id}/restore`
- **Auth:** `Admin`
- **Summary:** Restores an archived product.

#### Request example
```http
POST /api/products/35feb37b-05e6-4b53-bb7b-264ecc8714c1/restore HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
{
  "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
  "name": "Laptop",
  "sku": "lap/123",
  "price": 1200.0,
  "quantityInStock": 23,
  "expiryDate": "2027-08-04T00:00:00Z",
  "isArchived": false,
  "supplierId": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "supplierName": "Tech Supplier"
}
```

### 11) Get low-stock products
- **Method:** `GET`
- **Route:** `/api/products/low-stock`
- **Auth:** `UserOrAdmin`
- **Query params:**
  - `threshold` (`int`, optional, default `5`)
- **Summary:** Returns non-archived products at or below the threshold.

#### Request example
```http
GET /api/products/low-stock?threshold=5 HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
[
  {
    "id": "c50d9e28-60be-407d-a163-1af84755c3e0",
    "name": "Mouse",
    "sku": "mouse/123",
    "price": 100.0,
    "quantityInStock": 3,
    "expiryDate": "2027-08-04T00:00:00Z",
    "isArchived": false,
    "supplierId": "supplier-id2",
    "supplierName": "Accessory Supplier"
  }
]
```

### 12) Get product statistics
- **Method:** `GET`
- **Route:** `/api/products/statistics`
- **Auth:** `UserOrAdmin`
- **Summary:** Returns aggregate product statistics.

#### Request example
```http
GET /api/products/statistics HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
{
  "totalProducts": 120,
  "activeProducts": 110,
  "archivedProducts": 10,
  "lowStockProducts": 8
}
```

### 13) Get localized server time
- **Method:** `GET`
- **Route:** `/api/products/server-time`
- **Auth:** `UserOrAdmin`
- **Headers:**
  - `Accept-Language: en-US | fr-FR | ar-LB`
- **Summary:** Returns server time formatted according to the requested supported locale.

#### Request example
```http
GET /api/products/server-time HTTP/1.1
Authorization: Bearer <jwt>
Accept-Language: fr-FR
```

#### Response example
```json
"04 août 2026 16:42:11"
```

### 14) Upload product image
- **Method:** `POST`
- **Route:** `/api/products/{id}/image`
- **Auth:** `Admin`
- **Content-Type:** `multipart/form-data`
- **Summary:** Uploads a product image after validating presence, extension, and size.

#### Request example
```http
POST /api/products/35feb37b-05e6-4b53-bb7b-264ecc8714c1/image HTTP/1.1
Authorization: Bearer <jwt>
Content-Type: multipart/form-data

Form field: image=<binary file>
```

#### Response example
```text
Image uploaded successfully.
```

#### Validation notes
Possible bad-request cases include:
- invalid product ID
- missing image
- empty image
- invalid file extension
- image too large

### 15) Download product image
- **Method:** `GET`
- **Route:** `/api/products/{id}/image`
- **Auth:** `UserOrAdmin`
- **Summary:** Downloads the stored product image stream.

#### Request example
```http
GET /api/products/35feb37b-05e6-4b53-bb7b-264ecc8714c1/image HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```http
HTTP/1.1 200 OK
Content-Type: image/png

<binary stream>
```

### 16) Get cache statistics
- **Method:** `GET`
- **Route:** `/api/products/cache-statistics`
- **Auth:** `Admin`
- **Summary:** Returns application cache usage statistics.

#### Request example
```http
GET /api/products/cache-statistics HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
{
  "cachedKeys": ["products:list", "products:statistics"],
  "hitCount": 20,
  "missCount": 4,
  "lastCacheRefreshTime": "2026-08-04T15:00:00Z"
}
```

---

## Suppliers API

Base route: `/api/suppliers`

### 1) Get all suppliers
- **Method:** `GET`
- **Route:** `/api/suppliers`
- **Auth:** `UserOrAdmin`
- **Summary:** Returns all suppliers.

#### Request example
```http
GET /api/suppliers HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
[
  {
    "id": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
    "name": "Tech Supplier",
    "country": "Lebanon",
    "contactEmail": "contact@techsupplier.com",
    "phoneNumber": "+96170123456",
    "isActive": true
  }
]
```

### 2) Get supplier by ID
- **Method:** `GET`
- **Route:** `/api/suppliers/{id}`
- **Auth:** `UserOrAdmin`
- **Summary:** Returns one supplier by ID.

#### Request example
```http
GET /api/suppliers/ba0d85a1-3913-4753-aeea-6504270e3ab1 HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
{
  "id": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
  "name": "Tech Supplier",
  "country": "Lebanon",
  "contactEmail": "contact@techsupplier.com",
  "phoneNumber": "+96170123456",
  "isActive": true
}
```

### 3) Create supplier
- **Method:** `POST`
- **Route:** `/api/suppliers`
- **Auth:** `Admin`
- **Summary:** Creates a new supplier.

#### Request body
```json
{
  "name": "Fresh Foods",
  "country": "Lebanon",
  "contactEmail": "sales@freshfoods.com",
  "phoneNumber": "+96176111222"
}
```

#### Response example
```json
{
  "id": "53fcce91-cd1b-4ea9-b4de-88a98738b968",
  "name": "Fresh Foods",
  "country": "Lebanon",
  "contactEmail": "sales@freshfoods.com",
  "phoneNumber": "+96176111222",
  "isActive": true
}
```

### 4) Deactivate supplier
- **Method:** `DELETE`
- **Route:** `/api/suppliers/{id}`
- **Auth:** `Admin`
- **Summary:** Soft-deactivates a supplier.

#### Request example
```http
DELETE /api/suppliers/ba0d85a1-3913-4753-aeea-6504270e3ab1 HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```text
Supplier deleted successfully.
```

### 5) Get supplier statistics
- **Method:** `GET`
- **Route:** `/api/suppliers/statistics`
- **Auth:** `Admin`
- **Summary:** Returns aggregate supplier statistics.

#### Request example
```http
GET /api/suppliers/statistics HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
{
  "totalSuppliers": 25,
  "activeSuppliers": 22,
  "inactiveSuppliers": 3
}
```

---

## Inventory API

Base route: `/api/inventory`

### 1) Get inventory dashboard
- **Method:** `GET`
- **Route:** `/api/inventory/dashboard`
- **Auth:** `Admin`
- **Summary:** Returns a combined dashboard with product and supplier statistics.

#### Request example
```http
GET /api/inventory/dashboard HTTP/1.1
Authorization: Bearer <jwt>
```

#### Response example
```json
{
  "productStatistics": {
    "totalProducts": 120,
    "activeProducts": 110,
    "archivedProducts": 10,
    "lowStockProducts": 8
  },
  "supplierStatistics": {
    "totalSuppliers": 25,
    "activeSuppliers": 22,
    "inactiveSuppliers": 3
  }
}
```

---

## OData API

Base route: `/odata`

These endpoints expose `IQueryable` entity sets and support OData query options such as:
- `$select`
- `$filter`
- `$orderby`
- `$expand`
- `$count`
- `$top` (max configured top is 100)

### 1) Query products via OData
- **Method:** `GET`
- **Route:** `/odata/products`
- **Auth:** no explicit `[Authorize]` attribute on this controller
- **Summary:** Query products directly using OData conventions.

#### Request example
```http
GET /odata/products?$filter=contains(Name,'Lap')&$orderby=Price desc&$top=10 HTTP/1.1
```

#### Response example
```json
{
  "value": [
    {
      "id": "35feb37b-05e6-4b53-bb7b-264ecc8714c1",
      "name": "Laptop",
      "sku": "lap/123"
    }
  ]
}
```

### 2) Query suppliers via OData
- **Method:** `GET`
- **Route:** `/odata/suppliers`
- **Auth:** no explicit `[Authorize]` attribute on this controller
- **Summary:** Query suppliers directly using OData conventions.

#### Request example
```http
GET /odata/suppliers?$filter=IsActive eq true&$top=20 HTTP/1.1
```

#### Response example
```json
{
  "value": [
    {
      "id": "ba0d85a1-3913-4753-aeea-6504270e3ab1",
      "name": "Tech Supplier",
      "country": "Lebanon"
    }
  ]
}
```

---

## Request contract notes

### CreateProductRequest
Validation inferred from data annotations:
- `Name`: required, max 50 chars
- `SKU`: required, max 200 chars
- `Description`: required, max 500 chars
- `Price`: required, minimum `0.01`
- `QuantityInStock`: required, minimum `0`
- `SupplierId`: required, max 500 chars
- `ExpiryDate`: validated by `FutureDateAttribute`

### CreateSupplierRequest
- `Name`: required, max 50 chars
- `Country`: required, max 50 chars
- `ContactEmail`: required, valid email
- `PhoneNumber`: required, valid phone

### UpdateProductQuantityRequest
- `QuantityInStock`: required, minimum `0`

### UpdateProductPriceRequest
- `Price`: required, minimum `0`

### GetExpiringSoonProductsRequest
- `DaysAhead`: range `1..365`, default `30`

---

## Architecture notes

### Clean Architecture and CQRS
The project is structured around Clean Architecture and CQRS:
- controllers are thin and delegate work through MediatR
- commands model write operations
- queries model read operations
- handlers implement application use cases
- repositories are abstractions in the Domain layer and implementations in Infrastructure

### Presentation layer responsibilities
The `Warehouse.Presentation` project handles:
- HTTP routing
- model binding
- authorization attributes
- localization-aware messaging
- middleware pipeline
- Swagger/OpenAPI

### Application layer responsibilities
The `Warehouse.Application` project contains:
- MediatR handlers
- commands and queries
- view models
- orchestration of repository/storage/messaging services

### Domain layer responsibilities
The `Warehouse.Domain` project contains:
- entities such as products, suppliers, and product images
- business rules and domain behaviors
- repository interfaces

### Infrastructure layer responsibilities
The `Warehouse.Infrastructure` project provides:
- EF Core database access
- PostgreSQL integration
- Redis cache integration
- MinIO-based storage service
- RabbitMQ publishing
- Hangfire background jobs
- health checks

### Middleware pipeline
The application registers custom middleware for:
- request timing
- correlation IDs
- exception handling

### Swagger and API discoverability
Swagger is enabled and configured with:
- bearer token authentication support
- a custom `Accept-Language` header operation filter

### Background jobs and observability
Operational features include:
- Hangfire dashboard at `/hangfire`
- health check endpoint at `/health`
- health UI at `/health-ui`
- Serilog file and console logging

### Storage and images
Product image download is served through the storage abstraction. Uploading validates file input first, then delegates image persistence and storage behavior through the application layer.

---

## Notes and limitations

- Some bad-request responses are plain localized strings rather than the shared `ApiErrorResponse` contract.
- Success messages such as archive/delete/upload are localized resource strings, so exact text may vary by language/resource content.
- OData endpoints expose domain entities directly from EF Core queryables.
- The server-time endpoint returns a JSON string rather than an object.
