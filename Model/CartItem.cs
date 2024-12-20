namespace PizzaritoShop.Model
{
    public class CartItem
    {
        public int CartItemId { get; set; }  // Primary key for the cart item
        public int PizzaId { get; set; }
        public string PizzaName { get; set; }
        public int Quantity { get; set; }
        public double PizzaPrice { get; set; }
        //public double TotalPrice => Quantity * PizzaPrice;

        // Foreign Key to OrderListModel
        public int OrderListModelId { get; set; }

        // Navigation property to link to the order
        public OrderListModel Order { get; set; }

        //Custom Pizza Attributes
        public bool StuffedCrust { get; set; }
        public bool ThinCrispy { get; set; }
        public bool Chicken { get; set; }
        public bool Pepperoni { get; set; }

    }



}
