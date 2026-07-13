using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class ListSuppliersQuery : IRequest<List<SupplierViewModel>>
{
}