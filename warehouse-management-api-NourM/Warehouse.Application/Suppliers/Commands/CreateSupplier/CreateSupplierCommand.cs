using MediatR;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class CreateSupplierCommand : IRequest<Supplier>
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
}