using System.Collections.Generic;

namespace Lambda3.WorkItemFieldHistory.Models
{
    public class WorkItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string TypeName { get; set; }

        public IEnumerable<Revision> Revisions { get; set; }

        public IEnumerable<Field> Fields { get; set; }
    }
}
