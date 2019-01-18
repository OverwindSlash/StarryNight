using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Abp.Domain.Entities;

namespace StarryNight.Entities
{
    public enum TagType
    {
        Static = 1,
        Dynamic = 2
    }

    [Table("Tags")]
    public class Tag : Entity<long>
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        public TagType Type { get; set; }

        [Required]
        [DefaultValue("2100-1-1")]
        public DateTime ExpireTime { get; set; }

        public long? ParentId { get; set; }

        public string FullName => Parent == null ? Name : $"{Parent.FullName}>>{Name}";

        [ForeignKey("ParentId")]
        public virtual Tag Parent { get; set; }

        public virtual ICollection<Tag> Children { get; } = new List<Tag>();

        public virtual ICollection<TagTarget> TagTargets { get; set; } = new List<TagTarget>();

        [NotMapped]
        public virtual IEnumerable<Target> TaggedTarget
        {
            get
            {
                return TagTargets.Select(tt => tt.Target);
            }
        }
                   
        public Tag()
        {
            Name = "未知";
            Type = TagType.Static;
        }

        public Tag(string name, Tag parent = null, TagType type = TagType.Static)
        {
            Name = name;
            Type = type;

            if (parent != null)
            {
                AttachToParent(parent);
            }
        }

        public void AttachToParent(Tag parent)
        {
            if (parent == null) { throw new ArgumentException(nameof(parent)); }

            parent.AddChildTag(this);
        }

        public void DetachFromParent()
        {
            Parent?.RemoveChildTag(this);
        }

        public void AddChildTag(Tag child)
        {
            if (child == null) { throw new ArgumentException(nameof(child)); }

            if (Children.Contains(child))
            {
                return;
            }

            child.ParentId = this.Id;
            Children.Add(child);
        }

        public void RemoveChildTag(Tag child)
        {
            if (child == null) { throw new ArgumentException(nameof(child)); }

            if (!Children.Contains(child))
            {
                return;
            }

            child.ParentId = null;
            Children.Remove(child);
        }

        public void AssignToTarget(Target target)
        {
            TagTarget tagTarget = new TagTarget()
            {
                Tag = this,
                Target = target
            };

            TagTargets.Add(tagTarget);
        }

        public void AssignToTarget(IList<Target> targets)
        {
            foreach (Target target in targets)
            {
                AssignToTarget(target);
            }
        }

        public void DismissTarget(Target target)
        {
            TagTarget tagTarget = TagTargets.FirstOrDefault(tt => (tt.Tag == this && tt.Target == target));

            if (tagTarget == null)
            {
                return;
            }

            TagTargets.Remove(tagTarget);
        }

        public void DismissTarget(IList<Target> targets)
        {
            foreach (Target target in targets)
            {
                DismissTarget(target);
            }
        }

        public bool HasTarget(Target queriedTarget)
        {
            if (TaggedTarget.Contains(queriedTarget))
            {
                return true;
            }

            return false;
        }
    }
}
