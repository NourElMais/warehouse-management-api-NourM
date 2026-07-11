using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class CreateSupplierCommand : IRequest<SupplierViewModel>
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
}