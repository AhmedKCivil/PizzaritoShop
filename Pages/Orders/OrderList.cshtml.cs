using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PizzaritoShop.Data;
using Repository.Helpers;
using Core.Entities.Core_Entities;
using Core.Entities.Order_Entities;

namespace PizzaritoShop.Pages.Orders
{
    [Authorize (Roles = "Admin, Manager")]
    public class OrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "Cart";

        public OrdersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<AllOrders> OrderList { get; set; } = new List<AllOrders>();
        public List<CartItem> CartItems { get; set; }
        public double TotalPrice { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            // Fetch orders along with their related CartItems
            OrderList = await _context.OrdersTable
                .Include(o => o.CartItems)  // Include CartItems in the query
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();

            return Page();
        }


    }

}
