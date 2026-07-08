using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Commands;
using MediatR;
public class RestoreProductCommand: IRequest<Product?>
{
    public string ProductId { get; set; }

    public RestoreProductCommand(string productId)
    {
        ProductId = productId;
    }
}