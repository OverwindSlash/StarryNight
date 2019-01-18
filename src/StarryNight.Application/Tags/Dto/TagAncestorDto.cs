using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using StarryNight.Entities;

namespace StarryNight.Tags.Dto
{
    [AutoMapTo(typeof(Tag))]
    public class TagAncestorDto : EntityDto<long>
    {
        public string Name { get; set; }

        public TagType Type { get; set; }

        public long? ParentId { get; set; }

        public string FullName { get; set; }

        public TagAncestorDto Parent { get; set; }
    }
}
