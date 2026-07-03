using Microsoft.AspNetCore.Mvc;
using warehouse_management_api_NourM.Contracts;
using warehouse_management_api_NourM.Models;
using warehouse_management_api_NourM.Services;

namespace warehouse_management_api_NourM.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly SupplierService _supplierService;
    public SuppliersController(SupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public ActionResult GetAllSuppliers()
    {
        return Ok(_supplierService.GetAllActiveSuppliers());
    }

    [HttpGet("{id}")]
    public ActionResult GetSupplierById([FromRoute] string id)
    {
        //First, we check if the Id is a valid GUID
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The provided Id is not valid");
        }

        Supplier? supplier = _supplierService.GetSupplierById(id);

        if (supplier == null)
        {
            return NotFound("There is no supplier found with the specified Id");
        }

        return Ok(supplier);
    }

    [HttpPost]
    public ActionResult CreateSupplier([FromBody] CreateSupplierRequest supplier)
    {
        _supplierService.CreateSupplier(supplier);
        return Ok("Supplier Created");
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteSupplier([FromRoute] string id)
    {
        //First, we check if the Id is a valid GUID
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The provided Id is not valid");
        }

        string result = _supplierService.DeactivateSupplier(id);

        if (result == "not found")
        {
            return NotFound("There is no supplier found with the specified Id");
        }

        if (result == "already deactivated")
        {
            return Ok("Supplier is already deactivated");
        }

        return Ok("Supplier Deleted (Deactivated)");
    }

    // Extra endpoint to get supplier statistics
    [HttpGet("statistics")]
    public ActionResult GetSupplierStatistics()
    {
        return Ok(_supplierService.GetSupplierStatistics());
    }
}