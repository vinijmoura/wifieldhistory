using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation;
using System.Windows.Forms;

namespace Lambda3.WorkItemFieldHistory
{
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidWorkItemFieldHistoryPkgString)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideToolWindow(typeof(WorkItemFieldHistoryToolWindow), Transient = true, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Bottom)]
    public sealed class WorkItemFieldHistoryPackage : Package
    {
        private const string EXTENSION_CLASS = "Microsoft.VisualStudio.TeamFoundation.TeamFoundationServerExt";
        private const int TEST_COMMAND_ID = 0x100;
        private const int TEST_TOOL_ID = 0x101;


        protected override void Initialize()
        {
            base.Initialize();

            try
            {
                var commandService = GetService(typeof(IMenuCommandService)) as IMenuCommandService;
                if (commandService is OleMenuCommandService)
                {
                    var menuCommandID = new CommandID(GuidList.guidWorkItemFieldHistoryCmdSet, TEST_COMMAND_ID);
                    commandService.AddCommand(new MenuCommand(this.MenuItemCallback, menuCommandID));

                    var toolwndCommandID = new CommandID(GuidList.guidWorkItemFieldHistoryCmdSet, TEST_TOOL_ID);
                    commandService.AddCommand(new MenuCommand(this.ShowToolWindow, toolwndCommandID));
                }
            }
            catch (Exception error)
            {
                ShowErrorMessage(error);
            }

        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            try
            {
                var extension = GetTeamFoundationServerExt();
                var window = FindToolWindow(typeof(WorkItemFieldHistoryToolWindow), 0, true);

                if ((null == window) || (null == window.Frame))
                    throw new NotSupportedException(Resources.CanNotCreateWindow);

                var wnd = window as WorkItemFieldHistoryToolWindow;
                wnd.InitializeExtension(extension);
                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;

                Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            this.ShowToolWindow(this, new EventArgs());
        }


        private TeamFoundationServerExt GetTeamFoundationServerExt()
        {
            var dte = GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            return dte.GetObject(EXTENSION_CLASS) as TeamFoundationServerExt;
        }

        private void ShowErrorMessage(Exception error)
        {
            MessageBox.Show(error.Message,
                            "Work Item Field Manager",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
    }
}
