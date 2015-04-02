using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Lambda3.WorkItemFieldHistory.Models
{
    public class FieldAtRevision
    {
        public int RevisionNumber { get; set; }

        public string RevisedBy { get; set; }

        public string RevisionDate { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public string FieldName { get; set; }

        public string ReferenceName { get; set; }

        public FieldType FieldType { get; set; }

        public string CompleteName
        {
            get
            {
                return string.Format("{0} ({1})", FieldName, ReferenceName);
            }
        }
    }
}
