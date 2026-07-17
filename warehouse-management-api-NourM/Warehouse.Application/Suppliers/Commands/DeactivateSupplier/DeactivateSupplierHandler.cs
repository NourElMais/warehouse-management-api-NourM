using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Exceptions;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Suppliers.Commands;

public class DeactivateSupplierHandler 
    : IRequestHandler<DeactivateSupplierCommand, SupplierViewModel>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    private  readonly ILogger<DeactivateSupplierHandler> _logger;
    public DeactivateSupplierHandler(ISupplierRepository supplierRepository, IMapper mapper, ILogger<DeactivateSupplierHandler> logger)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SupplierViewModel> Handle(DeactivateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken);

        if (supplier is null)
            throw new NotFoundException("SupplierNotFound");

        supplier.Deactivate();
        await _supplierRepository.UpdateAsync(supplier, cancellationToken);
        _logger.LogInformation("Supplier {SupplierID} was deactivated", supplier.Id);

        return _mapper.Map<SupplierViewModel>(supplier);
        
    }
}