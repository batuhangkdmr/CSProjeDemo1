using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Core.Entities
{
    public class Kutuphane
    {
        public List<Kitap> Kitaplar { get; set; }
        public List<Uye> Uyeler { get; set; }

        public Kutuphane()
        {
            Kitaplar = new List<Kitap>();
            Uyeler = new List<Uye>();
        }

        public void KitapEkle(Kitap kitap)
        {
            Kitaplar.Add(kitap);
        }

        public void UyeEkle(Uye uye)
        {
            Uyeler.Add(uye);
        }

        public List<Kitap> OduncAlinabilirKitaplar()
        {
            return Kitaplar.Where(k => k.KitapDurumu == Enums.Durum.OduncAlabilir).ToList();
        }
    }
}
