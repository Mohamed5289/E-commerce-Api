namespace E_Commerce.Models
{
    public class SupplierProduct
    {
        public int Id { get; set; }
        public int? SupplierId { get; set; }
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateOnly CreatedAt { get; set; }

    }
}
