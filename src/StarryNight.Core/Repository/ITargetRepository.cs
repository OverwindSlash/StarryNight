using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Repositories;
using StarryNight.Entities;

namespace StarryNight.Repository
{
    public interface ITargetRepository : IRepository<Target, long>
    {
        IList<Target> GetTargetsByIds(IList<string> queriedTargetIds);
    }
}
