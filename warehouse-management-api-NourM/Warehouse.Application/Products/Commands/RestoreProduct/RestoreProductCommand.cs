using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Commands;
using MediatR;
public class RestoreProductCommand: IRequest<ProductViewModel?>
{
    public string ProductId { get; set; }

    public RestoreProductCommand(string productId)
    {
        ProductId = productId;
    }
}