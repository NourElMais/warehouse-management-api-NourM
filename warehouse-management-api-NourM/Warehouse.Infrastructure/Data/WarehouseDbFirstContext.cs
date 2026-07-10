using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Infrastructure.Models;

public partial class WarehouseDbFirstContext : DbContext
{
    public WarehouseDbFirstContext()
    {
    }

    public WarehouseDbFirstContext(DbContextOptions<WarehouseDbFirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productimage> Productimages { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=WarehouseDbFirst;Username=nour;Password=pass123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.HasIndex(e => e.Sku, "products_sku_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(200)
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Expirydate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expirydate");
            entity.Property(e => e.Isarchived)
                .HasDefaultValue(false)
                .HasColumnName("isarchived");
            entity.Property(e => e.LastUpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_updated_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(8, 2)
                .HasColumnName("price");
            entity.Property(e => e.QuantityInStock).HasColumnName("quantity_in_stock");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("sku");
            entity.Property(e => e.SupplierId)
                .HasMaxLength(200)
                .HasColumnName("supplier_id");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_supplier_id_fkey");
        });

        modelBuilder.Entity<Productimage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("productimages_pkey");

            entity.ToTable("productimages");

            entity.Property(e => e.ImageId)
                .HasMaxLength(200)
                .HasColumnName("image_id");
            entity.Property(e => e.FileName)
                .HasMaxLength(200)
                .HasColumnName("file_name");
            entity.Property(e => e.FilePath)
                .HasMaxLength(200)
                .HasColumnName("file_path");
            entity.Property(e => e.ProductId)
                .HasMaxLength(200)
                .HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Productimages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productimages_product_id_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.HasIndex(e => e.ContactEmail, "suppliers_contact_email_key").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "suppliers_phone_number_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(200)
                .HasColumnName("id");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(100)
                .HasColumnName("contact_email");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .HasColumnName("phone_number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
