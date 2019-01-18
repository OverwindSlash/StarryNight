using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using StarryNight.Entities;
using StarryNight.EntityFrameworkCore;
using StarryNight.Tags;
using StarryNight.Tags.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StarryNight.Tests.AppService.Tags
{
    public class TagAppService_Tests : StarryNightTestBase
    {
        private readonly ITagAppService tagAppService;

        public TagAppService_Tests()
        {
            tagAppService = Resolve<ITagAppService>();
        }

        [Fact]
        public async Task AddRootTag_Test()
        {
            // Root
            CreateTagDto createRootTagDto = new CreateTagDto { Name = "Root" };
            var rootTagDto = await tagAppService.Create(createRootTagDto);

            var outputDto = await tagAppService.Get(new EntityDto<long> { Id = rootTagDto.Id });

            outputDto.Name.ShouldBe("Root");
            outputDto.FullName.ShouldBe("Root");
            outputDto.Parent.ShouldBe(null);
            outputDto.Children.Count.ShouldBe(0);
        }

        [Fact]
        public async Task AddSubTag_Test()
        {
            // Root
            CreateTagDto createRootTagDto = new CreateTagDto { Name = "Root" };
            var rootTagDto = await tagAppService.Create(createRootTagDto);

            // Sub
            CreateTagDto createSubTagDto = new CreateTagDto { Name = "Sub", ParentId = rootTagDto.Id };
            var subTagDto = await tagAppService.Create(createSubTagDto);

            // Unit test project does not support lazy loading.
            // Navigation properties need eager loading.
            await UsingDbContextAsync(async context =>
            {
                Tag resultTag = await context.Tags.Include(t => t.Parent).FirstOrDefaultAsync(t => t.Id == subTagDto.Id);
                resultTag.Name.ShouldBe("Sub");
                resultTag.FullName.ShouldBe("Root>>Sub");
                resultTag.ParentId.ShouldBe(rootTagDto.Id);
                resultTag.Parent.Name.ShouldBe("Root");
                resultTag.Parent.Children.Count.ShouldBe(1);
                resultTag.Children.Count.ShouldBe(0);
            });
        }

        [Fact]
        public async Task AddLeafTag_Test()
        {
            // Root
            CreateTagDto createRootTagDto = new CreateTagDto { Name = "Root" };
            var rootTagDto = await tagAppService.Create(createRootTagDto);

            // Sub
            CreateTagDto createSubTagDto = new CreateTagDto { Name = "Sub", ParentId = rootTagDto.Id };
            var subTagDto = await tagAppService.Create(createSubTagDto);

            // Leaf
            CreateTagDto createLeafTagDto = new CreateTagDto { Name = "Leaf", ParentId = subTagDto.Id };
            var leafTagDto = await tagAppService.Create(createLeafTagDto);

            // Unit test project does not support lazy loading.
            // Navigation properties need eager loading.
            await UsingDbContextAsync(async context =>
            {
                Tag resultTag = await context.Tags.Include(t => t.Parent)
                .ThenInclude(t => t.Parent).FirstOrDefaultAsync(t => t.Id == leafTagDto.Id);
                resultTag.Name.ShouldBe("Leaf");
                resultTag.FullName.ShouldBe("Root>>Sub>>Leaf");
                resultTag.ParentId.ShouldBe(subTagDto.Id);
                resultTag.Parent.Name.ShouldBe("Sub");
                resultTag.Parent.Children.Count.ShouldBe(1);
                resultTag.Children.Count.ShouldBe(0);
            });
        }

        [Fact]
        public async Task GetAllRootTags_Test()
        {
            await CreateTagTree1();
            await CreateTagTree2();
            await CreateTagTree3();

            var output = await tagAppService.GetAllRootTags();
            output.Count.ShouldBe(3);

            IList<TagDto> tagDtos = output.ToList();
            TagDto rootDto1 = tagDtos[0];
            TagDto rootDto2 = tagDtos[1];
            TagDto rootDto3 = tagDtos[2];

            // Unit test project does not support lazy loading.
            // Navigation properties need eager loading.
            await UsingDbContextAsync(async context =>
            {
                await CheckRootTag1(context, rootDto1);
                await CheckRootTag2(context, rootDto2);
                await CheckRootTag3(context, rootDto3);
            });
        }

        private async Task CreateTagTree1()
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

            // Leaf 112
            CreateTagDto createLeaf112TagDto = new CreateTagDto { Name = "Leaf112", ParentId = sub11TagDto.Id };
            var leaf112TagDto = await tagAppService.Create(createLeaf112TagDto);

            // Sub 12
            CreateTagDto createSub12TagDto = new CreateTagDto { Name = "Sub12", ParentId = root1TagDto.Id };
            var sub12TagDto = await tagAppService.Create(createSub12TagDto);

            // Leaf 121
            CreateTagDto createLeaf121TagDto = new CreateTagDto { Name = "Leaf111", ParentId = sub12TagDto.Id };
            var leaf121TagDto = await tagAppService.Create(createLeaf121TagDto);

            // Leaf 122
            CreateTagDto createLeaf122TagDto = new CreateTagDto { Name = "Leaf112", ParentId = sub12TagDto.Id };
            var leaf122TagDto = await tagAppService.Create(createLeaf122TagDto);
        }

        private async Task CreateTagTree2()
        {
            // Root 2
            CreateTagDto createRoot2TagDto = new CreateTagDto { Name = "Root2" };
            var root2TagDto = await tagAppService.Create(createRoot2TagDto);

            // Sub 21
            CreateTagDto createSub21TagDto = new CreateTagDto { Name = "Sub21", ParentId = root2TagDto.Id };
            var sub21TagDto = await tagAppService.Create(createSub21TagDto);

            // Leaf 211
            CreateTagDto createLeaf211TagDto = new CreateTagDto { Name = "Leaf211", ParentId = sub21TagDto.Id };
            var leaf211TagDto = await tagAppService.Create(createLeaf211TagDto);
        }

        private async Task CreateTagTree3()
        {
            // Root 3
            CreateTagDto createRoot3TagDto = new CreateTagDto { Name = "Root3" };
            var root3TagDto = await tagAppService.Create(createRoot3TagDto);
        }

        private static async Task CheckRootTag1(StarryNightDbContext context, TagDto rootDto1)
        {
            Tag root1Tag = await context.Tags.Include(t => t.Children)
                            .ThenInclude(t => t.Children).FirstOrDefaultAsync(t => t.Id == rootDto1.Id);
            root1Tag.Name.ShouldBe("Root1");
            root1Tag.Children.Count.ShouldBe(2);

            Tag sub11Tag = root1Tag.Children.ToList()[0];
            sub11Tag.Name.ShouldBe("Sub11");
            sub11Tag.Children.Count.ShouldBe(2);

            Tag leaf111 = sub11Tag.Children.ToList()[0];
            leaf111.Name.ShouldBe("Leaf111");
            leaf111.Children.Count.ShouldBe(0);

            Tag leaf112 = sub11Tag.Children.ToList()[1];
            leaf112.Name.ShouldBe("Leaf112");
            leaf112.Children.Count.ShouldBe(0);

            Tag sub12Tag = root1Tag.Children.ToList()[1];
            sub12Tag.Name.ShouldBe("Sub12");
            sub12Tag.Children.Count.ShouldBe(2);

            Tag leaf121 = sub12Tag.Children.ToList()[0];
            leaf121.Name.ShouldBe("Leaf111");
            leaf121.Children.Count.ShouldBe(0);

            Tag leaf122 = sub12Tag.Children.ToList()[1];
            leaf122.Name.ShouldBe("Leaf112");
            leaf122.Children.Count.ShouldBe(0);
        }

        private static async Task CheckRootTag2(StarryNightDbContext context, TagDto rootDto2)
        {
            Tag root2Tag = await context.Tags.Include(t => t.Children)
                            .ThenInclude(t => t.Children).FirstOrDefaultAsync(t => t.Id == rootDto2.Id);
            root2Tag.Name.ShouldBe("Root2");
            root2Tag.Children.Count.ShouldBe(1);

            Tag sub21Tag = root2Tag.Children.ToList()[0];
            sub21Tag.Name.ShouldBe("Sub21");
            sub21Tag.Children.Count.ShouldBe(1);

            Tag leaf211 = sub21Tag.Children.ToList()[0];
            leaf211.Name.ShouldBe("Leaf211");
            leaf211.Children.Count.ShouldBe(0);
        }

        private static async Task CheckRootTag3(StarryNightDbContext context, TagDto rootDto3)
        {
            Tag root3Tag = await context.Tags.Include(t => t.Children)
                            .ThenInclude(t => t.Children).FirstOrDefaultAsync(t => t.Id == rootDto3.Id);
            root3Tag.Name.ShouldBe("Root3");
            root3Tag.Children.Count.ShouldBe(0);
        }

        [Fact]
        public async Task RenameTag_Test()
        {
            await CreateTagTree1();

            // Rename Root
            TagRenameDto tagRenameDto = new TagRenameDto { Id = 2, Name = "New Sub11" };

            TagDto renameSubTagDto = await tagAppService.Rename(tagRenameDto);

            TagDto resultRootTagDto = await tagAppService.Get(new TagDto { Id = tagRenameDto.Id });
            resultRootTagDto.Name.ShouldBe("New Sub11");

            // Unit test project does not support lazy loading.
            // Navigation properties need eager loading.
            await UsingDbContextAsync(async context =>
            {
                Tag root1Tag = await context.Tags.Include(t => t.Children)
                            .ThenInclude(t => t.Children).FirstOrDefaultAsync(t => t.Id == 1);
                root1Tag.Name.ShouldBe("Root1");

                Tag sub11Tag = root1Tag.Children.ToList()[0];
                sub11Tag.Name.ShouldBe("New Sub11");
                sub11Tag.FullName.ShouldBe("Root1>>New Sub11");

                Tag leaf111 = sub11Tag.Children.ToList()[0];
                leaf111.Name.ShouldBe("Leaf111");
                leaf111.FullName.ShouldBe("Root1>>New Sub11>>Leaf111");
            });
        }

        [Fact]
        public async Task ShiftTag_Test()
        {
            await CreateTagTree1();
            await CreateTagTree2();

            // Shift Sub
            TagShiftDto tagShiftDto = new TagShiftDto { Id = 9, NewParentId = 1 };
            TagDto shiftedTagDto = await tagAppService.ShiftTo(tagShiftDto);

            // Unit test project does not support lazy loading.
            // Navigation properties need eager loading.
            await UsingDbContextAsync(async context =>
            {
                Tag root1Tag = await context.Tags.Include(t => t.Children)
                            .ThenInclude(t => t.Children).FirstOrDefaultAsync(t => t.Id == 1);
                root1Tag.Children.Count.ShouldBe(3);

                Tag shiftedSubTag = root1Tag.Children.ToList()[2];
                shiftedSubTag.Name.ShouldBe("Sub21");
                shiftedSubTag.Children.Count.ShouldBe(1);
            });
        }
    }
}
