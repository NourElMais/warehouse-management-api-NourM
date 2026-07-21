using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, SupplierViewModel>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    private  readonly ILogger<CreateSupplierHandler> _logger;
    public CreateSupplierHandler(ISupplierRepository supplierRepository, IMapper mapper, ILogger<CreateSupplierHandler> logger)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _logger = logger;
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
        _logger.LogInformation("Supplier {SupplierId} created successfully", supplier.Id);
        return _mapper.Map<SupplierViewModel>(supplier);
        
    }
}