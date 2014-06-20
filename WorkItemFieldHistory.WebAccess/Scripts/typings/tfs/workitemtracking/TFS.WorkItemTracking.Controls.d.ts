
declare module TFS.WorkItemTracking.Controls {
    function registerWorkItemControl(controlName: string, controlType: {}, requiredModule?: any);
}

declare module "WorkItemTracking/Scripts/TFS.WorkItemTracking.Controls" {
    function registerWorkItemControl(controlName: string, controlType: {}, requiredModule?: any);
    function fixLinkTargets(targets): void

    class WorkItemLogControl {
        _workItem: any;
        _init(): void;
        _loadTab(index: number): void;
        _tabHost: JQuery;
        _reset(): void;
    }

    class WorkItemField {
        fieldDefinition;
    }
}

//declare module "Presentation/Scripts/TFS/TFS.UI" {
//}
