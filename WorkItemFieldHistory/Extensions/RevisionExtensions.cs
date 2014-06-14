using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lambda3.WorkItemFieldHistory.Extensions
{
    static class RevisionExtensions
    {
        public static string GetFieldValue(this Revision revision, string fieldName)
        {
            return (revision.Fields[fieldName].Value ?? string.Empty).ToString();
        }

        public static string GetFieldOriginalValue(this Revision revision, string fieldName)
        {
            return (revision.Fields[fieldName].OriginalValue ?? string.Empty).ToString();
        }
    }
}
