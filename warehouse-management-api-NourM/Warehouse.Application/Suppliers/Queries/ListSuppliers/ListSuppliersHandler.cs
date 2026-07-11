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
    private IMapper _mapper;

    public ListSuppliersHandler(ISupplierRepository supplierRepository, IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public Task<List<SupplierViewModel>> Handle(
        ListSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        List<Supplier> suppliers = _supplierRepository.GetAll();

        var viewModel = _mapper.Map<List<SupplierViewModel>>(suppliers);

        return Task.FromResult(viewModel);
    }
}