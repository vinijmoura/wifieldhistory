namespace Lambda3.WorkItemFieldHistory
{
    using System;
    using Microsoft.VisualStudio.TeamFoundation;


    public class VSExtensionContext : ITfsContext
    {
        private readonly TeamFoundationServerExt ext;
        private string currentConnectionUri;

        public VSExtensionContext(TeamFoundationServerExt ext)
        {
            this.ext = ext;
            if (ext != null)
            {
                this.currentConnectionUri = ext.ActiveProjectContext.DomainUri;
                ext.ProjectContextChanged += this.OnSelectedProjectChanged;
            }
        }

        public event EventHandler ProjectChanged;

        public string SelectedProject
        {
            get { return this.ext.ActiveProjectContext.ProjectName; }
        }

        public string ActiveConnection
        {
            get { return this.ext.ActiveProjectContext.DomainUri; }
        }

        public void OnSelectedProjectChanged(object sender, EventArgs e)
        {
            if (this.ProjectChanged != null && this.ext.ActiveProjectContext.DomainUri != this.currentConnectionUri)
            {
                this.currentConnectionUri = this.ext.ActiveProjectContext.DomainUri;
                this.ProjectChanged(this, new EventArgs());
            }
        }

        
        
    }
}