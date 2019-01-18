using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace StarryNight.Tags.Dto
{
    public class TagRenameDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}
