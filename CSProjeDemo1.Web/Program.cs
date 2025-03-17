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

            // **Veritaban� Ba�lant�s�**
            builder.Services.AddDbContext<KutuphaneContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // **Generic Repository Pattern Ba��ml�l���**
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // **Servislerin Ba��ml�l�k Enjeksiyonu**
            builder.Services.AddScoped<IKitapService, KitapService>();
            builder.Services.AddScoped<IOduncAlmaService, OduncAlmaService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // **Identity Yap�land�rmas�**
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<KutuphaneContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // **Veritaban� G�ncellenmesi (Migration Otomatik Uygulama)**
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<KutuphaneContext>();
                context.Database.Migrate(); // **Migration'� otomatik uygula**
            }

            // **Middleware Konfig�rasyonu**
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // **Statik Dosyalar i�in Gereklidir**
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Kitap}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
