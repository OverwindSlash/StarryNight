using System;
using System.Collections.Generic;
using System.Text;
using Abp.AutoMapper;
using StarryNight.Entities;

namespace StarryNight.MetaEntities.Dto
{
    [AutoMapTo(typeof(MetaEntitySyncResult))]
    public class MetaEntityResultDto
    {
        public long InsertedCount { get; set; }
        public long UpdatedCount { get; set; }
        public long UnchangedCount { get; set; }
    }
}
