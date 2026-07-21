using MediatR;
using Warehouse.Domain.ProductImages;

namespace Warehouse.Application.Products.Queries.GetProductImageQuery;

public class GetProductImageQuery : IRequest<ProductImage?>
    {
        public string ProductId { get; }

        public GetProductImageQuery(string productId)
        {
            ProductId = productId;
        }
}