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
        public KutuphaneContext() { }

        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<OduncAlma> OduncAlmalar { get; set; }
        public DbSet<Uye> Uyeler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kitaplar için Discriminator (Türler ayrıştırılıyor)
            modelBuilder.Entity<Kitap>().HasDiscriminator<string>("KitapTuru")
                .HasValue<KitapBilim>("Bilim")
                .HasValue<KitapRoman>("Roman")
                .HasValue<KitapTarih>("Tarih");

            // **Rol ve Kullanıcı Seed Data**
            var adminRoleId = "00000000-0000-0000-0000-000000000001";
            var userRoleId = "00000000-0000-0000-0000-000000000002";
            var adminUserId = "10000000-0000-0000-0000-000000000001";
            var normalUserId = "10000000-0000-0000-0000-000000000002";

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

            // **Ödünç Alma Tablosu İçin İlişkiler**
            modelBuilder.Entity<OduncAlma>()
                .HasOne(o => o.Kitap)  // Ödünç alma ilişkisini kitap ile kur
                .WithMany()
                .HasForeignKey(o => o.KitapId)
                .OnDelete(DeleteBehavior.Restrict); // Kitap silindiğinde ödünç alma kaydı silinmez

            modelBuilder.Entity<OduncAlma>()
       .HasOne(o => o.ApplicationUser)
       .WithMany()
       .HasForeignKey(o => o.ApplicationUserId)
       .OnDelete(DeleteBehavior.Restrict);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
