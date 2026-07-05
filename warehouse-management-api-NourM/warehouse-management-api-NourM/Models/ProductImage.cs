using System.ComponentModel.DataAnnotations;

namespace warehouse_management_api_NourM.Models;

public class ProductImage
{
    [Required]
    public string ProductId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string FileName { get; set; }
    
    [Required]
    [StringLength(255)]
    public string FilePath { get; set; }
}