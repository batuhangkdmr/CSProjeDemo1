using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Data.Contexts;
using CSProjeDemo1.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSProjeDemo1.Service.Services
{
    public class OduncAlmaService : IOduncAlmaService
    {
        private readonly KutuphaneContext _context;

        public OduncAlmaService(KutuphaneContext context)
        {
            _context = context;
        }

        public async Task<bool> KitapOduncAl(string applicationUserId, int kitapId)
        {
            Console.WriteLine($"DEBUG: KitapOduncAl metodu çağrıldı. KullanıcıID: {applicationUserId}, KitapID: {kitapId}");

            if (string.IsNullOrEmpty(applicationUserId))
            {
                Console.WriteLine("DEBUG: Kullanıcı ID boş!");
                return false;
            }

            var kitap = await _context.Kitaplar.FindAsync(kitapId);
            if (kitap == null)
            {
                Console.WriteLine("DEBUG: Kitap bulunamadı!");
                return false;
            }

            if (kitap.KitapDurumu != Core.Enums.Durum.OduncAlabilir)
            {
                Console.WriteLine("DEBUG: Kitap ödünç alınamaz durumda!");
                return false;
            }

            bool zatenOduncAlindi = await _context.OduncAlmalar
                .AnyAsync(o => o.ApplicationUserId == applicationUserId && o.KitapId == kitapId && o.IadeTarihi == null);

            if (zatenOduncAlindi)
            {
                Console.WriteLine("DEBUG: Kullanıcı bu kitabı zaten ödünç almış!");
                return false;
            }

            Console.WriteLine("DEBUG: Ödünç alma işlemi başlıyor...");

            var odunc = new OduncAlma
            {
                ApplicationUserId = applicationUserId,
                KitapId = kitapId,
                AlmaTarihi = DateTime.Now
            };

            kitap.KitapDurumu = Core.Enums.Durum.OduncVerildi;
            _context.OduncAlmalar.Add(odunc);
            int affectedRows = await _context.SaveChangesAsync();

            Console.WriteLine($"DEBUG: SaveChanges sonrası etkilenen satır: {affectedRows}");

            return affectedRows > 0;
        }

        public async Task<bool> KitapIadeEt(int kitapId)
        {
            var odunc = await _context.OduncAlmalar
                .Include(o => o.Kitap)
                .FirstOrDefaultAsync(o => o.KitapId == kitapId && o.IadeTarihi == null);

            if (odunc == null)
                return false; // Kitap zaten iade edilmiş veya ödünç alınmamış.

            odunc.IadeTarihi = DateTime.Now;
            odunc.Kitap.KitapDurumu = Core.Enums.Durum.OduncAlabilir; // Kitap tekrar ödünç alınabilir hale getirildi

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OduncAlma>> KullaniciOduncKitaplari(string uyeId)
        {
            return await _context.OduncAlmalar
                .Include(o => o.Kitap)
                .Where(o => o.ApplicationUserId == uyeId && o.IadeTarihi == null)
                .ToListAsync();
        }
    }
}
