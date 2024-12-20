using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaritoShop.Data.Services.Base;
using PizzaritoShop.Data.ViewModel;
using PizzaritoShop.Helpers;
using PizzaritoShop.Model;
using PizzaritoShop.Pages.Checkout;
using System.Net.Http;

namespace PizzaritoShop.Data.Services
{
    public class PizzasService : IPizzasService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public PizzasService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<List<PizzasModel>> GetAllPizzasAsync(string apiUrl)
        {
            List<PizzasModel> pizzas = null;

            try
            {
                // Attempt to fetch data from the API
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var pizzasJson = await response.Content.ReadAsStringAsync();
                    pizzas = JsonConvert.DeserializeObject<List<PizzasModel>>(pizzasJson);
                }
            }
            catch (Exception ex)
            {

            }

            // If the API call fails, fallback to fetching from the database
            if (pizzas == null || !pizzas.Any())
            {
                pizzas = await _context.Pizzas.ToListAsync();
            }

            return pizzas;

        }
        public async Task AddNewPizzaAsync(NewPizzaVM data)
        {
            var newPizza = new PizzasModel()
            {
                PizzaName = data.PizzaName,
                Price = data.Price,
                ImageTitle = data.ImageTitle

            };

           await  _context.Pizzas.AddAsync(newPizza);
           await _context.SaveChangesAsync();
        }
        public async Task AddToCartAsync(int pizzaId, string imageTitle, string pizzaName, double pizzaPrice, string cartSessionKey)
        {
            // Retrieve the existing cart from session or create a new one
            var cart = _httpContextAccessor.HttpContext.Session.GetObject<List<CartItem>>(cartSessionKey) ?? new List<CartItem>();

            // Check if the pizza already exists in the cart
            var existingItem = cart.FirstOrDefault(c => c.PizzaId == pizzaId);
            if (existingItem != null)
            {
                // If the pizza already exists, increase the quantity
                existingItem.Quantity++;
            }
            else
            {
                // If not, add the new CartItem to the cart
                var cartItem = new CartItem
                {
                    PizzaId = pizzaId,
                    PizzaName = pizzaName,
                    PizzaPrice = pizzaPrice,
                    Quantity = 1,
                };
                cart.Add(cartItem);
            }

            // Save the updated cart back to the session
            _httpContextAccessor.HttpContext.Session.SetObject(cartSessionKey, cart);
        }

        public async Task<PizzasModel> GetPizzasModelByIdAsync(int id)
        {
            var pizzaDetails = await _context.Pizzas.FirstOrDefaultAsync(p => p.Id == id);

            return pizzaDetails;
        }

        public async Task<PizzasModel> GetByIdAsync(int id)
        {
            return await _context.Pizzas.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(int id, PizzasModel updatedPizza)
        {
            var existingPizza = await _context.Pizzas.FirstOrDefaultAsync(p => p.Id == id);
            if (existingPizza == null)
            {
                throw new KeyNotFoundException($"Pizza with ID {id} not found.");
            }

            // Update specific properties
            existingPizza.PizzaName = updatedPizza.PizzaName;
            existingPizza.Price = updatedPizza.Price;
            existingPizza.ImageTitle = updatedPizza.ImageTitle;

            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var pizza = await _context.Pizzas.FindAsync(id);
            if (pizza == null)
            {
                throw new InvalidOperationException("Pizza not found");
            }

            _context.Pizzas.Remove(pizza);
            await _context.SaveChangesAsync();
        }
    }
}

