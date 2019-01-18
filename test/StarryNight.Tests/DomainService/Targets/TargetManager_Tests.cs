using Microsoft.EntityFrameworkCore;
using Shouldly;
using StarryNight.Entities;
using StarryNight.EntityFrameworkCore;
using StarryNight.Repository;
using StarryNight.Tags;
using StarryNight.Tags.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StarryNight.Tests.DomainService.Targets
{
    public class TargetManager_Tests : StarryNightTestBase
    {
        private readonly ITargetManager targetManager;

        private readonly ITagAppService tagAppService;
        private readonly ITagRepository tagRepository;
        private readonly IMetaEntityManager metaEntityManager;
        private readonly ITagTargetManager tagTargetManager;
        private readonly ITagTargetRepository tagTargetRepository;

        public TargetManager_Tests()
        {
            targetManager = Resolve<ITargetManager>();

            tagAppService = Resolve<ITagAppService>();
            tagRepository = Resolve<ITagRepository>();
            metaEntityManager = Resolve<IMetaEntityManager>();
            tagTargetManager = Resolve<ITagTargetManager>();
            tagTargetRepository = Resolve<ITagTargetRepository>();
        }

        [Fact]
        public async Task GetTagsOfTargetAsync_Tests()
        {
            string Sql1 = @"SELECT Id, Name FROM Product WHERE VisibleIndividually = @vi AND ApprovedRatingSum = @ars";
            IList<MetaEntity> metaEntities1 = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql1, new { vi = 1, ars = 4 });
            MetaEntitySyncResult result1 = metaEntityManager.SyncEntityToTarget(metaEntities1);

            // Root 1
            CreateTagDto createRoot1TagDto = new CreateTagDto { Name = "Root1" };
            var root1TagDto = await tagAppService.Create(createRoot1TagDto);

            // Sub 11
            CreateTagDto createSub11TagDto = new CreateTagDto { Name = "Sub11", ParentId = root1TagDto.Id };
            var sub11TagDto = await tagAppService.Create(createSub11TagDto);

            // Leaf 111
            CreateTagDto createLeaf111TagDto = new CreateTagDto { Name = "Leaf111", ParentId = sub11TagDto.Id };
            var leaf111TagDto = await tagAppService.Create(createLeaf111TagDto);

            // Leaf 112
            CreateTagDto createLeaf112TagDto = new CreateTagDto { Name = "Leaf112", ParentId = sub11TagDto.Id };
            var leaf112TagDto = await tagAppService.Create(createLeaf112TagDto);

            Tag tag1 = tagRepository.FirstOrDefault(leaf111TagDto.Id);
            Tag tag2 = tagRepository.FirstOrDefault(leaf112TagDto.Id);

            tagTargetManager.AssignTag(result1.Targets, tag1);
            tagTargetManager.AssignTag(result1.Targets[0], tag2);

            IList<TagTarget> tagTargets = tagTargetRepository.GetAllList();
            tagTargets.Count.ShouldBe(19);

            using (var context = Resolve<StarryNightDbContext>())
            {
                Target target1 = context.Targets
                    .Include(t => t.TagTargets)
                    .ThenInclude(tt => tt.Tag)
                    .FirstOrDefault(t => t.Id == result1.Targets[0].Id);
                target1.WithTags.Count().ShouldBe(2);

                ICollection<Tag> withTags1 = targetManager.GetTagsOfTarget(target1, 10, 0);
                withTags1.Count.ShouldBe(2);

                Target target2 = context.Targets
                    .Include(t => t.TagTargets)
                    .ThenInclude(tt => tt.Tag)
                    .FirstOrDefault(t => t.Id == result1.Targets[1].Id);
                target2.WithTags.Count().ShouldBe(1);

                ICollection<Tag> withTags2 = targetManager.GetTagsOfTarget(target2, 10, 0);
                withTags2.Count.ShouldBe(1);
            }
        }
    }
}
