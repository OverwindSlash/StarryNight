using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Repositories;
using StarryNight.Entities;

namespace StarryNight.Repository
{
    public interface IMetaEntityRepository : IRepository
    {
        IList<MetaEntity> ExtractEntity(MetaEntityType type, string sql, object param = null);
    }
}
