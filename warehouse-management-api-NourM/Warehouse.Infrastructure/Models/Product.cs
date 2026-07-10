using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Models;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Sku { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int QuantityInStock { get; set; }

    public DateTime Expirydate { get; set; }

    public bool Isarchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public string SupplierId { get; set; } = null!;

    public virtual ICollection<Productimage> Productimages { get; set; } = new List<Productimage>();

    public virtual Supplier Supplier { get; set; } = null!;
}
