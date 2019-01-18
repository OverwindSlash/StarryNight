using Abp.Application.Services;
using StarryNight.Tags.Dto;
using StarryNight.Targets.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StarryNight.Targets
{
    public interface ITargetAppService : IApplicationService
    {
        Task<ICollection<TagDto>> GetTagsOfTargetAsync(QueryTargetDto input, int takeNum = 100, int skipNum = 0);
    }
}
