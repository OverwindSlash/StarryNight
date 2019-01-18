using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Abp.Domain.Entities;

namespace StarryNight.Entities
{
    [Table("Targets")]
    public class Target : Entity<long>
    {
        [Required]
        public MetaEntityType Type { get; set; }

        [Required]
        [MaxLength(256)]
        public string OriginalId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public string Properties { get; set; }

        public virtual ICollection<TagTarget> TagTargets { get; set; } = new List<TagTarget>();

        [NotMapped]
        public virtual IEnumerable<Tag> WithTags
        {
            get
            {
                return TagTargets.Select(tt => tt.Tag); 
            }
        }

        public bool HasTag(Tag queriedTag)
        {
            if (WithTags.Contains(queriedTag))
            {
                return true;
            }

            return false;
        }

        public void AppendTag(Tag queriedTag)
        {
            TagTarget tagTarget = new TagTarget()
            {
                Tag = queriedTag,
                Target = this
            };

            TagTargets.Add(tagTarget);
        }

        public void RemoveTag(Tag queriedTag)
        {
            TagTarget tagTarget = TagTargets.SingleOrDefault(tt => tt.Tag == queriedTag);
            TagTargets.Remove(tagTarget);
        }        
    }
}
