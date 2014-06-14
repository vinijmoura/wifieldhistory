using Microsoft.VisualStudio.TeamFoundation;
using System;

namespace Lambda3.WorkItemFieldHistory.Package
{
    public class VSExtensionContext
    {
        public event EventHandler ProjectChanged;
        private readonly TeamFoundationServerExt extension;
        private string currentConnectionUri;

        public VSExtensionContext(TeamFoundationServerExt extension)
        {
            this.extension = extension;
            if (extension != null)
            {
                currentConnectionUri = extension.ActiveProjectContext.DomainUri;
                extension.ProjectContextChanged += OnSelectedProjectChanged;
            }
        }

        public string SelectedProject
        {
            get { return extension.ActiveProjectContext.ProjectName; }
        }

        public string ActiveConnection
        {
            get { return extension.ActiveProjectContext.DomainUri; }
        }

        public void OnSelectedProjectChanged(object sender, EventArgs e)
        {
            if (ProjectChanged != null && extension.ActiveProjectContext.DomainUri != currentConnectionUri)
            {
                currentConnectionUri = extension.ActiveProjectContext.DomainUri;
                ProjectChanged(this, new EventArgs());
            }
        }
    }
}