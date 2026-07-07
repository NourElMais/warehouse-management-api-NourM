namespace Warehouse.Application.Suppliers.Commands;

public class DeactivateSupplierCommand
{
    public string SupplierId { get; set; }

    public DeactivateSupplierCommand(string supplierId)
    {
        SupplierId = supplierId;
    }
}