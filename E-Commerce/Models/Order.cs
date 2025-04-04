namespace E_Commerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string Status { get; set; } = "Pending";
        public decimal TotalPrice { get; set; }
        public string? AppUserId { get; set; }
        public virtual AppUser? AppUser  { get; set; }
        public int PaymentId { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual Payment? Payment { get; set; }
    }
}
