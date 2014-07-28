TFS.module("L3.WorkItemHistory", [
    "./L3.WorkItemHistory.Controls",
    "TFS.WorkItemTracking.Controls"
], function () {
    var WIControls = TFS.WorkItemTracking.Controls;
    var WIHLogControl = L3.WorkItemHistory.Controls.WIHistoryLogControl;

    WIControls.registerWorkItemControl("WorkItemLogControl", WIHLogControl);
    TFS.tfsModuleLoaded("L3.WorkItemHistory", {});
});
