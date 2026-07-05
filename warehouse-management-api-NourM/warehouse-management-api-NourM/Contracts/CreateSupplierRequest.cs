using System.ComponentModel.DataAnnotations;

namespace warehouse_management_api_NourM.Contracts;

//This is the only DTO I will create for Supplier, because there is no Update endpoint required
public class CreateSupplierRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Country { get; set; }
    
    [Required]
    [EmailAddress]
    public string ContactEmail { get; set; }
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}