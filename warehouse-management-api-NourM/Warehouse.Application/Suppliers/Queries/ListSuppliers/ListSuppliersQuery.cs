using MediatR;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class ListSuppliersQuery : IRequest<List<Supplier>>
{
}