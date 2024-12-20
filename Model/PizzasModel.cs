using PizzaritoShop.Data.Services.Base;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PizzaritoShop.Model
{
    public class PizzasModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Pizza Name")]
        public string PizzaName { get; set; }
        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Image URL")]
        public string ImageTitle { get; set; }


        //public string StripePriceId { get; set; }
    }
}


