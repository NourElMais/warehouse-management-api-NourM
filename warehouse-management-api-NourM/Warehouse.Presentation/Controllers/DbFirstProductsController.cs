using Microsoft.AspNetCore.Mvc;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("/dbfirst")]
public class DbFirstProductsController:ControllerBase
{
    private readonly WarehouseDbFirstContext _db;

    public DbFirstProductsController(WarehouseDbFirstContext context)
    {
        _db = context;
    }
    
    [HttpGet("bySuppName")]
    public IActionResult GetProductsBySupplierName([FromQuery] string supplierName,[FromQuery] string order = "desc")
    {
        List<Product> products = _db.Products.Where(p=> p.Supplier.Name == supplierName).ToList();
        if (order == "desc")
        {
            products = products.OrderByDescending(p => p.CreatedAt).ToList();
        }
        else
        {
            products = products.OrderBy(p => p.CreatedAt).ToList();
        }

        return Ok(products.ToList());
    }
    
}