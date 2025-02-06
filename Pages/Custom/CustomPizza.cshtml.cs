using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Collections.Specialized.BitVector32;
using System;
using Microsoft.AspNetCore.Http;
using Repository.Helpers;
using Core.Entities;
using Core.Entities.Core_Entities;

namespace PizzaritoShop.Pages.Forms
{
    [BindProperties(SupportsGet = true)]
    public class CustomPizzaModel : PageModel
    {
        private const string CartSessionKey = "Cart";
        public CustomPizza CustomPizza { get; set; }
        public double PizzaPrice { get; set; }

        public IActionResult OnPost()
        {
            PizzaPrice = CustomPizza.BasePrice;

            if (CustomPizza.StuffedCrust) PizzaPrice += 4.5;
            if (CustomPizza.ThinCrispy) PizzaPrice += 2.5;
            if (CustomPizza.Chicken) PizzaPrice += 6.5;
            if (CustomPizza.Pepperoni) PizzaPrice += 5.5;

            CustomPizza.TotalPrice = PizzaPrice;

            var customPizzaCartItem = new CartItem
            {
                ProductId = CustomPizza.Id,
                //PizzaName = "Custom Pizza",
                ProductName = string.IsNullOrEmpty(CustomPizza.ProductName) ? "Custom Pizza" : CustomPizza.ProductName, // Use user input or fallback to "Custom Pizza"
                ProductPrice = PizzaPrice,
                Quantity = 1,
                StuffedCrust = CustomPizza.StuffedCrust,
                ThinCrispy = CustomPizza.ThinCrispy,
                Chicken = CustomPizza.Chicken,
                Pepperoni = CustomPizza.Pepperoni
            };

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
            
            cart.Add(customPizzaCartItem);

            // Store the custom pizza object in the session
            //HttpContext.Session.SetObject("CustomPizza", CustomPizza);
            HttpContext.Session.SetObject(CartSessionKey, cart);

            //debugging purpose
            foreach (var item in cart)
            {
                Console.WriteLine($"Pizza: {item.ProductName}, StuffedCrust: {item.StuffedCrust}, ThinCrispy: {item.ThinCrispy}, Chicken: {item.Chicken}, Pepperoni: {item.Pepperoni}");
            }


            return RedirectToPage("/Cart/Cart");
               
        }

     
    }
}



