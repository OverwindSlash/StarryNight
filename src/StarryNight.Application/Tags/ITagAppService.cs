using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using StarryNight.Entities;
using StarryNight.Tags.Dto;
using StarryNight.Targets.Dto;

namespace StarryNight.Tags
{
    public interface ITagAppService : IApplicationService
    {
        Task<TagDto> Create(CreateTagDto createRootTagDto);
        Task<TagDto> Get(EntityDto<long> entityDto);
        Task<ICollection<TagDto>> GetByName(string name);

        Task<ICollection<TagDto>> GetAllRootTags();
        Task<TagDto> Rename(TagRenameDto input);
        Task<TagDto> ShiftTo(TagShiftDto input);

        Task<ICollection<TargetDto>> GetTargetsOfTag(QueryTagDto input, int takeNum = 100, int skipNum = 0);
    }
}
