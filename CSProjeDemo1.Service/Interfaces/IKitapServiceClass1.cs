using CSProjeDemo1.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Service.Interfaces
{
    public interface IKitapService
    {
        Task<IEnumerable<Kitap>> GetAllBooksAsync();
        Task<IEnumerable<Kitap>> GetAvailableBooksAsync();
        Task<Kitap> GetBookByIdAsync(int id);
        Task<bool> OduncAlAsync(int kitapId, int uyeId);
        Task<bool> IadeEtAsync(int kitapId);
    }
}
