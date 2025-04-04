namespace E_Commerce.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }

    }
}
