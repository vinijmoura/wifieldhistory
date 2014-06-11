namespace Lambda3.WorkItemFieldHistory
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for MainView
    /// </summary>
    public partial class MainView
    {
        private ITfsContext context;
        private TfsClientRepository repository;
        private bool initialized;
        WorkItem wit;

        public MainView()
        {
            try
            {
                this.InitializeComponent();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                this.initialized = false;
            }
            catch (Exception ex)
            {
                this.DisplayError(ex);
            }
        }



        public void InitializeContext(ITfsContext tfsContext)
        {
            this.initialized = false;
            this.context = tfsContext;
            this.context.ProjectChanged += this.OnProjectChanged;
        }

        public void InitializeRepository(TfsClientRepository rep)
        {
            this.repository = rep;
        }

        public void DisplayError(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Work Item Field History", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnProjectChanged(object sender, EventArgs e)
        {
            try
            {
                //this.Reload();
            }
            catch (Exception ex)
            {
                this.DisplayError(ex);
            }
        }



        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!this.initialized)
            {
                this.initialized = true;
            }
        }

        private void btnFieldNames_Click(object sender, RoutedEventArgs e)
        {
            wit = repository.workItemStore.GetWorkItem(Convert.ToInt32(txtWorkItem.Text));
            cmbFieldNames.DisplayMemberPath = "Name";
            cmbFieldNames.ItemsSource = wit.Fields;
            cmbFieldNames.SelectedIndex = 0;   
        }

        private void cmbFieldNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbFieldNames.SelectedItem == null) return;

            var selectedValue = (cmbFieldNames.SelectedItem as Field).Name;

            if (string.IsNullOrEmpty(selectedValue)) return;

            var changedFields = new ObservableCollection<RevisionFieldChanged>();

            foreach (Revision rev in wit.Revisions)
            {
                string changedBy = rev.Fields["Changed By"].Value.ToString();
                string changedDate = rev.Fields["Changed Date"].Value.ToString();

                var field = rev.Fields[selectedValue];

                if (field.IsChangedInRevision)
                {
                    var changedField = new RevisionFieldChanged
                    {
                        RevisionNumber = (rev.Index + 1).ToString(),
                        RevisedBy = changedBy,
                        RevisionDate = changedDate,
                        NewValue = field.Value.ToString(),
                        OldValue = field.OriginalValue != null ? field.OriginalValue.ToString() : ""
                    };
                    changedFields.Add(changedField);
                }
            }
            grdFieldNames.ItemsSource = changedFields;         
        }
    }
}
