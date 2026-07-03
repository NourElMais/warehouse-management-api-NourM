using Microsoft.AspNetCore.Mvc;
using warehouse_management_api_NourM.Contracts;
using warehouse_management_api_NourM.Models;

namespace warehouse_management_api_NourM.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController:ControllerBase
{
    [HttpGet]
    public ActionResult GetAllSuppliers()
    {
        List<Supplier> activeSuppliers = new List<Supplier>();
        foreach (Supplier s in FakeSupplierStore.Suppliers)
        {
            if (s.IsActive)
            {
                activeSuppliers.Add(s);
            }
        }
        return Ok(activeSuppliers);
    }

    [HttpGet("{id}")]
    public ActionResult GetSupplierById(string id)
    {
        //First, we check if the Id is a valid GUID
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The provided Id is not valid");
        }

        foreach (Supplier s in FakeSupplierStore.Suppliers)
        {
            if (s.Id == id)
            {
                return Ok(s);
            }
        }
        return NotFound("There is no supplier found with the specified Id");
    }

    [HttpPost]
    public ActionResult CreateSupplier([FromBody] CreateSupplierRequest supplier)
    {
        Supplier s = new Supplier
        {
            Id = Guid.NewGuid().ToString(),
            Name = supplier.Name,
            Country = supplier.Country,
            ContactEmail = supplier.ContactEmail,
            PhoneNumber = supplier.PhoneNumber,
            IsActive = true
        };
        FakeSupplierStore.Suppliers.Add(s);
        return Ok("Supplier Created");
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteSupplier(string id)
    {
        //First, we check if the Id is a valid GUID
        if (!Guid.TryParse(id, out var guid))
        {
            return BadRequest("The provided Id is not valid");
        }

        foreach (Supplier s in FakeSupplierStore.Suppliers)
        {
            if (s.Id == id)
            {
                if (!s.IsActive)
                {
                    return Ok("Supplier is already deactivated");
                }
                s.IsActive = false;
                return Ok("Supplier Deleted (Deactivated)");
            }
        }

        return NotFound("There is no supplier found with the specified Id");
    }
}