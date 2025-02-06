using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaritoShop.Data;
using Service;

namespace PizzaritoShop.Pages.Pizzas
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductsService _productsService;

        public CreateModel(ApplicationDbContext context, IProductsService pizzasService)
        {
            _context = context;
            _productsService = pizzasService;
        }

        [BindProperty]
        public NewProductVM NewProduct { get; set; }
        public List<SelectListItem> Categories { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Categories = await _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();
           
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Categories = await _context.Categories
               .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
               .ToListAsync();

            Console.WriteLine($"Received CategoryId: {NewProduct.CategoryId}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid! Errors:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return Page();
            }

            Console.WriteLine("ModelState is valid, adding product using service...");

            // Use Service to Save Product
            await _productsService.AddNewProductAsync(NewProduct);

            Console.WriteLine("Product added successfully!");

            return RedirectToPage("/Pizzas/Pizzas");
        }
    }
}
