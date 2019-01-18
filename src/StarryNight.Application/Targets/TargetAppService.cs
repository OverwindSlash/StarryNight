using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using StarryNight.Entities;
using StarryNight.Tags.Dto;
using StarryNight.Targets.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StarryNight.Targets
{
    public class TargetAppService
        : AsyncCrudAppService<Target, TargetDto, long, PagedResultRequestDto, CreateTargetDto, TargetDto>, ITargetAppService
    {
        private readonly ITargetManager targetManager;

        public TargetAppService(IRepository<Target, long> repository, ITargetManager targetManager)
            : base(repository)
        {
            this.targetManager = targetManager;
        }

        public async Task<ICollection<TagDto>> GetTagsOfTargetAsync(QueryTargetDto input, int takeNum = 100, int skipNum = 0)
        {
            CheckGetPermission();

            Target target = await Repository.FirstOrDefaultAsync(input.Id);

            ICollection<Tag> withTags = targetManager.GetTagsOfTarget(target, takeNum, skipNum);

            IList<TagDto> tagDtos = new List<TagDto>();
            foreach (Tag tag in withTags)
            {
                tagDtos.Add(ObjectMapper.Map<TagDto>(tag));
            }

            return tagDtos;
        }
    }
}
