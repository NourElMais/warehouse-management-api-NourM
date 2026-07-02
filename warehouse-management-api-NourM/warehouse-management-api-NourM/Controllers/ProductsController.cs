using Microsoft.AspNetCore.Mvc;
using warehouse_management_api_NourM.Models;

namespace warehouse_management_api_NourM.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetProducts()
    {
        var store = new FakeWarehouseStore();
        return Ok(store.products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(string id)
    {
        var store = new FakeWarehouseStore();
        List<Product> products = store.products;
        foreach (Product p in products)
        {
            if (p.Id == id)
            {
                return Ok(p);
            }
        }
        return NotFound("No product found with this Id");
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct([FromBody] Product product)
    {
        
    }
    
}
    
