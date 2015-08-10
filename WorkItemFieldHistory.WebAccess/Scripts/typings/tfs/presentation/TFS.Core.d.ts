
declare module "Presentation/Scripts/TFS/TFS.Core" {
    function delegate(instance, method, data?: any): any;
    class DateUtils {
        static localeFormat(date, format: string): string;
    }
    class ArrayUtils {
        static sortIfNotSorted(arr: any[], comparer: any): void;
    }
    class StringUtils {
        static localeIgnoreCaseComparer;
    }
}