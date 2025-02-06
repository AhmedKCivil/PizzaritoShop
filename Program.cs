using PizzaritoShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Core.Interfaces;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework Core to use SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IProductsService, ProductsService>();

// Configure session with a 30-minute timeout
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container, including Razor Pages and Memory Cache
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpClient(); //newly added for API

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
 
app.UseSession();           // Enable session handling
app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS
app.UseStaticFiles();       // Serve static files
app.UseRouting();           // Use routing
app.UseAuthentication();    // Ensure this comes before authorization
app.UseAuthorization();     // Use authorization

app.MapRazorPages();        // Map Razor Pages


app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

    // Redirect to /Pizzas/Pizzas when accessing the root URL
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/Pizzas/Pizzas");
        return Task.CompletedTask;
    });
});

// Seed roles within a scoped context
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Manager", "Member" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
             await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin@admin.com";
    string password = "Ahmed931022!";

    if(await userManager.FindByEmailAsync(email) ==null)
    {
        var user = new IdentityUser();
        user.UserName = email;
        user.Email = email;
        
        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();
