using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace StarryNight.Entities
{
    public interface ITagManager : IDomainService
    {
        Task<ICollection<Tag>> GetAllRootTagsAsync();

        Task<Tag> RenameTag(Tag tag, string name);

        ICollection<Target> GetTargetsOfTag(Tag tag, int takeNum = 100, int skipNum = 0);
    }
}
