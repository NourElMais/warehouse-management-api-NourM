using System.ComponentModel.DataAnnotations;

namespace warehouse_management_api_NourM.Contracts;

public class UpdateProductPriceRequest
{
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price {get; set;}
}