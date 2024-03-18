using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__laba_2_console_traffic_police.Models
{
    public class Model
    {
        [Key]
        public int? ModelId { get; set; }
        [Required]
        public string? ModelName { get; set; }

        public int MarkId { get; set; }
        public byte[]? imgPath { get; set; }

    }
}
