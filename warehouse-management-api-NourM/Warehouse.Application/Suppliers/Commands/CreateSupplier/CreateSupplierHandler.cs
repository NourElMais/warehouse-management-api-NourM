using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, SupplierViewModel>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    public CreateSupplierHandler(ISupplierRepository supplierRepository, IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public Task<SupplierViewModel> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = new Supplier(
            request.Name,
            request.Country,
            request.ContactEmail,
            request.PhoneNumber
        );

        _supplierRepository.Add(supplier);

        var viewModel = _mapper.Map<SupplierViewModel>(supplier);

        return Task.FromResult(viewModel);
    }
}