using EnvDTE;
using Lambda3.WorkItemFieldHistory.Extensions;
using Lambda3.WorkItemFieldHistory.Package;
using Microsoft.TeamFoundation.Controls;
using VS = Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using 


namespace Lambda3.WorkItemFieldHistory.Views
{
    [TeamExplorerNavigationLink(GuidList.MENU_ITEM, TeamExplorerNavigationItemIds.WorkItems, 0)]
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
                EnvDTE.DTE dte = VS.Package.GetGlobalService(typeof(DTE)) as EnvDTE.DTE;
                

                if (dte != null)
                {
                    dte.ExecuteCommand("Tools.WorkItemFieldHistory");
                }
            }
            catch (Exception error)
            {
                error.Show("Work Item Field History");
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
