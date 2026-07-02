using Microsoft.AspNetCore.Mvc;
using warehouse_management_api_NourM.Models;

namespace warehouse_management_api_NourM.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public ActionResult GetProducts([FromQuery] bool onlyAvailable = false)
    {
        //The assumption for this method is that it will return only active products (where IsArchived = false)
        var products = new List<Product>();
        foreach (Product p in FakeWarehouseStore.Products)
        {
            if (!p.IsArchived)
            {
                products.Add(p);
            }
        }
        
        //If the user specified that he wants only available products, we filter the products list
        //and keep only products that are in stock
        if (onlyAvailable)
        {
            var availableProducts  = new List<Product>();
            foreach (Product p in products)
            {
                if (p.QuantityInStock > 0)
                {
                    availableProducts.Add(p);
                }
            }
            products = availableProducts;
        }

        List<Product> toReturn = products
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        return Ok(toReturn);
    }
}
    
