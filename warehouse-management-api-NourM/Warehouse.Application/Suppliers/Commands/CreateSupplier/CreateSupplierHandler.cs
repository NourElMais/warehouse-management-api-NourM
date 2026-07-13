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

    public async Task<SupplierViewModel> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = new Supplier(
            request.Name,
            request.Country,
            request.ContactEmail,
            request.PhoneNumber
        );

        await _supplierRepository.AddAsync(supplier, cancellationToken);

        return _mapper.Map<SupplierViewModel>(supplier);
        
    }
}