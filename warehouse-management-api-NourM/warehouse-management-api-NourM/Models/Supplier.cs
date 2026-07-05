using System.ComponentModel.DataAnnotations;

namespace warehouse_management_api_NourM.Models;

public class Supplier
{
    [Required]
    public string Id { get; set; }
    
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
    
    [Required]
    public bool IsActive { get; set; }
}