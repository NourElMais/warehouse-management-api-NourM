using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;


namespace Warehouse.Application.Products.Commands;
using MediatR;
public class AssignSupplierToProductCommand : IRequest<ProductViewModel?>
{
    public string ProductId { get; set; }
    public string SupplierId { get; set; }

    public AssignSupplierToProductCommand(string productId, string supplierId)
    {
        ProductId = productId;
        SupplierId = supplierId;
    }
}