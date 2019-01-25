using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using StarryNight.Repository;

namespace StarryNight.Entities
{
    public class TagManager : DomainService, ITagManager
    {
        private readonly ITagRepository tagRepository;

        public TagManager(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public async Task<ICollection<Tag>> GetAllRootTagsAsync()
        {
            return await tagRepository.GetAllListAsync(t => t.ParentId.HasValue != true);
        }

        public async Task<ICollection<Tag>> GetTagsByNameAsync(string name)
        {
            return await tagRepository.GetAllListAsync(t => t.Name.Contains(name));
        }

        public async Task<Tag> RenameTag(Tag tag, string name)
        {
            tag.Name = name;
            return await tagRepository.UpdateAsync(tag);
        }

        public ICollection<Target> GetTargetsOfTag(Tag tag, int takeNum = 100, int skipNum = 0)
        {
            ICollection<Target> targets = new List<Target>();

            targets = tag.TaggedTarget.Skip(skipNum).Take(takeNum).ToList();

            return targets;
        }
    }
}
 