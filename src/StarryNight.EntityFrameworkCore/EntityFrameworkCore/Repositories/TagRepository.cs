using System;
using System.Collections.Generic;
using System.Text;
using Abp.EntityFrameworkCore;
using StarryNight.Entities;
using StarryNight.Repository;

namespace StarryNight.EntityFrameworkCore.Repositories
{
    public class TagRepository : StarryNightRepositoryBase<Tag, long>, ITagRepository
    {
        public TagRepository(IDbContextProvider<StarryNightDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
