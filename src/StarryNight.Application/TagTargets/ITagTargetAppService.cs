using Abp.Application.Services;
using StarryNight.TagTargets.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarryNight.TagTargets
{
    public interface ITagTargetAppService : IApplicationService
    {
        void AssignTag(AssignTagDto input);
    }
}
