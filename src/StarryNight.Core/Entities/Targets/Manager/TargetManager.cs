using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using StarryNight.Repository;

namespace StarryNight.Entities
{
    public class TargetManager : DomainService, ITargetManager
    {
        private readonly ITargetRepository targetRepository;

        public TargetManager(ITargetRepository targetRepository)
        {
            this.targetRepository = targetRepository;
        }

        public IList<Target> GetTargetsByIds(IList<string> queriedTargetIds)
        {
            return targetRepository.GetTargetsByIds(queriedTargetIds);
        }

        public ICollection<Tag> GetTagsOfTarget(Target target, int takeNum = 100, int skipNum = 0)
        {
            ICollection<Tag> tags = new List<Tag>();

            tags = target.WithTags.Skip(skipNum).Take(takeNum).ToList();

            return tags;
        }
    }
}
