using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.Products.Commands;
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

    [HttpGet]
    public async Task<ActionResult> GetProducts([FromQuery] bool onlyAvailable = false)
    {
        var products = await _mediator.Send(new ListProductsQuery(onlyAvailable));
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProductById([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out _))
            return BadRequest("The entered Id is not a valid GUID");

        var product = await _mediator.Send(new GetProductByIdQuery(id));

        if (product is null)
            return NotFound("There is no product with the specified id");

        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<ActionResult> GetProductsBySearch([FromQuery] string? name, [FromQuery] string? supplier)
    {
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(supplier))
            return BadRequest("Please provide at least the item name or the supplier name.");

        var products = await _mediator.Send(new SearchProductsQuery(name, supplier));
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        var command = new CreateProductCommand
        {
            Name = request.Name,
            SKU = request.SKU,
            Description = request.Description,
            Price = request.Price,
            QuantityInStock = request.QuantityInStock,
            SupplierName = request.SupplierName,
            ExpiryDate = request.ExpiryDate
        };

        var product = await _mediator.Send(command);

        return Ok(product);
    }

    [HttpPost("{id}/quantity")]
    public async Task<ActionResult> UpdateQuantity([FromRoute] string id, [FromBody] UpdateProductQuantityRequest request)
    {
        if (!Guid.TryParse(id, out _))
            return BadRequest("The entered Id is not valid");

        var product = await _mediator.Send(
            new UpdateProductQuantityCommand(id, request.QuantityInStock));

        if (product is null)
            return NotFound("There is no product with the specified id");

        return Ok(product);
    }

    [HttpPost("{id}/price")]
    public async Task<ActionResult> UpdatePrice([FromRoute] string id, [FromBody] UpdateProductPriceRequest request)
    {
        if (!Guid.TryParse(id, out _))
            return BadRequest("The entered Id is not valid");

        var product = await _mediator.Send(
            new UpdateProductPriceCommand(id, request.Price));

        if (product is null)
            return NotFound("There is no product with the specified id");

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out _))
            return BadRequest("The entered Id is not valid");

        var product = await _mediator.Send(new ArchiveProductCommand(id));

        if (product is null)
            return NotFound("There is no product with the specified id.");

        return Ok("Product Deleted (Archived)");
    }

    [HttpPost("{id}/assign-supplier/{supplierId}")]
    public async Task<ActionResult> AssignSupplierToProduct(
        [FromRoute] string id,
        [FromRoute] string supplierId)
    {
        if (!Guid.TryParse(id, out _))
            return BadRequest("The product Id is not valid.");

        if (!Guid.TryParse(supplierId, out _))
            return BadRequest("The supplier Id is not valid.");

        var product = await _mediator.Send(
            new AssignSupplierToProductCommand(id, supplierId));

        if (product is null)
            return NotFound("Product or supplier not found.");

        return Ok(product);
    }

    [HttpPost("{id}/restore")]
    public async Task<ActionResult> RestoreProduct([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out _))
            return BadRequest("The entered Id is not valid");

        var product = await _mediator.Send(new RestoreProductCommand(id));

        if (product is null)
            return NotFound("There is no product with the specified id");

        return Ok(product);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult> GetLowStockProducts([FromQuery] int threshold = 5)
    {
        var products = await _mediator.Send(new GetLowStockProductsQuery(threshold));
        return Ok(products);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult> GetProductStatistics()
    {
        var statistics = await _mediator.Send(new GetProductsStatisticsQuery());
        return Ok(statistics);
    }
    
    [HttpGet("server-time")]
    public async Task<ActionResult> GetServerTime([FromHeader(Name = "Accept-Language")] string language)
    {
        if (language != "en-US" && language != "fr-FR" && language != "ar-LB")
            return BadRequest("The specified language is not supported");

        var result = await _mediator.Send(new GetServerTimeQuery(language));
        return Ok(result);
    }
    
    [HttpPost("{id}/image")]
    public async Task<ActionResult> UploadImage([FromRoute] string id, IFormFile image)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest("The entered Id is not valid");

        if (image is null)
            return BadRequest("Please upload an image.");

        var result = await _mediator.Send(
            new UploadProductImageCommand(id, image.FileName, image.Length));

        if (result == "empty image")
            return BadRequest("Please upload an image.");

        if (result == "invalid extension")
            return BadRequest("Only JPG and PNG images are allowed.");

        if (result == "too large")
            return BadRequest("Image size cannot exceed 2 MB.");

        if (result == "not found")
            return NotFound("There is no product with the specified id.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, image.FileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);

        return Ok("Image uploaded successfully.");
    }
}