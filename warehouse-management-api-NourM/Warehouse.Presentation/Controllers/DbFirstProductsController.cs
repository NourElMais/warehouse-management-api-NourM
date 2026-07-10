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

    [HttpGet("groupByExpiryYear")]
    public IActionResult GroupByExpiryYear()
    {
        var grouped = _db.Products.GroupBy(p => p.Expirydate.Year)
            .Select(g => new
            {
                ExpiryYear = g.Key,
                Products = g.Select(p => p.Name)
            })
            .ToList();

        return Ok(grouped);
    }

    [HttpGet("groupByExpiryYear&SupplierCountry")]
    public IActionResult GroupByExpiryYearAndSupplierCountry()
    {
        var grouped = _db.Products.GroupBy(p => new
            {
                p.Expirydate.Year,
                p.Supplier.Country
            }).Select(g => new
            {
                ExpiryYearAndSupplierCountry = g.Key,
                Products = g.Select(p => p.Name)
            })
            .ToList();

        return Ok(grouped);
    }
    
}