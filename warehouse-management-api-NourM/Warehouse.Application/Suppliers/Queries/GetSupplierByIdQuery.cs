namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierByIdQuery
{
    public string Id { get; set; }

    public GetSupplierByIdQuery(string id)
    {
        Id = id;
    }
}