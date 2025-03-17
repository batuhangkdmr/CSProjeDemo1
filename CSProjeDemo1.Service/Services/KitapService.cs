using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Core.Enums;
using CSProjeDemo1.Data.IRepositories;
using CSProjeDemo1.Service.Interfaces;
using CSProjeDemo1.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSProjeDemo1.Service.Services
{
    public class KitapService : IKitapService
    {
        private readonly IRepository<Kitap> _kitapRepository;
        private readonly KutuphaneContext _context;

        public KitapService(IRepository<Kitap> kitapRepository, KutuphaneContext context)
        {
            _kitapRepository = kitapRepository;
            _context = context;
        }

        public async Task<IEnumerable<Kitap>> GetAllBooksAsync()
        {
            return await _kitapRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Kitap>> GetAvailableBooksAsync()
        {
            return await _kitapRepository.FindAsync(k => k.KitapDurumu == Durum.OduncAlabilir);
        }

        public async Task<Kitap> GetBookByIdAsync(int id)
        {
            return await _kitapRepository.GetByIdAsync(id);
        }

        public async Task<bool> OduncAlAsync(int kitapId, string userId)
        {
            var kitap = await _kitapRepository.GetByIdAsync(kitapId);
            if (kitap == null || kitap.KitapDurumu != Durum.OduncAlabilir)
                return false;

            // **Ödünç alma kaydı ekle**
            var oduncAlma = new OduncAlma
            {
                ApplicationUserId = userId, // Artık UserId kullanıyoruz
                KitapId = kitapId,
                AlmaTarihi = DateTime.Now
            };

            kitap.KitapDurumu = Durum.OduncVerildi;

            _context.OduncAlmalar.Add(oduncAlma);
            await _kitapRepository.UpdateAsync(kitap);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IadeEtAsync(int kitapId, string userId)
        {
            var odunc = await _context.OduncAlmalar
                .Include(o => o.Kitap)
                .FirstOrDefaultAsync(o => o.KitapId == kitapId && o.ApplicationUserId == userId && o.IadeTarihi == null);

            if (odunc == null)
                return false; // Kitap zaten iade edilmiş veya ödünç alınmamış.

            odunc.IadeTarihi = DateTime.Now;
            odunc.Kitap.KitapDurumu = Durum.OduncAlabilir;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddKitapAsync(Kitap kitap)
        {
            await _kitapRepository.AddAsync(kitap);
        }

        public async Task<bool> UpdateKitapAsync(Kitap kitap)
        {
            var existingKitap = await _kitapRepository.GetByIdAsync(kitap.Id);
            if (existingKitap == null)
                return false;

            existingKitap.ISBN = kitap.ISBN.Trim(); // boşlukları temizlemek için Trim kullandık.
            existingKitap.Baslik = kitap.Baslik;
            existingKitap.Yazar = kitap.Yazar;
            existingKitap.YayinYili = kitap.YayinYili;
            existingKitap.KitapDurumu = kitap.KitapDurumu;

            await _kitapRepository.UpdateAsync(existingKitap);
            return true;
        }

        public async Task<bool> DeleteKitapAsync(int id)
        {
            var kitap = await _kitapRepository.GetByIdAsync(id);
            if (kitap == null)
                return false;

            await _kitapRepository.DeleteAsync(kitap);
            return true;
        }
    }
}
