using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Warehouse.Application.Cache;
using Warehouse.Application.Products.Commands;
using Warehouse.Application.Products.GetProductsStatistics;
using Warehouse.Application.Products.Queries;
using Warehouse.Presentation.Contracts;
using Warehouse.Presentation.Resources;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public ProductsController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }
// ASP.NET creates a CancellationToken for each HTTP request,
// so we pass it through all application layers (Controller → MediatR → Handler → Repository → EF Core)
// so that if the client cancels the request (for example if he closes the browser),
// every layer is notified and can stop its work instead of wasting resources.

    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet]
    public async Task<ActionResult> GetProducts(CancellationToken cancellationToken, [FromQuery] bool onlyAvailable = false)
    {
        var role = User.FindFirst("role")?.Value;

        Console.WriteLine("ROLE FROM TOKEN: " + role);
        var products = await _mediator.Send(new ListProductsQuery(onlyAvailable), cancellationToken);
        return Ok(products);
    }

    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetProductById([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest(SharedResources.InvalidID);

        var product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        return Ok(product);
    }

    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("search")]
    public async Task<ActionResult> GetProductsBySearch([FromQuery] string? name, [FromQuery] string? supplier, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(supplier))
            return BadRequest(SharedResources.SearchInputRequired);

        var products = await _mediator.Send(new SearchProductsQuery(name, supplier), cancellationToken);
        return Ok(products);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand
        {
            Name = request.Name,
            SKU = request.SKU,
            Description = request.Description,
            Price = request.Price,
            QuantityInStock = request.QuantityInStock,
            SupplierId = request.SupplierId,
            ExpiryDate = request.ExpiryDate
        };

        var product = await _mediator.Send(command, cancellationToken);

        return Ok(product);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("{id}/quantity")]
    public async Task<ActionResult> UpdateQuantity([FromRoute] string id, [FromBody] UpdateProductQuantityRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest(SharedResources.InvalidID);

        var product = await _mediator.Send(new UpdateProductQuantityCommand(id, request.QuantityInStock), cancellationToken);
        
        return Ok(product);
    }
    
    [Authorize(Policy = "Admin")]
    [HttpPost("{id}/price")]
    public async Task<ActionResult> UpdatePrice([FromRoute] string id, [FromBody] UpdateProductPriceRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest(SharedResources.InvalidID);

        var product = await _mediator.Send(new UpdateProductPriceCommand(id, request.Price), cancellationToken);
        return Ok(product);
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest(SharedResources.InvalidID);

        var product = await _mediator.Send(new ArchiveProductCommand(id), cancellationToken);
        return Ok(SharedResources.ProductArchived);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("{id}/assign-supplier/{supplierId}")]
    public async Task<ActionResult> AssignSupplierToProduct([FromRoute] string id, [FromRoute] string supplierId, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest(SharedResources.InvalidID);

        if (!Guid.TryParse(supplierId, out var g))
            return BadRequest(SharedResources.InvalidID);

        var product = await _mediator.Send(new AssignSupplierToProductCommand(id, supplierId), cancellationToken);

        return Ok(product);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("{id}/restore")]
    public async Task<ActionResult> RestoreProduct([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest(SharedResources.InvalidID);

        var product = await _mediator.Send(new RestoreProductCommand(id), cancellationToken);

        return Ok(product);
    }
    
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("low-stock")]
    public async Task<ActionResult> GetLowStockProducts(CancellationToken cancellationToken, [FromQuery] int threshold = 5)
    {
        var products = await _mediator.Send(new GetLowStockProductsQuery(threshold), cancellationToken);
        return Ok(products);
    }
    
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("statistics")]
    public async Task<ActionResult> GetProductStatistics(CancellationToken cancellationToken)
    {
        var statistics = await _mediator.Send(new GetProductsStatisticsQuery(), cancellationToken);
        return Ok(statistics);
    }
    
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("server-time")]
    public async Task<ActionResult> GetServerTime(CancellationToken cancellationToken, [FromHeader(Name = "Accept-Language")] string language)
    {
        if (language != "en-US" && language != "fr-FR" && language != "ar-LB")
            return BadRequest(_localizer["LanguageNotSupported"].Value);

        var result = await _mediator.Send(new GetServerTimeQuery(language), cancellationToken);
        return Ok(result);
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("{id}/image")]
    public async Task<ActionResult> UploadImage([FromRoute] string id, IFormFile? image, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest(SharedResources.InvalidID);
        if (image is null)
            return BadRequest(SharedResources.EmptyImageViolation);
        
        var result = await _mediator.Send(new UploadProductImageCommand(id, image.FileName, image.Length), cancellationToken);

        if (result == UploadProductImageResult.EmptyImage)
            return BadRequest(SharedResources.EmptyImageViolation);

        if (result == UploadProductImageResult.InvalidExtension)
            return BadRequest(SharedResources.ImageTypeViolation);

        if (result == UploadProductImageResult.FileTooLarge)
            return BadRequest(SharedResources.ImageSizeViolation);

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, image.FileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream, cancellationToken);

        return Ok(SharedResources.ImageUploaded);
    }
    
    // Challenge– Cache Statistics Endpoint
    [Authorize(Policy = "Admin")]
    [HttpGet("cache-statistics")]
    public async Task<IActionResult> GetCacheStatistics(CancellationToken cancellationToken)
    {
        var stats = await _mediator.Send(new GetCacheStatisticsQuery(), cancellationToken);
        return Ok(stats);
    }
    
    [AllowAnonymous]
    [HttpGet("debug-claims")]
    public IActionResult DebugClaims()
    {
        return Ok(User.Claims.Select(c => new
        {
            c.Type,
            c.Value
        }));
    }
    
    //Endpoint to make the role=admin
    // [HttpPost("make-admin")]
    // public async Task<IActionResult> MakeAdmin()
    // {
    //     string uid = "PqaFzzBm8tTtbiC7fFfyReYjA7V2"; //UID of the admin stored in Firebase
    //
    //     await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, new Dictionary<string, object>
    //         {
    //             { "role", "admin" }
    //         });
    //
    //     return Ok("User is now an admin.");
    // }
    
    // [HttpPost("make-user")]
    // public async Task<IActionResult> MakeUser()
    // {
    //     string uid = "ggBglcg4vYPEMv6C7zR0eVxreuE2";
    //
    //     await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(
    //         uid,
    //         new Dictionary<string, object>
    //         {
    //             { "role", "user" }
    //         });
    //
    //     return Ok("User now has the user role.");
    // }
        
}

