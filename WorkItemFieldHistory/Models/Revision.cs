using System.Collections.Generic;

namespace Lambda3.WorkItemFieldHistory.Models
{
    public class Revision
    {
        public int Index { get; set; }
        public IDictionary<string, Field> Fields { get; set; }

        public FieldAtRevision GetField(string fieldName)
        {
            return new FieldAtRevision
            {
                RevisionNumber = Index + 1,
                RevisedBy = GetFieldValue("Changed By"),
                RevisionDate = GetFieldValue("Changed Date"),
                NewValue = GetFieldValue(fieldName),
                OldValue = GetFieldOriginalValue(fieldName),
                FieldName = fieldName
            };
        }

        private string GetFieldValue(string fieldName)
        {
            return (Fields[fieldName].Value ?? string.Empty).ToString();
        }

        private string GetFieldOriginalValue(string fieldName)
        {
            return (Fields[fieldName].OriginalValue ?? string.Empty).ToString();
        }
    }
}
