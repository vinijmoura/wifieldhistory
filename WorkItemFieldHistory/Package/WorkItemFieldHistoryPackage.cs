using Lambda3.WorkItemFieldHistory.Extensions;
using Lambda3.WorkItemFieldHistory.Views;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Lambda3.WorkItemFieldHistory.Package
{
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.PACKAGE)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideToolWindow(typeof(BaseWindow), Transient = true, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Bottom)]
    public sealed class WorkItemFieldHistoryPackage : Microsoft.VisualStudio.Shell.Package
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
                    var menuCommandID = new CommandID(new Guid(GuidList.COMMAND_SET), TEST_COMMAND_ID);
                    commandService.AddCommand(new MenuCommand((s, e) => ShowToolWindow(), menuCommandID));

                    var toolwndCommandID = new CommandID(new Guid(GuidList.COMMAND_SET), TEST_TOOL_ID);
                    commandService.AddCommand(new MenuCommand((s, e) => ShowToolWindow(), toolwndCommandID));
                }
            }
            catch (Exception error)
            {
                error.Show("Work Item Field History");
            }
        }

        private void ShowToolWindow()
        {
            try
            {
                var window = this.FindToolWindow<BaseWindow>(0, true);

                if ((null == window) || (null == window.Frame))
                    throw new NotSupportedException(Resources.CanNotCreateWindow);

                window.InitializeExtension(GetExtension());
                ErrorHandler.ThrowOnFailure((window.Frame as IVsWindowFrame).Show());
            }
            catch (Exception error)
            {
                error.Show("Work Item Field History");
            }
        }

        private TeamFoundationServerExt GetExtension()
        {
            var dte = GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            return dte.GetObject(EXTENSION_CLASS) as TeamFoundationServerExt;
        }
    }
}