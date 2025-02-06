using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace PizzaritoShop.Pages.Pizzas
{
    public class DetailsModel : PageModel
    {
        private readonly IProductsService _pizzasService;

        public DetailsModel(IProductsService pizzasService)
        {
            _pizzasService = pizzasService;
        }

        public Product PizzaDetail { get; set; } //Property to hold pizza details.

        public async Task<IActionResult> OnGetAsync(int id)
        {
            PizzaDetail = await _pizzasService.GetByIdAsync(id);

            return PizzaDetail == null ? NotFound() : Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _pizzasService.DeleteAsync(id);
                return RedirectToPage("/Pizzas/Pizzas"); 
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

    }
}
