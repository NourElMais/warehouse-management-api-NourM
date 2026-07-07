using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Commands;
using MediatR;
public class CreateProductCommand: IRequest<Product>
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public string SupplierName { get; set; }
    public DateTime ExpiryDate { get; set; }
}