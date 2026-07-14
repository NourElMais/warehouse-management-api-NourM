using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Commands;
using MediatR;
public class UpdateProductQuantityCommand: IRequest<ProductViewModel>
{
    public string ProductId { get; set; }
    public int NewQuantity { get; set; }

    public UpdateProductQuantityCommand(string productId, int newQuantity)
    {
        ProductId = productId;
        NewQuantity = newQuantity;
    }
}