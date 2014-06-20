
declare module "Presentation/Scripts/TFS/TFS" {
    function tfsModuleLoaded(moduleName: string, moduleExports): void;
}

declare module TFS {
    function module(moduleName: string, dependencies: string[], func: (dep1?: any, ...deps: any[]) => void): void;
    function tfsModuleLoaded(moduleName: string, moduleExports): void;
}

declare var exports: any