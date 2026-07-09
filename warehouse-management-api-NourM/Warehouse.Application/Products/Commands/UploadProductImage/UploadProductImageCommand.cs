using MediatR;

namespace Warehouse.Application.Products.Commands;

public class UploadProductImageCommand : IRequest<UploadProductImageResult>
{
    public string ProductId { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }

    public UploadProductImageCommand(string productId, string fileName, long fileSize)
    {
        ProductId = productId;
        FileName = fileName;
        FileSize = fileSize;
    }
}