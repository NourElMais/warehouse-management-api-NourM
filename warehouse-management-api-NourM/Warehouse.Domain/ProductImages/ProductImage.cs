using Warehouse.Domain.Products;

namespace Warehouse.Domain.ProductImages;

public class ProductImage
{
    public string Id { get; private set; }

    public string ProductId { get; private set; }

    public string FileName { get; private set; }

    public string FilePath { get; private set; }

    public virtual Product? Product { get; private set; }

    private ProductImage()
    {
    }

    public ProductImage(
        string productId,
        string fileName,
        string filePath,
        string? id = null)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product Id is required.");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required.");

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path is required.");

        Id = id ?? Guid.NewGuid().ToString();
        ProductId = productId;
        FileName = fileName;
        FilePath = filePath;
    }
}