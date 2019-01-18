using StarryNight.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StarryNight.TagTargets.Dto
{
    public class AssignTagDto
    {
        [Required]
        public MetaEntityType Type { get; set; }

        [Required]
        public string Sql { get; set; }

        public object Param { get; set; }

        [Required]
        public long TagId { get; set; }
    }
}
