namespace Lambda3.WorkItemFieldHistory.Models
{
    public class Field
    {
        public string Name { get; set; }
        public bool IsChangedInRevision { get; set; }

        public object Value { get; set; }
        public object OriginalValue { get; set; }
    }
}
