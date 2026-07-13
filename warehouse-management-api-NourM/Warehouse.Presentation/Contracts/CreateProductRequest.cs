using System.ComponentModel.DataAnnotations;

namespace Warehouse.Presentation.Contracts;

public class CreateProductRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(30)]
    public string SKU { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int QuantityInStock { get; set; }

    [Required]
    [StringLength(500)]
    public string SupplierId { get; set; }

    public DateTime ExpiryDate { get; set; }
}