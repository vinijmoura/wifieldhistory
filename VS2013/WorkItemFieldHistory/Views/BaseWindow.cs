﻿using Lambda3.WorkItemFieldHistory.Models;
using Lambda3.WorkItemFieldHistory.Package;
using Lambda3.WorkItemFieldHistory.ViewModels;
using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Lambda3.WorkItemFieldHistory.Views
{
    [Guid(GuidList.WINDOW)]
    public class BaseWindow : ToolWindowPane
    {
        private VSExtensionContext extensionContext;

        public BaseWindow()
            : base(null)
        {
            Caption = Resources.ToolWindowTitle;
            BitmapResourceID = 301;
            BitmapIndex = 1;
            Content = new FieldHistoryView();
        }

        public void InitializeExtension(TeamFoundationServerExt ext)
        {
            extensionContext = new VSExtensionContext(ext);
            extensionContext.ProjectChanged += (sender, e) => ChangeConnection();
            ChangeConnection();
        }

        private void ChangeConnection()
        {
            if (!string.IsNullOrEmpty(extensionContext.ActiveConnection))
            {
                var uri = TfsTeamProjectCollection.GetFullyQualifiedUriForName(extensionContext.ActiveConnection);
                var collection = new TfsTeamProjectCollection(uri, new TfsClientCredentials());
                collection.EnsureAuthenticated();

                (Content as FrameworkElement).DataContext = new FieldHistoryViewModel(new TfsClientRepository(collection, extensionContext.SelectedProject));
            }
        }
    }
}
