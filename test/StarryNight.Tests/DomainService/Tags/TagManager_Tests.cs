using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using StarryNight.Entities;
using StarryNight.EntityFrameworkCore;
using StarryNight.Repository;
using StarryNight.Tags;
using StarryNight.Tags.Dto;
using Xunit;

namespace StarryNight.Tests.DomainService.Tags
{
    public class TagManager_Tests : StarryNightTestBase
    {
        private readonly ITagManager tagManager;

        private readonly ITagAppService tagAppService;
        private readonly ITagRepository tagRepository;
        private readonly IMetaEntityManager metaEntityManager;
        private readonly ITagTargetManager tagTargetManager;
        private readonly ITagTargetRepository tagTargetRepository;

        public TagManager_Tests()
        {
            tagManager = Resolve<ITagManager>();

            tagAppService = Resolve<ITagAppService>();
            tagRepository = Resolve<ITagRepository>();
            metaEntityManager = Resolve<IMetaEntityManager>();
            tagTargetManager = Resolve<ITagTargetManager>();
            tagTargetRepository = Resolve<ITagTargetRepository>();
        }

        [Fact]
        public async Task GetTagByNameAsync_Tests()
        {
            // Root 1
            CreateTagDto createRoot1TagDto = new CreateTagDto { Name = "Root1" };
            var root1TagDto = await tagAppService.Create(createRoot1TagDto);

            // Sub 11
            CreateTagDto createSub11TagDto = new CreateTagDto { Name = "Sub11", ParentId = root1TagDto.Id };
            var sub11TagDto = await tagAppService.Create(createSub11TagDto);

            // Leaf 111
            CreateTagDto createLeaf111TagDto = new CreateTagDto { Name = "Leaf111", ParentId = sub11TagDto.Id };
            var leaf111TagDto = await tagAppService.Create(createLeaf111TagDto);

            ICollection<Tag> tags1 = await tagManager.GetTagsByNameAsync("Root");
            tags1.Count.ShouldBe(1);

            ICollection<Tag> tags2 = await tagManager.GetTagsByNameAsync("Sub");
            tags2.Count.ShouldBe(1);

            ICollection<Tag> tags3 = await tagManager.GetTagsByNameAsync("Leaf");
            tags3.Count.ShouldBe(1);

            ICollection<Tag> tags4 = await tagManager.GetTagsByNameAsync("11");
            tags4.Count.ShouldBe(2);

            ICollection<Tag> tags5 = await tagManager.GetTagsByNameAsync("1");
            tags5.Count.ShouldBe(3);
        }

        [Fact]
        public async Task GetTargetsOfTagAsync_Tests()
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

            Tag tag = tagRepository.FirstOrDefault(leaf111TagDto.Id);

            tagTargetManager.AssignTag(result1.Targets, tag);

            IList<TagTarget> tagTargets = tagTargetRepository.GetAllList();
            tagTargets.Count.ShouldBe(18);

            using (var context = Resolve<StarryNightDbContext>())
            {
                Tag tagResult = context.Tags
                    .Include(t => t.TagTargets)
                    .ThenInclude(tt => tt.Target)
                    .FirstOrDefault(t => t.Id == leaf111TagDto.Id);
                tagResult.TagTargets.Count.ShouldBe(18);

                ICollection<Target> taggedTargets1 = tagManager.GetTargetsOfTag(tagResult, 10, 0);
                taggedTargets1.Count.ShouldBe(10);

                ICollection<Target> taggedTargets2 = tagManager.GetTargetsOfTag(tagResult, 10, 10);
                taggedTargets2.Count.ShouldBe(8);

                ICollection<Target> taggedTargets3 = tagManager.GetTargetsOfTag(tagResult, 100, 0);
                taggedTargets3.Count.ShouldBe(18);
            }
        }
    }
}
