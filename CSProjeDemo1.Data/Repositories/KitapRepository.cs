using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Data.Repositories
{
    public class KitapRepository : Repository<Kitap>
    {
        public KitapRepository(KutuphaneContext context) : base(context) { }

        public async Task<IEnumerable<Kitap>> GetAvailableBooksAsync()
        {
            return await _dbSet.Where(k => k.KitapDurumu == Core.Enums.Durum.OduncAlabilir).ToListAsync();
        }
    }
}
