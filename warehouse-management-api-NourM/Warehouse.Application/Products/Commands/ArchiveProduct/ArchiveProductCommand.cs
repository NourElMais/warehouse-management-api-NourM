using MediatR;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Commands;

public class ArchiveProductCommand : IRequest<Product?>
{
    public string ProductId { get; set; }

    public ArchiveProductCommand(string productId)
    {
        ProductId = productId;
    }
}