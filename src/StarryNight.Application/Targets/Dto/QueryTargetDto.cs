using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using StarryNight.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarryNight.Targets.Dto
{
    [AutoMapTo(typeof(Target))]
    public class QueryTargetDto : EntityDto<long>
    {
        public MetaEntityType Type { get; set; }
    }
}
