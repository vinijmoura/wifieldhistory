using Lambda3.WorkItemFieldHistory.Extensions;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

namespace Lambda3.WorkItemFieldHistory.Models
{
    public class RevisionHistory 
    {
        private readonly WorkItem workItem;

        public IEnumerable<Field> Fields
        {
            get 
            {
                return workItem.Fields
                    .Cast<Field>()
                    .OrderBy(field => field.Name); 
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
                  .Cast<Revision>()
                  .Where(revision => revision.Fields[field.Name].IsChangedInRevision)
                  .Select(revision => new FieldAtRevision
                  {
                      RevisionNumber = revision.Index + 1,
                      RevisedBy = revision.GetFieldValue("Changed By"),
                      RevisionDate = revision.GetFieldValue("Changed Date"),
                      NewValue = revision.GetFieldValue(field.Name),
                      OldValue = revision.GetFieldOriginalValue(field.Name),
                      FieldName = field.Name,
                  })
                  .OrderByDescending(revision => Convert.ToDateTime(revision.RevisionDate));
        }
        public IEnumerable<FieldAtRevision> GetAllFieldHistory()
        {
            return Fields.SelectMany(field => GetFieldHistory(field));
        }
    }
}
