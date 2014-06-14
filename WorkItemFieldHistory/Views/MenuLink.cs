using EnvDTE;
using Lambda3.WorkItemFieldHistory.Package;
using Microsoft.TeamFoundation.Controls;
using VS = Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Lambda3.WorkItemFieldHistory.Views
{
    [TeamExplorerNavigationLink(GuidList.WorkItemFieldHistoryNavigationLink, TeamExplorerNavigationItemIds.WorkItems, 0)]
    public class MenuLink : ITeamExplorerNavigationLink
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
                var dte2 = VS.Package.GetGlobalService(typeof(DTE)) as EnvDTE80.DTE2;
                if (dte2 != null)
                    dte2.ExecuteCommand("Tools.WorkItemFieldHistory");
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
