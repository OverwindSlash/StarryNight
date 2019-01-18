using Abp.Application.Services;
using StarryNight.MetaEntities.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarryNight.MetaEntities
{
    public interface IMetaEntityAppService : IApplicationService
    {
        MetaEntityResultDto ExtractAndSyncEntity(MetaEntityQueryDto input);
    }
}
