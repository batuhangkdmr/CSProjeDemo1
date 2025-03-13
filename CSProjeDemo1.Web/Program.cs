using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Core.Interfaces;
using CSProjeDemo1.Data.Contexts;
using CSProjeDemo1.Data.IRepositories;
using CSProjeDemo1.Data.Repositories;
using CSProjeDemo1.Service.Interfaces;
using CSProjeDemo1.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CSProjeDemo1.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<KutuphaneContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IKitapService, KitapService>();
            builder.Services.AddScoped<IUyeService, UyeService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<KutuphaneContext>()
    .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Kitap}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
