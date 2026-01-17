using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeyahatGunlugu.Models
{
    public class GunlukModel
    {
        public int Id { get; set; }

        [Required]
        public string? Baslik { get; set; }

        public string? Not { get; set; }

        [DataType(DataType.Date)]
        public DateTime Tarih { get; set; }

        public string? ResimYolu { get; set; }
    }
}
