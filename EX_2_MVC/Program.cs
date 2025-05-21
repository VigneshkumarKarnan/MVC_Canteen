//using EX_2_MVC.Models;
//using Microsoft.EntityFrameworkCore;

//namespace EX_2_MVC
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            builder.Services.AddDbContext<ApplicationDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//            // Add services to the container.
//            builder.Services.AddControllersWithViews();
//            builder.Services.AddSession();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Home/Error");
//            }
//            app.UseStaticFiles();

//            app.UseRouting();
//            app.UseSession();

//            app.UseAuthorization();

//            app.MapControllerRoute(
//                name: "default",
//                pattern: "{controller=Home}/{action=Index}/{id?}");

//            app.Run();
//        }
//    }
//}



using EX_2_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EX_2_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Seed the database
            SeedDatabase(app);

            app.Run();
        }

        // Add this method to seed MenuItems
        private static void SeedDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            if (!context.MenuItems.Any())
            {
                context.MenuItems.AddRange(
                    new MenuItem { Name = "Samosa", Price = 15.00m, Description = "Spicy fried snack", Category = "Snacks" },
                    new MenuItem { Name = "Tea", Price = 10.00m, Description = "Hot chai", Category = "Drinks" },
                    new MenuItem { Name = "Veg Biryani", Price = 60.00m, Description = "Aromatic rice dish", Category = "Meals" }
                );
                context.SaveChanges();
            }
        }
    }
}
