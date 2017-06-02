using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Lambda3.WorkItemFieldHistory.Models
{
    public class Field
    {
        public string Name { get; set; }

        public string ReferenceName { get; set; }

        public bool IsChangedInRevision { get; set; }

        public FieldType Type { get;set; }

        public object Value { get; set; }

        public object OriginalValue { get; set; }

        public string CompleteName
        {
            get
            {
                return string.Format("{0} ({1})", Name, ReferenceName);
            }
        }
    }
}
