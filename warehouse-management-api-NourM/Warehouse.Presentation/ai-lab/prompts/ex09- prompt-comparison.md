## Exercise 9- Comparison between weak and strong prompts
Note: For this exercise, I used Claude code, not Cline because I do not want the AI model to know my project structure in advance.

## Weak Prompt
Create endpoint for products.

## Output of the weak prompt
Models/Product.cs — the Product entity plus ProductInput (create/update) and ProductPatchInput (partial update), with data annotation validation
Services/IProductRepository.cs — repository interface + an in-memory, thread-safe implementation
Controllers/ProductsController.cs — the actual endpoints:
GET /api/products?category=&page=&limit=
GET /api/products/{id}
POST /api/products
PUT /api/products/{id}
PATCH /api/products/{id}
DELETE /api/products/{id}
Program.cs — minimal hosting setup, DI registration, Swagger
ProductsApi.csproj / appsettings.json — project + config

### Result

The weak prompt produced a very basic API.
Because the prompt did not specify the programming language at first, the AI initially generated a Node.js solution.
After asking again for C#, it created several endpoints and added basic validation and Swagger support.

However, it did not include:
a clear controller-service architecture, separate DTOs for all requests and responses, unit and integration tests...

## Strong Prompt

Create ASP.NET Core endpoint for warehouse products using controller-service pattern, DTO validation, unit tests, integration tests, and Swagger summary.

## Output of the strong prompt
Solution structure (3 projects, .NET 8):

WarehouseProducts.sln
├── WarehouseProducts.Api/               # Web API
│   ├── Models/Product.cs
│   ├── Dtos/{ProductDto, CreateProductDto, UpdateProductDto}.cs   # DataAnnotations validation
│   ├── Repositories/{IProductRepository, InMemoryProductRepository}.cs
│   ├── Services/{IProductService, ProductService}.cs               # business logic layer
│   ├── Controllers/ProductsController.cs                           # thin controller, XML docs → Swagger
│   └── Program.cs                                                  # DI + Swagger wiring
├── WarehouseProducts.Api.Tests/         # xUnit + Moq unit tests for ProductService
└── WarehouseProducts.Api.IntegrationTests/   # WebApplicationFactory<Program> end-to-end HTTP tests

Key design points:

Controller → Service → Repository separation; controller only handles HTTP concerns and catches the one expected business exception (duplicate SKU → 409).
Validation via [Required], [StringLength], [Range] on the DTOs — [ApiController] auto-returns 400 on failure, no manual ModelState checks needed.
Swagger: XML doc comments on every action (<summary>, <response>) plus GenerateDocumentationFile in the csproj and IncludeXmlComments in Program.cs, so each endpoint gets a real summary and response-code list in the UI.
Unit tests mock IProductRepository to test ProductService logic in isolation (create/duplicate-SKU/update/delete paths).
Integration tests spin up the real pipeline via WebApplicationFactory<Program> and hit actual HTTP routes, verifying status codes, validation errors, conflicts, and persistence across requests.
Repository is in-memory (thread-safe ConcurrentDictionary) for a runnable, dependency-free demo — swap InMemoryProductRepository for an EF Core implementation against IProductRepository when you're ready for a real database.

### Result

The strong prompt produced a much more complete solution because it specified the expected language, architecture, validation, testing, and documentation requirements.
The result was more accurate, organized, and closer to production style backend code.

## Comparison

The two prompts produced similar core functionality. Both outputs included a product model, CRUD endpoints, validation, Swagger, and an in-memory repository.
The main difference was the level of structure and completeness.

The weak prompt still produced a usable API because the AI made several assumptions on its own.
However, it initially selected the wrong programming language and did not include tests or a clearly separated service layer.

The strong prompt produced a more organized solution with dedicated DTOs, a controller-service-repository structure, unit tests,
integration tests, and more detailed Swagger documentation.

So, a weak prompt may still produce a good result, but the output depends a lot 
on the assumptions made by the AI. 
A stronger prompt is less error prone, and reduces missing requirements.