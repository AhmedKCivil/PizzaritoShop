using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PizzaritoShop.Data;

namespace PizzaritoShop.Pages.Pizzas
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product NewPizza { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            //Add the new pizza to db

            _context.Pizzas.Add(NewPizza);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Pizzas/Pizzas");
        }

        

    }
}
