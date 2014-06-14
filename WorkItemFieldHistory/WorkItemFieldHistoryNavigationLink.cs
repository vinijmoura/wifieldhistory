namespace Lambda3.WorkItemFieldHistory
{
    using System;
    using System.Windows.Forms;
    using EnvDTE;
    using Microsoft.TeamFoundation.Controls;

    [TeamExplorerNavigationLink(GuidList.WorkItemFieldHistoryNavigationLink, TeamExplorerNavigationItemIds.WorkItems, 0)]
    public class WorkItemFieldHistoryNavigationLink : ITeamExplorerNavigationLink
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public bool IsEnabled
        {
            get { return true; }
        }

        public bool IsVisible
        {
            get { return true; }
        }

        public string Text
        {
            get { return "Work Item Field History"; }
        }

        public void Execute()
        {
            try
            {
                EnvDTE80.DTE2 dte2 = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as EnvDTE80.DTE2;
                if (dte2 != null)
                {
                    dte2.ExecuteCommand("Tools.WorkItemFieldHistory");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Invalidate()
        {
        }

        public void Dispose()
        {
        }
    }
}
