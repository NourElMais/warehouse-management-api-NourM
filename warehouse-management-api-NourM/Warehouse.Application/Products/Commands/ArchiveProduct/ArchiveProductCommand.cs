using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Commands;

public class ArchiveProductCommand : IRequest<ProductViewModel?>
{
    public string ProductId { get; set; }

    public ArchiveProductCommand(string productId)
    {
        ProductId = productId;
    }
}