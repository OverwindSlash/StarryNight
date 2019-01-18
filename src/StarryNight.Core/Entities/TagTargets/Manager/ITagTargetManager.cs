using StarryNight.Entities.TagTargets;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarryNight.Entities
{
    public interface ITagTargetManager
    {
        TagTargetOpResult AssignTag(Target target, Tag tagToAssign);
        TagTargetOpResult AssignTag(IList<Target> targets, Tag tagToAssign);
        TagTargetOpResult AssignTag(Target target, IList<Tag> tagsToAssign);
        TagTargetOpResult AssignTag(IList<Target> targets, IList<Tag> tagsToAssign);


        TagTargetOpResult RemoveTag(Target target, Tag tagToRemove);
        TagTargetOpResult RemoveTag(IList<Target> targets, Tag tagToRemove);
        TagTargetOpResult RemoveTag(Target target, IList<Tag> tagsToRemove);
        TagTargetOpResult RemoveTag(IList<Target> targets, IList<Tag> tagsToRemove);
    }
}
