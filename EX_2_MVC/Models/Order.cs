namespace EX_2_MVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Canceled", etc.

        public List<OrderItem> Items { get; set; }
    }
}
