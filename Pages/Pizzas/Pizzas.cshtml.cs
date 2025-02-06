using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Entities.Core_Entities;

namespace PizzaritoShop.Pages.Pizzas
{
    public class PizzaModel : PageModel
    {
        private const string CartSessionKey = "Cart";
        private readonly IProductsService _pizzasService;
        private readonly IHttpClientFactory _httpClientFactory;

        public PizzaModel(IProductsService pizzasService, IHttpClientFactory httpClientFactory)
        {
            _pizzasService = pizzasService;
            _httpClientFactory = httpClientFactory;
        }

        public List<Product> Pizzas { get; set; } = new List<Product>();
        public Product PizzaDetail { get; set; } //Property to hold pizza details.
        
        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            // Fetch pizzas using the PizzasService (API + Fallback to DB)
            Pizzas = await _pizzasService.GetAllProductsAsync("https://localhost:7030/api/Pizzas");

            // Apply search filter if SearchQuery is provided
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                Pizzas = Pizzas.Where(p => p.ProductName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Get the cart count from session
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
            ViewData["CartCount"] = cart.Sum(item => item.Quantity);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] int pizzaId, [FromForm] string imageTitle, [FromForm] string pizzaName, [FromForm] double pizzaPrice)
        {
            // Handle adding pizza to cart
            await _pizzasService.AddToCartAsync(pizzaId, imageTitle, pizzaName, pizzaPrice, CartSessionKey);

            // Get the updated cart count
            var cartCount = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey)?.Sum(item => item.Quantity) ?? 0;

            // Return the cart count as a JSON response
            return new JsonResult(new { cartCount });
        }



        //public IActionResult OnPost([FromForm] int pizzaId, [FromForm] string imageTitle, [FromForm] string pizzaName, [FromForm] double pizzaPrice)
        //{
        //    // Retrieve the existing cart from session or create a new one
        //    List<CartItem> cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

        //    // Check if the pizza already exists in the cart
        //    var existingItem = cart.FirstOrDefault(c => c.PizzaName == pizzaName);
        //    if (existingItem != null)
        //    {
        //        // If the pizza already exists, increase the quantity
        //        existingItem.Quantity++;
        //    }
        //    else
        //    {
        //        // Otherwise, add a new pizza to the cart
        //        var cartItem = new CartItem
        //        {
        //            PizzaId = pizzaId,
        //            PizzaName = pizzaName,
        //            PizzaPrice = pizzaPrice,
        //            Quantity = 1,
        //            ImageTitle = imageTitle
        //        };
        //        cart.Add(cartItem);
        //    }

        //    // Save the cart back to the session
        //    HttpContext.Session.SetObject(CartSessionKey, cart);

        //    return new JsonResult(new { cartCount = cart.Sum(item => item.Quantity) });
        //}


        //ABOVE EXPLAINED CODE
        //Why Pass These Parameters?
        //Purpose of Passing Data: When the user chooses a pizza and submits the form, you want to keep track of which pizza they selected.
        //By passing ImageTitle and PizzaPrice, you ensure that the checkout page receives the relevant information about the selected pizza.
        //Usage on Checkout Page: In the /Checkout/Checkout page, you can then access these values to display the correct pizza information, calculate totals, or perform any other actions necessary for processing the order


    }
}

//try
//{
//    // Option 1: Fetch from the API
//    var client = _httpClientFactory.CreateClient();
//    var response = await client.GetAsync("https://localhost:7030/api/Pizzas");

//    if (response.IsSuccessStatusCode)
//    {
//        var pizzasJson = await response.Content.ReadAsStringAsync();
//        var pizzas = JsonConvert.DeserializeObject<List<PizzasModel>>(pizzasJson);
//        Pizzas = pizzas;
//    }
//    else
//    {
//        // Handle API failure
//        ModelState.AddModelError(string.Empty, "Error fetching pizzas.");
//    }
//}
//catch (Exception ex)
//{
//    // Handle exception if API call fails
//    ModelState.AddModelError(string.Empty, "An error occurred while contacting the API.");
//}

//// Option 2: Fetch from the database (using repository) as a fallback if the API fails
//if (Pizzas == null || !Pizzas.Any())
//{
//    Pizzas = (await _pizzaRepository.GetAllAsync()).ToList();
//}


//// Apply search filter if SearchQuery is provided
//if (!string.IsNullOrEmpty(SearchQuery))
//{
//    // Filter pizzas based on the PizzaName containing the SearchQuery
//    Pizzas = Pizzas.Where(p => p.PizzaName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
//}

//var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

//ViewData["CartCount"] = cart.Sum(item => item.Quantity);