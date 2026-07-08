namespace Warehouse.Domain.DomainEvents;

public class ProductArchivedEvent
{
    public string ProductId { get; }
    public DateTime ArchivedAt { get; }

    public ProductArchivedEvent(string productId)
    {
        ProductId = productId;
        ArchivedAt = DateTime.UtcNow;
    }
}