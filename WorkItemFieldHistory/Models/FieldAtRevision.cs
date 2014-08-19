namespace Lambda3.WorkItemFieldHistory.Models
{
    public class FieldAtRevision
    {
        public int RevisionNumber { get; set; }

        public string RevisedBy { get; set; }

        public string RevisionDate { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public string FieldName { get; set;}
    }
}
