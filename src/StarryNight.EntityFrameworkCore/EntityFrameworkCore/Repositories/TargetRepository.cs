using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;
using StarryNight.Entities;
using StarryNight.Repository;

namespace StarryNight.EntityFrameworkCore.Repositories
{
    public class TargetRepository : StarryNightRepositoryBase<Target, long>, ITargetRepository
    {
        public TargetRepository(IDbContextProvider<StarryNightDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public IList<Target> GetTargetsByIds(IList<string> queriedTargetIds)
        {
            return GetAllList(t => queriedTargetIds.Contains(t.OriginalId));
        }
    }
}
