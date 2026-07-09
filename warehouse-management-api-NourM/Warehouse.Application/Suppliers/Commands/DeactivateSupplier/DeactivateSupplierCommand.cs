using MediatR;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class DeactivateSupplierCommand : IRequest<Supplier?>
{
    public string SupplierId { get; set; }

    public DeactivateSupplierCommand(string supplierId)
    {
        SupplierId = supplierId;
    }
}