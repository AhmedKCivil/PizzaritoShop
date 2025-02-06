using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PizzaritoShop.Pages.Pizzas
{
    public class EditModel : PageModel
    {
        private readonly IProductsService _pizzasService;
        public EditModel(IProductsService pizzasService)
        {
            _pizzasService = pizzasService;
        }

        [BindProperty]
        public Product PizzaDetail { get; set; } //Property to hold pizza details.

        public async Task<IActionResult> OnGetAsync(int id)
        {
            PizzaDetail = await _pizzasService.GetByIdAsync(id);

            return PizzaDetail == null ? NotFound() : Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _pizzasService.UpdateAsync(id, PizzaDetail);
                TempData["SuccessMessage"] = "Pizza updated successfully!"; // For redirect logic

                return Page();

                //return RedirectToPage("/Pizzas/Pizzas");
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}
