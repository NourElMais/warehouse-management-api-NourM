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
        var products = _db.Products.Where(p=> p.Supplier.Name.ToLower() == supplierName.ToLower());
        if (order == "desc")
        {
            products = products.OrderByDescending(p => p.CreatedAt);
        }
        else
        {
            products = products.OrderBy(p => p.CreatedAt);
        }

        return Ok(products.Select(p => p.Name).ToList());
    }

    [HttpGet("groupByExpiryYear")]
    public IActionResult GroupByExpiryYear()
    {
        var grouped = _db.Products.GroupBy(p => p.Expirydate.Year)
            .Select(g => new
            {
                ExpiryYear = g.Key,
                Products = g.Select(p => p.Name).ToList()
            })
            .ToList();

        return Ok(grouped);
    }

    [HttpGet("groupByExpiryYearAndSupplierCountry")]
    public IActionResult GroupByExpiryYearAndSupplierCountry()
    {
        var grouped = _db.Products.GroupBy(p => new
            {
                p.Expirydate.Year,
                p.Supplier.Country
            }).Select(g => new
            {
                ExpiryYear = g.Key.Year,
                SupplierCountry = g.Key.Country,
                Products = g.Select(p => p.Name).ToList()
            })
            .ToList();

        return Ok(grouped);
    }
    
    [HttpGet("totalProducts")]
    public IActionResult TotalProducts()
    {
        return Ok(_db.Products.Count());
    }
    
    [HttpGet("byPageNbr")]
    public IActionResult GetProductsByPageNbr([FromQuery] int pageNbr, [FromQuery] int pageSize)
    {
        if (pageNbr < 1)
        {
            return BadRequest("Page Nbr must be greater than 1");
        }

        if (pageSize < 1)
        {
            return BadRequest("Page Size must be greater than 1");
        }
        int ProductsToSkip = (pageNbr-1)*pageSize;
        var products = _db.Products.Skip(ProductsToSkip).Take(pageSize);
        return Ok(products.ToList());
    }
}