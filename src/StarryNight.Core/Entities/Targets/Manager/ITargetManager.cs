using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace StarryNight.Entities
{
    public interface ITargetManager : IDomainService
    {
        IList<Target> GetTargetsByIds(IList<string> queriedTargetIds);

        ICollection<Tag> GetTagsOfTarget(Target target, int takeNum = 100, int skipNum = 0);
    }
}
