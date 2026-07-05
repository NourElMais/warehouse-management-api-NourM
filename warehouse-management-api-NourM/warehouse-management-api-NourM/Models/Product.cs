using System.ComponentModel.DataAnnotations;

namespace warehouse_management_api_NourM.Models;

public class Product
{
    [Required]
    public string Id { get; set; }
    
    //I will consider that Name is required, and the max length is 50
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    //I will consider that SKU is required, and the max length is 30
    [Required]
    [StringLength(30)]
    public string SKU { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; }
    
    //To prevent someone from entering an invalid price (negative price for example), i will add a range constraint, and make it required also
    [Required]
    [Range(0,double.MaxValue)]
    public decimal Price { get; set; }
    
    //To prevent someone from entering a negative quantity which is invalid
    [Required]
    [Range(0, int.MaxValue)]
    public int QuantityInStock { get; set; }
    
    [Required]
    [StringLength(50)]
    public string SupplierName { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}