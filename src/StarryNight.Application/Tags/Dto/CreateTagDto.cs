using System;
using System.Collections.Generic;
using System.Text;
using Abp.AutoMapper;
using StarryNight.Entities;

namespace StarryNight.Tags.Dto
{
    [AutoMapTo(typeof(Tag))]
    public class CreateTagDto
    {
        public string Name { get; set; }

        public TagType Type { get; set; }

        public long? ParentId { get; set; }
    }
}
