using Warehouse.Application.ViewModels;
namespace Warehouse.Application.Products.Commands;
using MediatR;
public class UpdateProductPriceCommand: IRequest<ProductViewModel>
{
    public string ProductId { get; set; }
    public decimal NewPrice { get; set; }

    public UpdateProductPriceCommand(string productId, decimal newPrice)
    {
        ProductId = productId;
        NewPrice = newPrice;
    }
}