using CSProjeDemo1.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Data.Contexts
{
    public class KutuphaneContext : DbContext
    {
        public KutuphaneContext() { }

        public KutuphaneContext(DbContextOptions<KutuphaneContext> options) : base(options) { }

        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Uye> Uyeler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Uye>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Kitap>()
                .HasKey(k => k.Id);

            modelBuilder.Entity<Kitap>().HasDiscriminator<string>("KitapTuru")
                .HasValue<KitapBilim>("Bilim")
                .HasValue<KitapRoman>("Roman")
                .HasValue<KitapTarih>("Tarih");

            base.OnModelCreating(modelBuilder);
        }



    }
}
