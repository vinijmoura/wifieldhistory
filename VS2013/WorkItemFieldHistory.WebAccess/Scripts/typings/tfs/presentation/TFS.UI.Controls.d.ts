
declare module "Presentation/Scripts/TFS/TFS.UI.Controls" {
    function getId(): number;

    class BaseControl {
        static createIn(control:any, container:any, func:any): any;
    }

}