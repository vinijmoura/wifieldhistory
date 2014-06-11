namespace Lambda3.WorkItemFieldHistory
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Framework.Client;
    using Microsoft.TeamFoundation.Server;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;


    /// <summary>
    /// The TFS client repository.
    /// </summary>
    public class TfsClientRepository : IDisposable
    {

        private TfsTeamProjectCollection collection;
        public WorkItemStore workItemStore { get; set; }

        public TfsClientRepository(TfsTeamProjectCollection collection)
        {
            this.collection = collection;
            this.workItemStore = this.collection.GetService<WorkItemStore>();
        }

        public TeamFoundationIdentity AuthenticatedIdentity
        {
            get { return this.collection.AuthorizedIdentity; }
        }

        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (this.collection != null)
                {
                    this.collection.Dispose();
                    this.collection = null;
                }
            }
        }


    }


}
