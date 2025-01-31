using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PizzaritoShop.Data;
using Repository.Helpers;
using Core.Entities.Core_Entities;

namespace PizzaritoShop.Pages.Checkout
{
    [BindProperties(SupportsGet = true)]
    public class ThankYouModel : PageModel
    {
        private const string CartSessionKey = "Cart";

        private readonly ApplicationDbContext _context;
        public ThankYouModel(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public double TotalPrice { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public int OrderId { get; set; }  // Get the OrderId from the query string


        public async Task<IActionResult> OnGetAsync()
        {
            // Fetch the order using OrderId passed via query string
            var order = await _context.OrdersTable
                .Include(o => o.CartItems)  // Include the related CartItems
                .FirstOrDefaultAsync(o => o.Id == OrderId);

            if (order == null)
            {
                return NotFound("Order not found. Please ensure the OrderId is correct.");
            }

            // Debugging: Log details about the order
            Console.WriteLine($"Order retrieved. Customer Name: {order.CustomerName}, Total Price: {order.TotalPrice}");

            CustomerName = order.CustomerName;
            Address = order.Address;
            TotalPrice = order.TotalPrice;
            CartItems = order.CartItems;  // Retrieve the CartItems from the order

            return Page();
        }

    }
}
