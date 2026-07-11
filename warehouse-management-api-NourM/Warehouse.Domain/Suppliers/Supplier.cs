using System.ComponentModel.DataAnnotations;
using Warehouse.Domain.Products;

namespace Warehouse.Domain.Suppliers;

public class Supplier
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Country { get; private set; }
    public string ContactEmail { get; private set; }
    public string PhoneNumber { get; private set; }
    public bool IsActive { get; private set; }
    
    public virtual List<Product> Products { get; private set; } = new List<Product>();

    private Supplier()
    {
        
    }
    public Supplier(
        string name,
        string country,
        string contactEmail,
        string phoneNumber,
        string? id=null
        )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Supplier name is required.");

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Supplier country is required.");

        if (string.IsNullOrWhiteSpace(contactEmail))
            throw new ArgumentException("Supplier contact email is required.");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Supplier phone number is required.");

        Id = id ?? Guid.NewGuid().ToString();
        Name = name;
        Country = country;
        ContactEmail = contactEmail;
        PhoneNumber = phoneNumber;
        IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Supplier is already inactive.");

        IsActive = false;
    }
}