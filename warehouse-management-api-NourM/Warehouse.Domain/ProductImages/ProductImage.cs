namespace Warehouse.Domain.ProductImages;

public class ProductImage
{
    public string ProductId { get; private set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }

    public ProductImage(
        string productId,
        string fileName,
        string filePath)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product Id is required.");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required.");

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path is required.");

        ProductId = productId;
        FileName = fileName;
        FilePath = filePath;
    }
}