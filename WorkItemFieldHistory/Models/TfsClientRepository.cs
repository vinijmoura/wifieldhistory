using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;

namespace Lambda3.WorkItemFieldHistory.Models
{
    public class TfsClientRepository : IDisposable
    {
        private readonly TfsTeamProjectCollection collection;
        private readonly WorkItemStore workItemStore;

        public TfsClientRepository(TfsTeamProjectCollection collection)
        {
            this.collection = collection;
            this.workItemStore = collection.GetService<WorkItemStore>();
        }

        public TeamFoundationIdentity AuthenticatedIdentity
        {
            get { return this.collection.AuthorizedIdentity; }
        }

        public WorkItem GetWorkItem(int id)
        {
            return workItemStore.GetWorkItem(id);
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
