using System;
using System.Collections.Generic;
using System.Linq;

namespace Lambda3.WorkItemFieldHistory.Models
{
    public class RevisionHistory 
    {
        private readonly WorkItem workItem;

        public IEnumerable<Field> Fields
        {
            get 
            {
                return workItem.Fields.OrderBy(field => field.Name); 
            }
        }

        public WorkItem WorkItem { get { return workItem; } }

        public RevisionHistory(WorkItem workItem)
        {
            this.workItem = workItem;
        }

        public IEnumerable<FieldAtRevision> GetFieldHistory(Field field)
        {
            return workItem.Revisions
                  .Where(revision => revision.Fields[field.Name].IsChangedInRevision)
                  .Select(revision => revision.GetField(field.Name))
                  .OrderByDescending(revision => Convert.ToDateTime(revision.RevisionDate));
        }

        public IEnumerable<FieldAtRevision> GetAllFieldHistory()
        {
            return Fields.SelectMany(field => GetFieldHistory(field));
        }
    }
}
