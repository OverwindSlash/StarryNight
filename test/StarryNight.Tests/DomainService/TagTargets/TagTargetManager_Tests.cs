using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using StarryNight.Entities;
using StarryNight.Repository;
using StarryNight.Tags;
using StarryNight.Tags.Dto;
using Xunit;

namespace StarryNight.Tests.DomainService.TagTargets
{
    public class TagTargetManager_Tests : StarryNightTestBase
    {
        private readonly ITagTargetManager tagTargetManager;
        private readonly IMetaEntityManager metaEntityManager;

        private readonly ITagAppService tagAppService;
        private readonly ITagRepository tagRepository;

        private readonly ITagTargetRepository tagTargetRepository;

        public TagTargetManager_Tests()
        {
            tagTargetManager = Resolve<ITagTargetManager>();
            metaEntityManager = Resolve<IMetaEntityManager>();

            tagAppService = Resolve<ITagAppService>();

            tagRepository = Resolve<ITagRepository>();
            tagTargetRepository = Resolve<ITagTargetRepository>();
        }

        [Fact]
        public async Task AssignTag_Tests()
        {
            string Sql1 = @"SELECT Id, Name FROM Product";
            IList<MetaEntity> metaEntities1 = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql1);
            MetaEntitySyncResult result1 = metaEntityManager.SyncEntityToTarget(metaEntities1);

            string Sql2 = @"SELECT Id, Name FROM Product WHERE VisibleIndividually = @param";
            IList<MetaEntity> metaEntities2 = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql2, new { param = 0 });
            MetaEntitySyncResult result2 = metaEntityManager.SyncEntityToTarget(metaEntities2);

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

            tagTargetManager.AssignTag(result2.Targets, tag);

            IList<TagTarget> tagTargets = tagTargetRepository.GetAllList();

            tagTargets.Count.ShouldBe(2);
            tagTargets[0].TagId.ShouldBe(3);
            tagTargets[0].TargetId.ShouldBe(14);
            tagTargets[1].TagId.ShouldBe(3);
            tagTargets[1].TargetId.ShouldBe(15);
        }
    }
}
