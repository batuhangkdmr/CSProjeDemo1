using CSProjeDemo1.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Service.Interfaces
{
    public interface IUyeService
    {
        Task<IEnumerable<Uye>> GetAllUyelerAsync();
        Task<Uye> GetUyeByIdAsync(int id);
        Task<IEnumerable<Kitap>> GetOduncAldigiKitaplarAsync(int uyeId);
    }
}
