using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierByIdHandler
    : IRequestHandler<GetSupplierByIdQuery, SupplierViewModel?>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    public GetSupplierByIdHandler(ISupplierRepository supplierRepository, IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public Task<SupplierViewModel?> Handle(
        GetSupplierByIdQuery request,
        CancellationToken cancellationToken)
    {
        Supplier? supplier = _supplierRepository.GetById(request.Id);

        var viewModel = _mapper.Map<SupplierViewModel>(supplier);

        return Task.FromResult(viewModel);
    }
}