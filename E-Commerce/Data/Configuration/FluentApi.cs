using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Data.Configuration
{
    public class FluentApi
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            AppUser(modelBuilder.Entity<AppUser>());
            Payment(modelBuilder.Entity<Payment>());
            Product(modelBuilder.Entity<Product>());
            Stock(modelBuilder.Entity<Stock>());
            Category(modelBuilder.Entity<Category>());
            Order(modelBuilder.Entity<Order>());
            OrderItem(modelBuilder.Entity<OrderItem>());
            SupplierProduct(modelBuilder.Entity<SupplierProduct>());

        }

        private static void AppUser(EntityTypeBuilder<AppUser> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(30);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(30);
            entity.OwnsOne(u => u.Address, a =>
            {
                a.Property(p => p.Street).HasColumnName("Street").HasMaxLength(30);
                a.Property(p => p.City).HasColumnName("City").HasMaxLength(30);
                a.Property(p => p.Governorate).HasColumnName("Governorate").HasMaxLength(30);
            });

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.UserName).IsUnique();
        }

        private static void Payment(EntityTypeBuilder<Payment> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Method).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(30);
            entity.Property(e => e.TransactionID).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();
        }

        private static void Product(EntityTypeBuilder<Product> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Price).HasColumnType("decimal(18,4)").IsRequired();
            entity.Property(e => e.Description).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
        }

        private static void Stock(EntityTypeBuilder<Stock> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).IsRequired();
        }

        private static void Category(EntityTypeBuilder<Category> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(30);
            entity.HasIndex(e => e.Name).IsUnique();
        }

        private static void Order(EntityTypeBuilder<Order> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(30);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,4)").IsRequired();
        }

        private static void OrderItem(EntityTypeBuilder<OrderItem> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18,4)").IsRequired();
        }

        private static void SupplierProduct(EntityTypeBuilder<SupplierProduct> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasColumnType("decimal(18,4)").IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        }
    }

}
