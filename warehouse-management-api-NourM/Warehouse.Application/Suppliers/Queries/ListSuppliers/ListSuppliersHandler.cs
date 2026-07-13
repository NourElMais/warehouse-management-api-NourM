using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class ListSuppliersHandler
    : IRequestHandler<ListSuppliersQuery, List<SupplierViewModel>>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;

    public ListSuppliersHandler(ISupplierRepository supplierRepository, IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public async Task<List<SupplierViewModel>> Handle(
        ListSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        List<Supplier> suppliers = await _supplierRepository.GetAllAsync(cancellationToken);

        return _mapper.Map<List<SupplierViewModel>>(suppliers);
    }
}