using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lambda3.WorkItemFieldHistory
{
    public class RevisionFieldChanged
    {
        public string RevisionNumber { get; set; }

        public string RevisedBy { get; set; }

        public string RevisionDate { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }
    }
}
