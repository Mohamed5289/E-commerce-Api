namespace E_Commerce.ModelDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }

        public decimal Total { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }
}
