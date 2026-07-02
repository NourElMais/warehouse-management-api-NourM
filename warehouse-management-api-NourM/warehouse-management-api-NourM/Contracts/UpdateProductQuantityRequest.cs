using System.ComponentModel.DataAnnotations;

namespace warehouse_management_api_NourM.Contracts;

public class UpdateProductQuantityRequest
{
    [Required]
    [Range(0,int.MaxValue)]
    public int QuantityInStock { get; set; }
}