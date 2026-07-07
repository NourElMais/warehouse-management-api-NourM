namespace Warehouse.Application.Products.Commands;

public class UpdateProductQuantityCommand
{
    public string ProductId { get; set; }
    public int NewQuantity { get; set; }

    public UpdateProductQuantityCommand(string productId, int newQuantity)
    {
        ProductId = productId;
        NewQuantity = newQuantity;
    }
}