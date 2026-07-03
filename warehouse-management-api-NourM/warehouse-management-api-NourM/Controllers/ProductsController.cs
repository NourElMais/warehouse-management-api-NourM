using Microsoft.AspNetCore.Mvc;
using warehouse_management_api_NourM.Contracts;
using warehouse_management_api_NourM.Services;

namespace warehouse_management_api_NourM.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    //1. Get all products
    [HttpGet]
    public ActionResult GetProducts([FromQuery] bool onlyAvailable = false)
    {
        return Ok(_productService.GetProducts(onlyAvailable));
    }

    //2. Get product by id
    [HttpGet("{id}")]
    public ActionResult GetProductById([FromRoute] string id)
    {
        //If the id is a valid GUID, we search the products list, else we return status code 400 (BadRequest) because the Id is not a valid GUID
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The entered Id is not a valid GUID");
        }

        var product = _productService.GetProductById(id);

        if (product == null)
        {
            return NotFound("There is no product with the specified id");
        }

        return Ok(product);
    }

    //3. Search products 
    [HttpGet("search")]
    //The query parameters can be null, in case the user wants to provide one parameter only
    public ActionResult GetProductsBySearch([FromQuery] string? name, [FromQuery] string? supplier)
    {
        //If both parameters are empty, we return BadRequest
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(supplier))
        {
            return BadRequest("Please provide at least the item name or the supplier name.");
        }

        return Ok(_productService.SearchProducts(name, supplier));
    }

    //4. Create product
    [HttpPost]
    public ActionResult CreateProduct([FromBody] CreateProductRequest p)
    {
        string result = _productService.CreateProduct(p);

        if (result == "duplicate sku")
        {
            return BadRequest("A product with this SKU already exists.");
        }

        return Ok("New product is added");
    }

    //5. Update quantity
    [HttpPost("{id}/quantity")]
    public ActionResult UpdateQuantity([FromRoute] string id, [FromBody] UpdateProductQuantityRequest req)
    {
        //Check if the Id provided is a valid GUID
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The entered Id is not valid");
        }

        string result = _productService.UpdateQuantity(id, req);

        if (result == "not found")
        {
            return NotFound("There is no product with the specified id");
        }

        return Ok("Quantity in Stock Updated");
    }

    //6. Update Price
    [HttpPost("{id}/price")]
    public ActionResult UpdatePrice([FromRoute] string id, [FromBody] UpdateProductPriceRequest req)
    {
        //Check if the Id provided is a valid GUID
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The entered Id is not valid");
        }

        string result = _productService.UpdatePrice(id, req);

        if (result == "not found")
        {
            return NotFound("There is no product with the specified id");
        }

        return Ok("Price Updated");
    }

    //7. Upload Image
    [HttpPost("{id}/image")]
    public async Task<ActionResult> UploadImage([FromRoute] string id, IFormFile image)
    {
        // Check if the Id provided is a valid GUID
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The entered Id is not valid");
        }

        string result = await _productService.UploadImage(id, image);

        if (result == "empty image")
        {
            return BadRequest("Please upload an image.");
        }

        if (result == "invalid extension")
        {
            return BadRequest("Only JPG and PNG images are allowed.");
        }

        if (result == "too large")
        {
            return BadRequest("Image size cannot exceed 2 MB.");
        }

        if (result == "not found")
        {
            return NotFound("There is no product with the specified id.");
        }

        return Ok("Image uploaded successfully.");
    }

    //8. Delete product 
    [HttpDelete("{id}")]
    public ActionResult DeleteProduct([FromRoute] string id)
    {
        // Check if the Id provided is a valid GUID
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The entered Id is not valid");
        }

        string result = _productService.DeleteProduct(id);

        if (result == "already archived")
        {
            return Ok("Product is already deleted (archived)");
        }

        if (result == "not found")
        {
            return NotFound("There is no product with the specified id.");
        }

        return Ok("Product Deleted (Archived)");
    }

    //9. Get warehouse server time
    [HttpGet("server-time")]
    public ActionResult GetServerTime([FromHeader(Name = "Accept-Language")] string language)
    {
        if (language != "en-US" &&
            language != "fr-FR" &&
            language != "ar-LB")
        {
            return BadRequest("The specified language is not supported");
        }

        return Ok(_productService.GetServerTime(language));
    }

    // Task 2: Assigning supplier to product
    [HttpPost("{id}/assign-supplier/{supplierId}")]
    // This method assigns the specified supplier to the specified product after validating that both exist and that the product is not archived.
    public ActionResult AssignSupplierToProduct(
        [FromRoute] string id,
        [FromRoute] string supplierId)
    {
        //First, we check if both ids are valid GUIDs
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The product Id is not valid.");
        }

        if (!Guid.TryParse(supplierId, out var g))
        {
            return BadRequest("The supplier Id is not valid.");
        }

        string result = _productService.AssignSupplierToProduct(id, supplierId);

        if (result == "product not found")
        {
            return NotFound("Product not found.");
        }

        if (result == "archived product")
        {
            return BadRequest("Archived products cannot be assigned to a supplier.");
        }

        if (result == "supplier not found")
        {
            return NotFound("Supplier not found.");
        }

        return Ok(result);
    }

    // Extra endpoints (Not mentioned in the lab)

    // 1- Restore archived product
    [HttpPost("{id}/restore")]
    public ActionResult RestoreProduct([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The entered Id is not valid");
        }

        string result = _productService.RestoreProduct(id);

        if (result == "already active")
        {
            return Ok("Product is already active");
        }

        if (result == "not found")
        {
            return NotFound("There is no product with the specified id");
        }

        return Ok("Product restored successfully");
    }

    //2- Get low stock products
    // Returns all active products that are running low on stock based on the specified threshold (default value for the threshold is 5)
    [HttpGet("low-stock")]
    public ActionResult GetLowStockProducts([FromQuery] int threshold = 5)
    {
        return Ok(_productService.GetLowStockProducts(threshold));
    }

    // 3- Get general warehouse statistics about products.
    [HttpGet("statistics")]
    public ActionResult GetProductStatistics()
    {
        return Ok(_productService.GetProductStatistics());
    }
}