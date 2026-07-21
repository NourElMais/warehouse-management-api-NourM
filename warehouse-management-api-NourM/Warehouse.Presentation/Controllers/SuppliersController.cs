using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Warehouse.Application.Suppliers.Commands;
using Warehouse.Application.Suppliers.Queries;
using Warehouse.Presentation.Contracts;
using Warehouse.Presentation.Resources;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/suppliers")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;
    public SuppliersController(IMediator mediator, IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
        _mediator = mediator;
    }

    // 1. Get all suppliers
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet]
    public async Task<ActionResult> GetAllSuppliers(CancellationToken cancellationToken)
    {
        var suppliers = await _mediator.Send(new ListSuppliersQuery(),cancellationToken);

        return Ok(suppliers);
    }

    // 2. Get supplier by Id
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetSupplierById([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest(SharedResources.InvalidID);
        }

        var supplier = await _mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);

        return Ok(supplier);
    }

    // 3. Create supplier
    [Authorize(Policy = "Admin")]
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
    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSupplier([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest(SharedResources.InvalidID);
        }

        var supplier = await _mediator.Send(new DeactivateSupplierCommand(id), cancellationToken);

        return Ok(SharedResources.SupplierDeleted);
    }

    // 5. Supplier statistics
    [Authorize(Policy = "Admin")]
    [HttpGet("statistics")]
    public async Task<ActionResult> GetSupplierStatistics(CancellationToken cancellationToken)
    {
        var statistics = await _mediator.Send(new GetSupplierStatisticsQuery(), cancellationToken);

        return Ok(statistics);
    }
}