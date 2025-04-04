namespace E_Commerce.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateOnly CreatedAt { get; set; }
        public string TransactionID { get; set; } = string.Empty;
        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }

    }
}
