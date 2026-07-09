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
    public async Task<ActionResult> GetAllSuppliers()
    {
        var suppliers = await _mediator.Send(new ListSuppliersQuery());

        return Ok(suppliers);
    }

    // 2. Get supplier by Id
    [HttpGet("{id}")]
    public async Task<ActionResult> GetSupplierById([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out _))
        {
            return BadRequest("The provided Id is not valid");
        }

        var supplier = await _mediator.Send(new GetSupplierByIdQuery(id));

        if (supplier is null)
        {
            return NotFound("There is no supplier found with the specified Id");
        }

        return Ok(supplier);
    }

    // 3. Create supplier
    [HttpPost]
    public async Task<ActionResult> CreateSupplier([FromBody] CreateSupplierRequest request)
    {
        var command = new CreateSupplierCommand
        {
            Name = request.Name,
            Country = request.Country,
            ContactEmail = request.ContactEmail,
            PhoneNumber = request.PhoneNumber
        };

        var supplier = await _mediator.Send(command);

        return Ok(supplier);
    }

    // 4. Deactivate supplier
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSupplier([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out _))
        {
            return BadRequest("The provided Id is not valid");
        }

        var supplier = await _mediator.Send(new DeactivateSupplierCommand(id));

        if (supplier is null)
        {
            return NotFound("There is no supplier found with the specified Id");
        }

        return Ok("Supplier Deleted (Deactivated)");
    }

    // 5. Supplier statistics
    [HttpGet("statistics")]
    public async Task<ActionResult> GetSupplierStatistics()
    {
        var statistics = await _mediator.Send(new GetSupplierStatisticsQuery());

        return Ok(statistics);
    }
}