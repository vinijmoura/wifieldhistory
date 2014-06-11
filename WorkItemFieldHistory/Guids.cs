// Guids.cs
// MUST match guids.h
using System;

namespace Lambda3.WorkItemFieldHistory
{
    static class GuidList
    {
        public const string guidWorkItemFieldHistoryPkgString = "1E622EED-10BF-4780-B558-2090E97ED7D2";

        public const string guidWorkItemFieldHistoryCmdSetString = "6027F516-BADA-43B5-9287-50D15EC4A60C";

        public static readonly Guid guidWorkItemFieldHistoryCmdSet = new Guid(guidWorkItemFieldHistoryCmdSetString);

        public const string WorkItemFieldHistoryNavigationLink = "99879CF2-1BAD-48B3-9F0B-C636B3B0E49A";

    };
}