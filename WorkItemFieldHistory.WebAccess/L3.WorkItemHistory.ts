
TFS.module("L3.WorkItemHistory",
    [
        "./L3.WorkItemHistory.Controls",
        "TFS.WorkItemTracking.Controls",
    ],
    function () {
        var WIControls = TFS.WorkItemTracking.Controls;
        var WIHLogControl = L3.WorkItemHistory.Controls.WIHistoryLogControl;

        //Registra o componente com o mesmo nome para trocar a implementação pela do WI Field History
        WIControls.registerWorkItemControl("WorkItemLogControl", WIHLogControl);
        TFS.tfsModuleLoaded("L3.WorkItemHistory", {});
    });
