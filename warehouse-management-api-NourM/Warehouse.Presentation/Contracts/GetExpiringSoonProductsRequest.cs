using System.ComponentModel.DataAnnotations;

namespace Warehouse.Presentation.Contracts;

public class GetExpiringSoonProductsRequest
{
    [Range(1, 365)]
    public int DaysAhead { get; set; } = 30;
}