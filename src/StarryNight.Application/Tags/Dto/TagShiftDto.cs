using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace StarryNight.Tags.Dto
{
    public class TagShiftDto : EntityDto<long>
    {
        public long? NewParentId { get; set; }
    }
}
