using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.Core.Logging;
using Dapper;
using StarryNight.Entities;
using StarryNight.Repository;

namespace StarryNight.Dapper
{
    public class MetaEntityRepository : IMetaEntityRepository
    {
        public ILogger Logger { get; set; }

        public MetaEntityRepository()
        {
            Logger = NullLogger.Instance;
        }

        public IList<MetaEntity> ExtractEntity(MetaEntityType type, string sql, object param = null)
        {
            string connStr = DbSettings.GetConnectionString(type.ToString());

            using (IDbConnection connection = new SqlConnection(connStr))
            {
                try
                {
                    connection.Open();

                    IList<MetaEntity> metaEntities = connection.Query(sql, param)
                        .Select(entity =>
                            new MetaEntity
                            {
                                Type = type,
                                OriginalId = $"{entity.Id}",
                                Name = entity.Name
                            }).ToList();

                    return metaEntities;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    throw new ApplicationException("抽取 Entity 失败", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
