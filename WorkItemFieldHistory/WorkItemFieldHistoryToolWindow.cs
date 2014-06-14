namespace Lambda3.WorkItemFieldHistory
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TeamFoundation;
    using Lambda3.WorkItemFieldHistory.Views;
    using System.Windows;
    using Lambda3.WorkItemFieldHistory.ViewModels;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsUIElementPane interface.
    /// </summary>
    [Guid("72A23C2B-974A-49AB-B6ED-660CC9BE45E3")]
    public class WorkItemFieldHistoryToolWindow : ToolWindowPane
    {
        private VSExtensionContext ctx;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkItemFieldHistoryToolManagerToolWindow"/> class.
        /// </summary>
        public WorkItemFieldHistoryToolWindow()
            : base(null)
        {
            // Set the window title reading it from the resources.
            this.Caption = Resources.ToolWindowTitle;

            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.
            this.Content = new FieldHistory();
        }

        public void InitializeExtension(TeamFoundationServerExt ext)
        {
            this.ctx = new VSExtensionContext(ext);
            this.ctx.ProjectChanged += this.OnProjectChanged;
            this.ChangeConnection();
        }
            
        private void OnProjectChanged(object sender, EventArgs e)
        {
            this.ChangeConnection();
        }

        private void ChangeConnection()
        {
            if (!string.IsNullOrEmpty(this.ctx.ActiveConnection))
            {
                var collection = new TfsTeamProjectCollection(TfsTeamProjectCollection.GetFullyQualifiedUriForName(this.ctx.ActiveConnection), new TfsClientCredentials());
                collection.EnsureAuthenticated();

                
                (Content as FrameworkElement).DataContext = new FieldHistoryViewModel(new TfsClientRepository(collection));
            }
        }


    }
}
