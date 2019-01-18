using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StarryNight.Entities
{
    public class MetaEntity
    {
        [Required]
        public MetaEntityType Type { get; set; }

        [Required]
        public string OriginalId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Properties { get; set; }
    }
}
