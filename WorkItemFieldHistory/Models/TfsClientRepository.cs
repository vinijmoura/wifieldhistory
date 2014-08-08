using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Threading.Tasks;

namespace Lambda3.WorkItemFieldHistory.Models
{
    public class TfsClientRepository : IDisposable
    {
        private readonly TfsTeamProjectCollection collection;
        private readonly string selectedProject;
        private readonly WorkItemStore workItemStore;

        public TfsClientRepository(TfsTeamProjectCollection collection, string selectedProject)
        {
            this.collection = collection;
            this.selectedProject = selectedProject;
            this.workItemStore = collection.GetService<WorkItemStore>();
        }

        public TeamFoundationIdentity AuthenticatedIdentity
        {
            get { return this.collection.AuthorizedIdentity; }
        }

        public Task<WorkItem> GetWorkItem(int id)
        {
            return Task.Run(() => workItemStore.GetWorkItem(id));
        }

        public WorkItemStore WorkItemStore
        {
            get { return workItemStore; }
        }

        public string SelectedProject
        {
            get { return selectedProject; }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!!disposing && this.collection != null)
                this.collection.Dispose();
        }
    }
}
