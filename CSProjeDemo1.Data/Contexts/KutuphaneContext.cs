using CSProjeDemo1.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CSProjeDemo1.Data.Contexts
{
    public class KutuphaneContext : IdentityDbContext<ApplicationUser>
    {
        public KutuphaneContext(DbContextOptions<KutuphaneContext> options) : base(options) { }

        // Parametresiz Constructor (Migration için gerekiyor)
        public KutuphaneContext() { }

        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Uye> Uyeler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity için gerekli

            modelBuilder.Entity<Uye>().HasKey(u => u.Id);
            modelBuilder.Entity<Kitap>().HasKey(k => k.Id);

            modelBuilder.Entity<Kitap>().HasDiscriminator<string>("KitapTuru")
                .HasValue<KitapBilim>("Bilim")
                .HasValue<KitapRoman>("Roman")
                .HasValue<KitapTarih>("Tarih");

            // **Identity Roller ve Kullanıcılar**
            var adminRoleId = "00000000-0000-0000-0000-000000000001"; // Sabit GUID
            var userRoleId = "00000000-0000-0000-0000-000000000002"; // Sabit GUID
            var adminUserId = "10000000-0000-0000-0000-000000000001"; // Sabit GUID
            var normalUserId = "10000000-0000-0000-0000-000000000002"; // Sabit GUID

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" }
            );

            var hasher = new PasswordHasher<ApplicationUser>();

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = adminUserId,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@library.com",
                    NormalizedEmail = "ADMIN@LIBRARY.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123!"),
                    SecurityStamp = string.Empty,
                    Ad = "Admin",
                    Soyad = "Library"
                },
                new ApplicationUser
                {
                    Id = normalUserId,
                    UserName = "user",
                    NormalizedUserName = "USER",
                    Email = "user@library.com",
                    NormalizedEmail = "USER@LIBRARY.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "User123!"),
                    SecurityStamp = string.Empty,
                    Ad = "Normal",
                    Soyad = "User"
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = adminUserId, RoleId = adminRoleId },
                new IdentityUserRole<string> { UserId = normalUserId, RoleId = userRoleId }
            );

            // **Üyeler için Seed Data**
            modelBuilder.Entity<Uye>().HasData(
                new Uye { Id = 1, Ad = "Ali", Soyad = "Yılmaz", UyeNumarasi = 1001 },
                new Uye { Id = 2, Ad = "Ayşe", Soyad = "Demir", UyeNumarasi = 1002 }
            );

            // **Kitaplar için Seed Data**
            modelBuilder.Entity<KitapBilim>().HasData(
                new KitapBilim { Id = 1, ISBN = "123456789", Baslik = "Bilim Kitabı 1", Yazar = "Isaac Newton", YayinYili = 1995, KitapDurumu = Core.Enums.Durum.OduncAlabilir }
            );

            modelBuilder.Entity<KitapRoman>().HasData(
                new KitapRoman { Id = 2, ISBN = "987654321", Baslik = "Roman Kitabı 1", Yazar = "Victor Hugo", YayinYili = 1980, KitapDurumu = Core.Enums.Durum.OduncAlabilir }
            );

            modelBuilder.Entity<KitapTarih>().HasData(
                new KitapTarih { Id = 3, ISBN = "567890123", Baslik = "Tarih Kitabı 1", Yazar = "Halil İnalcık", YayinYili = 2005, KitapDurumu = Core.Enums.Durum.OduncAlabilir }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Hata mesajını engellemek için PendingModelChangesWarning'ı görmezden geliyoruz
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
