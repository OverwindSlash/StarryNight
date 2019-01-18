using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using StarryNight.Entities;
using StarryNight.Tags.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarryNight.Targets.Dto
{
    [AutoMapTo(typeof(Target))]
    public class TargetDto : EntityDto<long>
    {
        public MetaEntityType Type { get; set; }

        public string OriginalId { get; set; }

        public string Name { get; set; }

        public string Properties { get; set; }

        public ICollection<TagDto> WithTags { get; set; }
    }
}
