using CSProjeDemo1.Core.Enums;
using CSProjeDemo1.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Core.Entities
{
    public class Uye : IUye
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int UyeNumarasi { get; set; }
        public List<Kitap> OduncAldigiKitaplar { get; set; }

        public Uye(string ad, string soyad, int uyeNumarasi)
        {
            Ad = ad;
            Soyad = soyad;
            UyeNumarasi = uyeNumarasi;
            OduncAldigiKitaplar = new List<Kitap>();
        }

        public void KitapOduncAl(Kitap kitap)
        {
            if (kitap.KitapDurumu == Durum.OduncAlabilir)
            {
                kitap.KitapDurumu = Durum.OduncVerildi;
                OduncAldigiKitaplar.Add(kitap);
            }
        }

        public void KitapIadeEt(Kitap kitap)
        {
            if (OduncAldigiKitaplar.Contains(kitap))
            {
                kitap.KitapDurumu = Durum.OduncAlabilir;
                OduncAldigiKitaplar.Remove(kitap);
            }
        }
    }
}
