using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.Suppliers.Commands;
using Warehouse.Application.Suppliers.Queries;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // 1. Get all suppliers
    [HttpGet]
    public async Task<ActionResult> GetAllSuppliers(CancellationToken cancellationToken)
    {
        var suppliers = await _mediator.Send(new ListSuppliersQuery(),cancellationToken);

        return Ok(suppliers);
    }

    // 2. Get supplier by Id
    [HttpGet("{id}")]
    public async Task<ActionResult> GetSupplierById([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The provided Id is not valid");
        }

        var supplier = await _mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);

        return Ok(supplier);
    }

    // 3. Create supplier
    [HttpPost]
    public async Task<ActionResult> CreateSupplier([FromBody] CreateSupplierRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSupplierCommand
        {
            Name = request.Name,
            Country = request.Country,
            ContactEmail = request.ContactEmail,
            PhoneNumber = request.PhoneNumber
        };

        var supplier = await _mediator.Send(command, cancellationToken);

        return Ok(supplier);
    }

    // 4. Deactivate supplier
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSupplier([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The provided Id is not valid");
        }

        var supplier = await _mediator.Send(new DeactivateSupplierCommand(id), cancellationToken);

        return Ok("Supplier Deleted (Deactivated)");
    }

    // 5. Supplier statistics
    [HttpGet("statistics")]
    public async Task<ActionResult> GetSupplierStatistics(CancellationToken cancellationToken)
    {
        var statistics = await _mediator.Send(new GetSupplierStatisticsQuery(), cancellationToken);

        return Ok(statistics);
    }
}