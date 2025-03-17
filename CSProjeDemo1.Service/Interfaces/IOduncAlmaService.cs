using CSProjeDemo1.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Service.Interfaces
{
    public interface IOduncAlmaService
    {
        Task<bool> KitapOduncAl(string uyeId, int kitapId);
        Task<bool> KitapIadeEt(int oduncId);
        Task<List<OduncAlma>> KullaniciOduncKitaplari(string uyeId);
    }
}
