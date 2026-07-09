using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Commands;
using MediatR;
public class UpdateProductPriceCommand: IRequest<Product?>
{
    public string ProductId { get; set; }
    public decimal NewPrice { get; set; }

    public UpdateProductPriceCommand(string productId, decimal newPrice)
    {
        ProductId = productId;
        NewPrice = newPrice;
    }
}