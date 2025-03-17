using CSProjeDemo1.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Core.Entities
{
    public class Kitap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^\d{9,13}$", ErrorMessage = "ISBN numarası 9 ile 13 basamak arasında olmalıdır.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Kitap başlığı zorunludur.")]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        public string Yazar { get; set; }

        [Range(1000, 2100, ErrorMessage = "Yayın yılı geçerli olmalıdır.")]
        public int YayinYili { get; set; }

        public Durum KitapDurumu { get; set; }

        public Kitap() { }
        public Kitap(string isbn, string baslik, string yazar, int yayinYili)
        {
            ISBN = isbn;
            Baslik = baslik;
            Yazar = yazar;
            YayinYili = yayinYili;
            KitapDurumu = Durum.OduncAlabilir; // Varsayılan olarak ödünç alınabilir
        }
    }
}
