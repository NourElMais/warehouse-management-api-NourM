using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.Products.Commands;
using Warehouse.Application.Products.GetProductsStatistics;
using Warehouse.Application.Products.Queries;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
// ASP.NET creates a CancellationToken for each HTTP request,
// so we pass it through all application layers (Controller → MediatR → Handler → Repository → EF Core)
// so that if the client cancels the request (for example if he closes the browser),
// every layer is notified and can stop its work instead of wasting resources.

    [HttpGet]
    public async Task<ActionResult> GetProducts(CancellationToken cancellationToken, [FromQuery] bool onlyAvailable = false)
    {
        var products = await _mediator.Send(new ListProductsQuery(onlyAvailable), cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProductById([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest("The entered Id is not a valid GUID");

        var product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<ActionResult> GetProductsBySearch([FromQuery] string? name, [FromQuery] string? supplier, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(supplier))
            return BadRequest("Please provide at least the item name or the supplier name.");

        var products = await _mediator.Send(new SearchProductsQuery(name, supplier), cancellationToken);
        return Ok(products);
    }

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

    [HttpPost("{id}/quantity")]
    public async Task<ActionResult> UpdateQuantity([FromRoute] string id, [FromBody] UpdateProductQuantityRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest("The entered Id is not valid");

        var product = await _mediator.Send(new UpdateProductQuantityCommand(id, request.QuantityInStock), cancellationToken);
        
        return Ok(product);
    }

    [HttpPost("{id}/price")]
    public async Task<ActionResult> UpdatePrice([FromRoute] string id, [FromBody] UpdateProductPriceRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest("The entered Id is not valid");

        var product = await _mediator.Send(new UpdateProductPriceCommand(id, request.Price), cancellationToken);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest("The entered Id is not valid");

        var product = await _mediator.Send(new ArchiveProductCommand(id), cancellationToken);
        return Ok("Product Deleted (Archived)");
    }

    [HttpPost("{id}/assign-supplier/{supplierId}")]
    public async Task<ActionResult> AssignSupplierToProduct([FromRoute] string id, [FromRoute] string supplierId, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest("The product Id is not valid.");

        if (!Guid.TryParse(supplierId, out var g))
            return BadRequest("The supplier Id is not valid.");

        var product = await _mediator.Send(new AssignSupplierToProductCommand(id, supplierId), cancellationToken);

        return Ok(product);
    }

    [HttpPost("{id}/restore")]
    public async Task<ActionResult> RestoreProduct([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest("The entered Id is not valid");

        var product = await _mediator.Send(new RestoreProductCommand(id), cancellationToken);

        return Ok(product);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult> GetLowStockProducts(CancellationToken cancellationToken, [FromQuery] int threshold = 5)
    {
        var products = await _mediator.Send(new GetLowStockProductsQuery(threshold), cancellationToken);
        return Ok(products);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult> GetProductStatistics(CancellationToken cancellationToken)
    {
        var statistics = await _mediator.Send(new GetProductsStatisticsQuery(), cancellationToken);
        return Ok(statistics);
    }

    [HttpGet("server-time")]
    public async Task<ActionResult> GetServerTime(CancellationToken cancellationToken, [FromHeader(Name = "Accept-Language")] string language)
    {
        if (language != "en-US" && language != "fr-FR" && language != "ar-LB")
            return BadRequest("The specified language is not supported");

        var result = await _mediator.Send(new GetServerTimeQuery(language), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id}/image")]
    public async Task<ActionResult> UploadImage([FromRoute] string id, IFormFile image, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest("The entered Id is not valid");

        if (image is null)
            return BadRequest("Please upload an image.");

        var result = await _mediator.Send(new UploadProductImageCommand(id, image.FileName, image.Length), cancellationToken);

        if (result == UploadProductImageResult.EmptyImage)
            return BadRequest("Please upload an image.");

        if (result == UploadProductImageResult.InvalidExtension)
            return BadRequest("Only JPG and PNG images are allowed.");

        if (result == UploadProductImageResult.FileTooLarge)
            return BadRequest("Image size cannot exceed 2 MB.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, image.FileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream, cancellationToken);

        return Ok("Image uploaded successfully.");
    }
}
