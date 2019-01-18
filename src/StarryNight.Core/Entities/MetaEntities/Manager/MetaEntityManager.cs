using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Services;
using Newtonsoft.Json;
using StarryNight.Repository;

namespace StarryNight.Entities
{
    public class MetaEntityManager : DomainService, IMetaEntityManager
    {
        public int ExistTargetCount { get; private set; }
        public int UpdatedTargetCount { get; private set; }
        public int InsertedTargetCount { get; private set; }

        private readonly IMetaEntityRepository metaEntityRepository;
        private readonly ITargetManager targetManager;
        private readonly ITargetRepository targetRepository;

        public MetaEntityManager(IMetaEntityRepository metaEntityRepository, ITargetManager targetManager, ITargetRepository targetRepository)
        {
            this.metaEntityRepository = metaEntityRepository;
            this.targetManager = targetManager;
            this.targetRepository = targetRepository;
        }

        public IList<MetaEntity> ExtractEntity(MetaEntityType type, string sql, object param = null)
        {            
            return metaEntityRepository.ExtractEntity(type, sql, param);
        }

        public MetaEntitySyncResult SyncEntityToTarget(IList<MetaEntity> queriedEntities)
        {
            try
            {
                // 根据原实体 Id 集合，查找已存在的 Target
                IList<string> originalIds = queriedEntities.Select(t => t.OriginalId).ToList();
                IList<Target> existTargets = targetManager.GetTargetsByIds(originalIds);

                // 筛选出需要更新以及插入的 Target
                IList<Target> targetsToUpdate = new List<Target>();
                IList<Target> targetsToInsert = new List<Target>();
                foreach (MetaEntity queriedEntity in queriedEntities)
                {
                    Target existTarget = existTargets.SingleOrDefault(et =>
                        et.Type == queriedEntity.Type &&
                        et.OriginalId == queriedEntity.OriginalId);

                    // 未找到对应 Target
                    if (existTarget == null)
                    {
                        Target targetToAdd = new Target
                        {
                            Type = queriedEntity.Type,
                            OriginalId = queriedEntity.OriginalId,
                            Name = queriedEntity.Name,
                        };
                        targetsToInsert.Add(targetToAdd);
                        ++InsertedTargetCount;
                        continue;
                    }

                    // 找到记录，但未更改
                    if (existTarget.Name == queriedEntity.Name)
                    {
                        continue;
                    }

                    // 找到记录，并且记录被更改
                    existTarget.Name = queriedEntity.Name;
                    targetsToUpdate.Add(existTarget);
                    ++UpdatedTargetCount;
                }

                // 更新与插入
                foreach (Target targetToUpdate in targetsToUpdate)
                {
                    targetRepository.Update(targetToUpdate);
                }

                foreach (Target targetToInsert in targetsToInsert)
                {
                    targetRepository.Insert(targetToInsert);
                }

                // 获取更新并插入过后的 Target
                IList<Target> syncedTargets = targetManager.GetTargetsByIds(originalIds);

                return new MetaEntitySyncResult
                {
                    Targets = syncedTargets,
                    InsertedCount = targetsToInsert.Count,
                    UpdatedCount = targetsToUpdate.Count,
                    UnchangedCount = queriedEntities.Count - targetsToInsert.Count - targetsToUpdate.Count
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ApplicationException("同步Entity至Target失败", ex);
            }
        }
    }
}
