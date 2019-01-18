using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.AutoMapper;
using StarryNight.Entities;

namespace StarryNight.MetaEntities.Dto
{
    public class MetaEntityQueryDto
    {
        [Required]
        public MetaEntityType Type { get; set; }

        [Required]
        public string Sql { get; set; }

        public object Param { get; set; }
    }
}
