using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using StarryNight.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarryNight.Entities.TagTargets
{
    public class TagTargetManager : DomainService, ITagTargetManager
    {
        private readonly ITagRepository tagRepository;
        private readonly ITargetRepository targetRepository;
        private readonly ITagTargetRepository tagTargetRepository;

        public TagTargetManager(
            ITagRepository tagRepository,
            ITargetRepository targetRepository,
            ITagTargetRepository tagTargetRepository)
        {
            this.tagRepository = tagRepository;
            this.targetRepository = targetRepository;
            this.tagTargetRepository = tagTargetRepository;
        }

        public TagTargetOpResult AssignTag(Target target, Tag tagToAssign)
        {
            IList<Target> targetsParam = new List<Target>() { target };
            IList<Tag> tagsParam = new List<Tag>() { tagToAssign };
            return PerformTagOperation(targetsParam, tagsParam, true);
        }

        public TagTargetOpResult AssignTag(IList<Target> targets, Tag tagToAssign)
        {
            IList<Target> targetsParam = targets;
            IList<Tag> tagsParam = new List<Tag>() { tagToAssign };
            return PerformTagOperation(targetsParam, tagsParam, true);
        }

        public TagTargetOpResult AssignTag(Target target, IList<Tag> tagsToAssign)
        {
            IList<Target> targetsParam = new List<Target>() { target };
            IList<Tag> tagsParam = tagsToAssign;
            return PerformTagOperation(targetsParam, tagsParam, true);
        }

        public TagTargetOpResult AssignTag(IList<Target> targets, IList<Tag> tagsToAssign)
        {
            IList<Target> targetsParam = targets;
            IList<Tag> tagsParam = tagsToAssign;
            return PerformTagOperation(targetsParam, tagsParam, true);
        }

        public TagTargetOpResult RemoveTag(Target target, Tag tagToRemove)
        {
            IList<Target> targetsParam = new List<Target>() { target };
            IList<Tag> tagsParam = new List<Tag>() { tagToRemove };
            return PerformTagOperation(targetsParam, tagsParam, false);
        }

        public TagTargetOpResult RemoveTag(IList<Target> targets, Tag tagToRemove)
        {
            IList<Target> targetsParam = targets;
            IList<Tag> tagsParam = new List<Tag>() { tagToRemove };
            return PerformTagOperation(targetsParam, tagsParam, false);
        }

        public TagTargetOpResult RemoveTag(Target target, IList<Tag> tagsToRemove)
        {
            IList<Target> targetsParam = new List<Target>() { target };
            IList<Tag> tagsParam = tagsToRemove;
            return PerformTagOperation(targetsParam, tagsParam, false);
        }

        public TagTargetOpResult RemoveTag(IList<Target> targets, IList<Tag> tagsToRemove)
        {
            IList<Target> targetsParam = targets;
            IList<Tag> tagsParam = tagsToRemove;
            return PerformTagOperation(targetsParam, tagsParam, false);
        }

        protected virtual TagTargetOpResult PerformTagOperation(IList<Target> targets, IList<Tag> tags, bool IsAssignOperation = true)
        {
            IList<Target> resultTargets = new List<Target>();
            IList<Tag> resultTags = new List<Tag>();

            foreach (var target in targets)
            {
                //Target queriedTarget = targetRepository.GetAll().Include("TagTargets.Tag").FirstOrDefault(t => t.Id == target.Id);
                Target queriedTarget = targetRepository.FirstOrDefault(t => t.Id == target.Id);
                //ICollection<Tag> assignedTags = queriedTarget.AssignedTags;

                foreach (var tag in tags)
                {
                    //Tag queriedTag = tagRepository.GetAll().Include("TagTargets.Target").FirstOrDefault(t => t.Id == tag.Id);
                    Tag queriedTag = tagRepository.FirstOrDefault(t => t.Id == tag.Id);
                    //ICollection<Target> taggedTargets = queriedTag.TaggedTargets;

                    if (IsAssignOperation)
                    {
                        if (queriedTag.HasTarget(queriedTarget))
                        {
                            continue;
                        }

                        //queriedTarget.AppendTag(queriedTag);
                        queriedTag.AssignToTarget(queriedTarget);
                    }
                    else
                    {
                        if (!queriedTarget.HasTag(queriedTag))
                        {
                            continue;
                        }

                        //queriedTarget.RemoveTag(queriedTag);
                        queriedTag.DismissTarget(queriedTarget);
                    }

                    tagRepository.Update(queriedTag);
                    if (!resultTags.Contains(queriedTag))
                    {
                        resultTags.Add(queriedTag);
                    }
                }

                //targetRepository.Update(queriedTarget);
                if (!resultTargets.Contains(queriedTarget))
                {
                    resultTargets.Add(queriedTarget);
                }
            }

            return new TagTargetOpResult()
            {
                Targets = resultTargets,
                Tags = resultTags
            };
        }
    }
}
