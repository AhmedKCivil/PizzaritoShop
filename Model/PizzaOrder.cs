namespace PizzaritoShop.Model
{
    public class PizzaOrder
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Related cart items
    }
}

