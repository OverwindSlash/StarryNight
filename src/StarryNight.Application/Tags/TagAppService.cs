using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using StarryNight.Entities;
using StarryNight.Tags.Dto;
using StarryNight.Targets.Dto;

namespace StarryNight.Tags
{
    public class TagAppService
        : AsyncCrudAppService<Tag, TagDto, long, PagedResultRequestDto, CreateTagDto, TagDto>, ITagAppService
    {
        private readonly ITagManager tagManager;

        public TagAppService(IRepository<Tag, long> repository, ITagManager tagManager)
            : base(repository)
        {
            this.tagManager = tagManager;
        }

        public async Task<ICollection<TagDto>> GetAllRootTags()
        {
            CheckGetPermission();

            ICollection<Tag> rootTags = await tagManager.GetAllRootTagsAsync();

            ICollection<TagDto> rootTagDtos = new List<TagDto>();
            foreach (var rootTag in rootTags)
            {
                TagDto rootTagDto = MapToEntityDto(rootTag);
                rootTagDtos.Add(rootTagDto);
            }

            return rootTagDtos;
        }

        public async Task<ICollection<TagDto>> GetByName(string name)
        {
            CheckGetPermission();

            ICollection<Tag> tags = await tagManager.GetTagsByNameAsync(name);

            ICollection<TagDto> rootTagDtos = new List<TagDto>();
            foreach (var rootTag in tags)
            {
                TagDto rootTagDto = MapToEntityDto(rootTag);
                rootTagDtos.Add(rootTagDto);
            }

            return rootTagDtos;
        }

        public async Task<TagDto> Rename(TagRenameDto input)
        {
            CheckUpdatePermission();

            Tag tag = await Repository.FirstOrDefaultAsync(input.Id);

            Tag renamedTag = await tagManager.RenameTag(tag, input.Name);

            return MapToEntityDto(renamedTag);
        }

        public async Task<TagDto> ShiftTo(TagShiftDto input)
        {
            CheckUpdatePermission();

            Tag tag = await Repository.FirstOrDefaultAsync(input.Id);

            tag.DetachFromParent();
            if (input.NewParentId.HasValue)
            {
                Tag parent = await Repository.FirstOrDefaultAsync(input.NewParentId.Value);
                tag.AttachToParent(parent);
            }

            Tag shiftedTag = await Repository.UpdateAsync(tag);

            return MapToEntityDto(shiftedTag);
        }

        public async Task<ICollection<TargetDto>> GetTargetsOfTag(QueryTagDto input, int takeNum = 100, int skipNum = 0)
        {
            CheckGetPermission();

            Tag tag = await Repository.FirstOrDefaultAsync(input.Id);

            ICollection<Target> taggedTargets = tagManager.GetTargetsOfTag(tag, takeNum, skipNum);

            IList<TargetDto> targetDtos = new List<TargetDto>();
            foreach (Target target in taggedTargets)
            {
                targetDtos.Add(ObjectMapper.Map<TargetDto>(target));
            }

            return targetDtos;
        }
    }
}
