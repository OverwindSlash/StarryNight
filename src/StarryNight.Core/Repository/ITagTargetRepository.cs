using Abp.Domain.Repositories;
using StarryNight.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarryNight.Repository
{
    public interface ITagTargetRepository : IRepository<TagTarget, long>
    {
    }
}
