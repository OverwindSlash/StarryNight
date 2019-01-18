using System;
using System.Collections.Generic;
using System.Text;

namespace StarryNight.Entities
{
    public class MetaEntitySyncResult
    {
        public IList<Target> Targets { get; set; }

        public long InsertedCount { get; set; }
        public long UpdatedCount { get; set; }
        public long UnchangedCount { get; set; }
    }
}
