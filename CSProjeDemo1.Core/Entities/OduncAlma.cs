using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProjeDemo1.Core.Entities
{
    public class OduncAlma
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }  // Foreign Key olarak zaten tanımlandı

        [Required]
        public int KitapId { get; set; }  // Foreign Key

        [Required]
        public DateTime AlmaTarihi { get; set; } = DateTime.Now;

        public DateTime? IadeTarihi { get; set; } // Nullable (iade edilmediyse null kalır)

        // Navigation Properties
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("KitapId")]
        public virtual Kitap Kitap { get; set; }
    }


}
