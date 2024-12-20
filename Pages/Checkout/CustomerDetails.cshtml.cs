using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using PizzaritoShop.Data;
using PizzaritoShop.Helpers;
using PizzaritoShop.Model;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace PizzaritoShop.Pages.Checkout
{
    [BindProperties(SupportsGet = true)]
    public class CustomerDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "Cart";

        public CustomerDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PizzaOrder PizzaOrder { get; set; }
        public double TotalPrice { get; set; }
        public List<CartItem> CartItems { get; set; }


        public void OnGet()
        {
            CartItems = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
            TotalPrice = CartItems.Sum(item => item.PizzaPrice * item.Quantity);

            //debugging purpose code
            foreach (var item in CartItems)
            {
                Console.WriteLine($"Pizza: {item.PizzaName}, StuffedCrust: {item.StuffedCrust}, ThinCrispy: {item.ThinCrispy}, Chicken: {item.Chicken}, Pepperoni: {item.Pepperoni}");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            CartItems = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
            TotalPrice = CartItems.Sum(item => item.PizzaPrice * item.Quantity);

            // Combine all the pizza names into a single string separated by commas.
            var combinedPizzaNames = string.Join(", ", CartItems.Select(item => item.PizzaName));  // Combining pizza names

            var newOrder = new OrderListModel
            {
                CustomerName = PizzaOrder.CustomerName,
                Address = PizzaOrder.Address,
                PizzaName = combinedPizzaNames,
                CartItems = CartItems,
                Quantity = CartItems.Sum(item => item.Quantity),
                TotalPrice = CartItems.Sum(item => item.PizzaPrice * item.Quantity),
                CreatedDate = DateTime.Now
            };

            _context.OrdersTable.Add(newOrder);
            await _context.SaveChangesAsync();

            // Clear session and redirect after order
            HttpContext.Session.Remove(CartSessionKey);

            //return RedirectToPage("/Checkout/Payment", new { OrderId = newOrder.Id, TotalPrice });

            return RedirectToPage("/Checkout/ThankYou", new { OrderId = newOrder.Id });
        }
    }
}