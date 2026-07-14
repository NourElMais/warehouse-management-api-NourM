using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class DeactivateSupplierCommand : IRequest<SupplierViewModel>
{
    public string SupplierId { get; set; }

    public DeactivateSupplierCommand(string supplierId)
    {
        SupplierId = supplierId;
    }
}