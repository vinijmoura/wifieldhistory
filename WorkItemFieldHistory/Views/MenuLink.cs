using System;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.TeamFoundation.Controls;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace Lambda3.WorkItemFieldHistory
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
                var dte2 = Package.GetGlobalService(typeof(DTE)) as EnvDTE80.DTE2;
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
