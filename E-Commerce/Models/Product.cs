namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty ;
        public string ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual Stock? Stock {  get; set; }
        public virtual ICollection<Supplier>? Suppliers { get; set; }
        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }

    }
}
