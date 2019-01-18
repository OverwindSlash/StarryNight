using Abp.Application.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarryNight.Entities;
using StarryNight.MetaEntities.Dto;
using StarryNight.Repository;
using StarryNight.TagTargets.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarryNight.TagTargets
{
    public class TagTargetAppService : ApplicationService, ITagTargetAppService
    {
        private readonly IMetaEntityManager metaEntityManager;
        private readonly ITagTargetManager tagTargetManager;
        private readonly ITagRepository tagRepository;

        public TagTargetAppService(IMetaEntityManager metaEntityManager, ITagTargetManager tagTargetManager, ITagRepository tagRepository)
        {
            this.metaEntityManager = metaEntityManager;
            this.tagTargetManager = tagTargetManager;
            this.tagRepository = tagRepository;
        }

        public void AssignTag(AssignTagDto input)
        {
            IDictionary<string, object> paramDict = GenerateParamDict(input);

            IList<MetaEntity> metaEntities = metaEntityManager.ExtractEntity(input.Type, input.Sql, paramDict);

            MetaEntitySyncResult targetResult = metaEntityManager.SyncEntityToTarget(metaEntities);

            Tag tagResult = tagRepository.FirstOrDefault(t => t.Id == input.TagId);

            tagTargetManager.AssignTag(targetResult.Targets, tagResult);
        }

        private static IDictionary<string, object> GenerateParamDict(AssignTagDto input)
        {
            IDictionary<string, object> paramDict = new Dictionary<string, object>();

            if (input.Param != null)
            {
                var jsonParams = JsonConvert.DeserializeObject(input.Param.ToString()) as JObject;
                foreach (var jsonParam in jsonParams)
                {
                    paramDict[jsonParam.Key] = jsonParam.Value.ToString();
                }
            }

            return paramDict;
        }
    }
}
