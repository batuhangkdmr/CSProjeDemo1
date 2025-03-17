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
        Task<IEnumerable<Kitap>> GetAllBooksAsync(); // Tüm kitapları getir
        Task<IEnumerable<Kitap>> GetAvailableBooksAsync(); // Sadece ödünç alınabilir kitapları getir
        Task<Kitap> GetBookByIdAsync(int id); // Belirli bir kitabı getir
        Task<bool> OduncAlAsync(int kitapId, string userId); // Kitap ödünç alma
        Task<bool> IadeEtAsync(int kitapId, string userId); // Kitap iade etme
        Task AddKitapAsync(Kitap kitap); // Yeni kitap ekleme
        Task<bool> UpdateKitapAsync(Kitap kitap); // Kitap bilgilerini güncelleme
        Task<bool> DeleteKitapAsync(int id); // Kitap silme
    }
}
