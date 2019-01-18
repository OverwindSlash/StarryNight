using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using StarryNight.Entities;
using Xunit;


namespace StarryNight.Tests.DomainService.MetaEntitys
{
    public class MetaEntityManager_Tests : StarryNightTestBase
    {
        private readonly IMetaEntityManager metaEntityManager;

        public MetaEntityManager_Tests()
        {
            metaEntityManager = Resolve<IMetaEntityManager>();
        }

        [Fact]
        public void ExtractAndSyncEntity_NoParam_Tests()
        {
            string Sql = @"SELECT Id, Name FROM Product";

            IList<MetaEntity> metaEntities = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql);

            metaEntities.Count.ShouldBe(45);
        }

        [Fact]
        public void ExtractAndSyncEntity_WithOneParam_Tests()
        {
            string Sql = @"SELECT Id, Name FROM Product WHERE VisibleIndividually = @param";

            IList<MetaEntity> metaEntities = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql, new { param = 0 });

            metaEntities.Count.ShouldBe(2);
        }

        [Fact]
        public void ExtractAndSyncEntity_WithTwoParam_Tests()
        {
            string Sql = @"SELECT Id, Name FROM Product WHERE VisibleIndividually = @vi AND ApprovedRatingSum = @ars";

            IList<MetaEntity> metaEntities = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql, new { vi = 1, ars = 4 });

            metaEntities.Count.ShouldBe(18);
        }

        [Fact]
        public void SyncEntityToTarget_AllNew_Tests()
        {
            string Sql = @"SELECT Id, Name FROM Product";

            IList<MetaEntity> metaEntities = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql);

            MetaEntitySyncResult result = metaEntityManager.SyncEntityToTarget(metaEntities);

            result.Targets.Count.ShouldBe(45);
            result.InsertedCount.ShouldBe(45);
            result.UpdatedCount.ShouldBe(0);
            result.UnchangedCount.ShouldBe(0);
        }

        [Fact]
        public void SyncEntityToTarget_SomeExisted_Tests()
        {
            string Sql1 = @"SELECT Id, Name FROM Product WHERE VisibleIndividually = @param";

            IList<MetaEntity> metaEntities1 = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql1, new { param = 0 });

            MetaEntitySyncResult result1 = metaEntityManager.SyncEntityToTarget(metaEntities1);

            result1.Targets.Count.ShouldBe(2);
            result1.InsertedCount.ShouldBe(2);
            result1.UpdatedCount.ShouldBe(0);
            result1.UnchangedCount.ShouldBe(0);


            string Sql2 = @"SELECT Id, Name FROM Product";

            IList<MetaEntity> metaEntities2 = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql2);

            MetaEntitySyncResult result2 = metaEntityManager.SyncEntityToTarget(metaEntities2);

            result2.Targets.Count.ShouldBe(45);
            result2.InsertedCount.ShouldBe(43);
            result2.UpdatedCount.ShouldBe(0);
            result2.UnchangedCount.ShouldBe(2);
        }

        [Fact]
        public void SyncEntityToTarget_SomeUpdated_Tests()
        {
            string Sql1 = @"SELECT Id, Name FROM Product WHERE VisibleIndividually = @param";

            IList<MetaEntity> metaEntities1 = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql1, new { param = 0 });
            metaEntities1[0].Name = "Changed";

            MetaEntitySyncResult result1 = metaEntityManager.SyncEntityToTarget(metaEntities1);

            result1.Targets.Count.ShouldBe(2);
            result1.InsertedCount.ShouldBe(2);
            result1.UpdatedCount.ShouldBe(0);
            result1.UnchangedCount.ShouldBe(0);


            string Sql2 = @"SELECT Id, Name FROM Product";

            IList<MetaEntity> metaEntities2 = metaEntityManager.ExtractEntity(MetaEntityType.Product, Sql2);

            MetaEntitySyncResult result2 = metaEntityManager.SyncEntityToTarget(metaEntities2);

            result2.Targets.Count.ShouldBe(45);
            result2.InsertedCount.ShouldBe(43);
            result2.UpdatedCount.ShouldBe(1);
            result2.UnchangedCount.ShouldBe(1);
        }
    }
}
