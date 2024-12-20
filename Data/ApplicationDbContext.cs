using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaritoShop.Model;

namespace PizzaritoShop.Data
    
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<PizzasModel> Pizzas { get; set; }
        public DbSet<OrderListModel> OrdersTable { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-many relationship
            modelBuilder.Entity<OrderListModel>()
                .HasMany(o => o.CartItems)                  // Indicates that OrderListModel has many CartItems
                .WithOne(c => c.Order)                      // Indicates that each CartItem is associated with one OrderListModel
                .HasForeignKey(c => c.OrderListModelId);    // Specifies that CartItem's foreign key is OrderListModelId


            base.OnModelCreating(modelBuilder);
        }

        //If CartItem is not meant to be saved as a separate table in the database(and you're using it only for
        //temporary in-memory storage or serialization in the session), you can configure it as a keyless entity.
        //In this case, you can modify the OnModelCreating method in your DbContext to explicitly tell Entity Framework
        //that CartItem does not have a key:

    }



}
