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
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // This attribute is used to register the informations needed to show the this package in the Help/About dialog of Visual Studio.
    [ProvideMenuResource("Menus.ctmenu", 1)] // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideToolWindow(typeof(WorkItemFieldHistoryToolWindow), Transient = true, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Bottom)] // This attribute registers a tool window exposed by this package.
    [Guid(GuidList.guidWorkItemFieldHistoryPkgString)]
    public sealed class WorkItemFieldHistoryPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public WorkItemFieldHistoryPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
            base.Initialize();

            try
            {
                // Add our command handlers for menu (commands must exist in the .vsct file)
                OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
                if (null != mcs)
                {
                    // Create the command for the menu item.
                    CommandID menuCommandID = new CommandID(GuidList.guidWorkItemFieldHistoryCmdSet, (int)PkgCmdIDList.CmdidTestCommand);
                    MenuCommand menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                    mcs.AddCommand(menuItem);

                    // Create the command for the tool window
                    CommandID toolwndCommandID = new CommandID(GuidList.guidWorkItemFieldHistoryCmdSet, (int)PkgCmdIDList.CmdidTestTool);
                    MenuCommand menuToolWin = new MenuCommand(this.ShowToolWindow, toolwndCommandID);
                    mcs.AddCommand(menuToolWin);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }

        }

        private static TeamFoundationServerExt GetTeamFoundationServerExt()
        {
            var dte = GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            return dte.GetObject("Microsoft.VisualStudio.TeamFoundation.TeamFoundationServerExt") as TeamFoundationServerExt;
        }
        private static void ShowErrorMessage(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Work Item Field Manager", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            try
            {
                var ext = GetTeamFoundationServerExt();

                // Get the instance number 0 of this tool window. This window is single instance so this instance is actually the only one.
                // The last flag is set to true so that if the tool window does not exists it will be created.
                ToolWindowPane window = this.FindToolWindow(typeof(WorkItemFieldHistoryToolWindow), 0, true);
                if ((null == window) || (null == window.Frame))
                {
                    throw new NotSupportedException(Resources.CanNotCreateWindow);
                }

                var wnd = window as WorkItemFieldHistoryToolWindow;
                wnd.InitializeExtension(ext);
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

    }
}
