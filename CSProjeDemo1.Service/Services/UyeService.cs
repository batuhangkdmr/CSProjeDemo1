using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Data.IRepositories;
using CSProjeDemo1.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Service.Services
{
    public class UyeService : IUyeService
    {
        private readonly IRepository<Uye> _uyeRepository;

        public UyeService(IRepository<Uye> uyeRepository)
        {
            _uyeRepository = uyeRepository;
        }

        public async Task<IEnumerable<Uye>> GetAllUyelerAsync()
        {
            return await _uyeRepository.GetAllAsync();
        }

        public async Task<Uye> GetUyeByIdAsync(int id)
        {
            return await _uyeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Kitap>> GetOduncAldigiKitaplarAsync(int uyeId)
        {
            var uye = await _uyeRepository.GetByIdAsync(uyeId);
            return uye?.OduncAldigiKitaplar ?? new List<Kitap>();
        }
    }
}
