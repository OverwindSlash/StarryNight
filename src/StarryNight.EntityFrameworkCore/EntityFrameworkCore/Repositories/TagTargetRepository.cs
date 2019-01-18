using Abp.EntityFrameworkCore;
using StarryNight.Entities;
using StarryNight.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarryNight.EntityFrameworkCore.Repositories
{
    public class TagTargetRepository : StarryNightRepositoryBase<TagTarget, long>, ITagTargetRepository
    {
        public TagTargetRepository(IDbContextProvider<StarryNightDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
