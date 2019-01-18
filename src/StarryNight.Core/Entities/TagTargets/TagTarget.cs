using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;

namespace StarryNight.Entities
{
    [Table("TagTargets")]
    public class TagTarget : Entity<long>
    {
        public long TagId { get; set; }

        public virtual Tag Tag { get; set; }

        public long TargetId { get; set; }

        public virtual Target Target { get; set; }
    }
}
