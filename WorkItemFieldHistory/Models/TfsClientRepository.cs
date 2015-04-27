using Lambda3.WorkItemFieldHistory.Extensions;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using System;
using System.Threading.Tasks;
using TFS = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Lambda3.WorkItemFieldHistory.Models
{
    public class TfsClientRepository : IDisposable
    {
        private readonly TfsTeamProjectCollection collection;
        private readonly TFS.WorkItemStore workItemStore;
        private readonly string selectedProject;

        public TfsClientRepository(TfsTeamProjectCollection collection, string selectedProject)
        {
            this.collection = collection;
            this.selectedProject = selectedProject;
            workItemStore = collection.GetService<TFS.WorkItemStore>();
        }

        public TeamFoundationIdentity AuthenticatedIdentity
        {
            get { return this.collection.AuthorizedIdentity; }
        }

        public async Task<WorkItem> GetWorkItem(int id)
        {
            return await Task.Run(() => workItemStore.GetWorkItem(id).MapToModel());
        }

        public TFS.WorkItemStore WorkItemStore
        {
            get { return workItemStore; }
        }

        public string SelectedProject
        {
            get { return selectedProject; }
        }

        public TfsTeamProjectCollection Collection
        {
            get { return collection;}
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing && collection != null)
                collection.Dispose();
        }
    }
}
