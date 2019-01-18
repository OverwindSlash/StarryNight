using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Services;

namespace StarryNight.Entities
{
    public interface IMetaEntityManager : IDomainService
    {
        /// <summary>
        /// 使用 SQL 从任意原始库提取符合条件的实体
        /// </summary>
        /// <param name="type">提取实体的类型</param>
        /// <param name="sql">提取实体的 SQL</param>
        /// <param name="param">SQL 参数</param>
        /// <returns>提取出的实体列表</returns>
        IList<MetaEntity> ExtractEntity(MetaEntityType type, string sql, object param = null);

        /// <summary>
        /// 将实体列表同步至 Target 库
        /// </summary>
        /// <param name="queriedEntities">实体列表</param>
        /// <returns>同步结果，包括 Target 列表，以及插入，更新，未改变的 Target 数量</returns>
        MetaEntitySyncResult SyncEntityToTarget(IList<MetaEntity> queriedEntities);
    }
}
