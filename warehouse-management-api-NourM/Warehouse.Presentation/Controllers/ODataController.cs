using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Warehouse.Domain.Products;
using Warehouse.Domain.Suppliers;
using Warehouse.Infrastructure;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("odata")]
public class ODataController : ControllerBase
{
    private readonly WarehouseDbContext _db;
    public ODataController(WarehouseDbContext db)
    {
        _db = db;
    }
    
    [HttpGet("products")]
    [EnableQuery]
    public IQueryable<Product> GetProd()
    {
        return _db.products;
    }
    
    [HttpGet("suppliers")]
    [EnableQuery]
    public IQueryable<Supplier> GetSupp()
    {
        return _db.suppliers;
    }
    
}
