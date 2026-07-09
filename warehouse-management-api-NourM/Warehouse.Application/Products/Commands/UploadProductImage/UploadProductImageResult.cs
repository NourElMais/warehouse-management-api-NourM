namespace Warehouse.Application.Products.Commands;

    public enum UploadProductImageResult
    {
        Success,
        ProductNotFound,
        EmptyImage,
        InvalidExtension,
        FileTooLarge
    }
