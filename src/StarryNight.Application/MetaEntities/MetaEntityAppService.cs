using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StarryNight.Entities;
using StarryNight.MetaEntities.Dto;

namespace StarryNight.MetaEntities
{
    public class MetaEntityAppService : IMetaEntityAppService
    {
        private readonly IMetaEntityManager metaEntityManager;

        public MetaEntityAppService(IMetaEntityManager metaEntityManager)
        {
            this.metaEntityManager = metaEntityManager;
        }

        public MetaEntityResultDto ExtractAndSyncEntity(MetaEntityQueryDto input)
        {
            IDictionary<string, object> paramDict = GenerateParamDict(input);

            IList<MetaEntity> metaEntities = metaEntityManager.ExtractEntity(input.Type, input.Sql, paramDict);

            MetaEntitySyncResult result = metaEntityManager.SyncEntityToTarget(metaEntities);

            return new MetaEntityResultDto()
            {
                InsertedCount = result.InsertedCount,
                UpdatedCount = result.UpdatedCount,
                UnchangedCount = result.UnchangedCount
            };
        }

        private static IDictionary<string, object> GenerateParamDict(MetaEntityQueryDto input)
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
