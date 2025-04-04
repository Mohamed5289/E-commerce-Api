using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Data.Configuration
{
    public class Relationships
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            AppUser  (modelBuilder.Entity<AppUser>());
            Payment  (modelBuilder.Entity<Payment>());
            Product  (modelBuilder.Entity<Product>());
            Order    (modelBuilder.Entity<Order>());
            OrderItem(modelBuilder.Entity<OrderItem>());
            Supplier (modelBuilder.Entity<Supplier>());
        }

        private static void AppUser(EntityTypeBuilder<AppUser> entity)
        {
            entity.HasMany(u => u.Orders)
                .WithOne(o => o.AppUser)
                .HasForeignKey(o => o.AppUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(u => u.Supplier)
                .WithOne(s => s.AppUser)
                .HasForeignKey<Supplier>(s => s.AppUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private static void Payment(EntityTypeBuilder<Payment> entity)
        {
            entity.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private static void Product(EntityTypeBuilder<Product> entity)
        {
            entity.HasOne(p => p.Stock)
                .WithOne(s => s.Product)
                .HasForeignKey<Stock>(s => s.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private static void Order(EntityTypeBuilder<Order> entity)
        {
            entity.HasMany(o => o.OrderItems)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.SetNull);

        }

        private static void OrderItem(EntityTypeBuilder<OrderItem> entity)
        {
            entity.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private static void Supplier(EntityTypeBuilder<Supplier> entity)
        {
            entity.HasMany(s => s.Products)
                .WithMany(p => p.Suppliers)
                .UsingEntity<SupplierProduct>(
                j => j.HasOne(sp => sp.Product)
                    .WithMany(p => p.SupplierProducts)
                    .HasForeignKey(sp => sp.ProductId)
                    .OnDelete(DeleteBehavior.SetNull),
                j => j.HasOne(sp => sp.Supplier)
                    .WithMany(s => s.SupplierProducts)
                    .HasForeignKey(sp => sp.SupplierId)
                    .OnDelete(DeleteBehavior.SetNull),
                j =>
                {
                    j.ToTable("SupplierProducts");
                    j.HasKey(sp => sp.Id);
                    j.Property(sp => sp.Price).IsRequired();
                    j.Property(sp => sp.Quantity).IsRequired();
                    j.Property(sp => sp.CreatedAt).IsRequired();

                });

            entity.HasOne(s => s.AppUser)
                .WithOne(u => u.Supplier)
                .HasForeignKey<Supplier>(s => s.AppUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
