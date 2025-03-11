using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Core.Enums;
using CSProjeDemo1.Data.IRepositories;
using CSProjeDemo1.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Service.Services
{
    public class KitapService : IKitapService
    {
        private readonly IRepository<Kitap> _kitapRepository;

        public KitapService(IRepository<Kitap> kitapRepository)
        {
            _kitapRepository = kitapRepository;
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

        public async Task<bool> OduncAlAsync(int kitapId, int uyeId)
        {
            var kitap = await _kitapRepository.GetByIdAsync(kitapId);
            if (kitap == null || kitap.KitapDurumu != Durum.OduncAlabilir)
                return false;

            kitap.KitapDurumu = Durum.OduncVerildi;
            await _kitapRepository.UpdateAsync(kitap);
            return true;
        }

        public async Task<bool> IadeEtAsync(int kitapId)
        {
            var kitap = await _kitapRepository.GetByIdAsync(kitapId);
            if (kitap == null || kitap.KitapDurumu != Durum.OduncVerildi)
                return false;

            kitap.KitapDurumu = Durum.OduncAlabilir;
            await _kitapRepository.UpdateAsync(kitap);
            return true;
        }
    }
}
