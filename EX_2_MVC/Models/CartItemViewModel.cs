namespace EX_2_MVC.Models
{
    public class CartItemViewModel
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
