namespace Warehouse.Application.Suppliers.Commands;

public class CreateSupplierCommand
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
}