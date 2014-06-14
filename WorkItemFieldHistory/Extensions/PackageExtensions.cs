using VS = Microsoft.VisualStudio.Shell;

namespace Lambda3.WorkItemFieldHistory.Extensions
{
    static class PackageExtensions
    {
        public static T FindToolWindow<T>(this VS.Package package, int id, bool create) where T : VS.ToolWindowPane
        {
            return package.FindToolWindow(typeof(T), id, create) as T;
        }
    }
}
