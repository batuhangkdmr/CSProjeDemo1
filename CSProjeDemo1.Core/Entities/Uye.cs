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
        [Required(ErrorMessage = "Üye adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Ad 50 karakterden uzun olamaz.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Üye soyadı zorunludur.")]
        [StringLength(50, ErrorMessage = "Soyad 50 karakterden uzun olamaz.")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Üye numarası zorunludur.")]
        [Range(1000, 9999, ErrorMessage = "Üye numarası 1000 ile 9999 arasında olmalıdır.")]
        public int UyeNumarasi { get; set; }
        // Bir üyenin ödünç aldığı kitapların listesi
        public List<Kitap> OduncAldigiKitaplar { get; set; } = new List<Kitap>();

        public Uye()
        {
            
        }
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
