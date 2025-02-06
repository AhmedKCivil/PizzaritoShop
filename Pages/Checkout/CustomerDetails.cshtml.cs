using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using PizzaritoShop.Data;
using Repository.Helpers;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using Core.Entities.Core_Entities;
using Core.Entities.Order_Entities;

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

        public Order PizzaOrder { get; set; }
        public double TotalPrice { get; set; }
        public List<CartItem> CartItems { get; set; }


        public void OnGet()
        {
            CartItems = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
            TotalPrice = CartItems.Sum(item => item.ProductPrice * item.Quantity);

            //debugging purpose code
            foreach (var item in CartItems)
            {
                Console.WriteLine($"Pizza: {item.ProductName}, StuffedCrust: {item.StuffedCrust}, ThinCrispy: {item.ThinCrispy}, Chicken: {item.Chicken}, Pepperoni: {item.Pepperoni}");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            CartItems = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
            TotalPrice = CartItems.Sum(item => item.ProductPrice * item.Quantity);

            // Combine all the pizza names into a single string separated by commas.
            var combinedPizzaNames = string.Join(", ", CartItems.Select(item => item.ProductName));  // Combining pizza names

            var newOrder = new AllOrders
            {
                CustomerName = PizzaOrder.CustomerName,
                Address = PizzaOrder.Address,
                ProductName = combinedPizzaNames,
                CartItems = CartItems,
                Quantity = CartItems.Sum(item => item.Quantity),
                TotalPrice = CartItems.Sum(item => item.ProductPrice * item.Quantity),
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