using Lambda3.WorkItemFieldHistory.Models;
using Microsoft.TeamFoundation.MVVM;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Lambda3.WorkItemFieldHistory.ViewModels
{
    class FieldHistoryViewModel : INotifyPropertyChanged
    {
        private TfsClientRepository clientRepository;

        private int workItemId;
        private Field selectedField;
        private WorkItem workItem;
        private ObservableCollection<RevisionFieldChanged> fieldRevisions;

        public int WorkItemId
        {
            get { return workItemId; }
            set { workItemId = value; OnPropertyChanged(); }
        }

        public Field SelectedField
        {
            get { return selectedField; }
            set { selectedField = value; OnPropertyChanged(); ChangeField(); }
        }

        public WorkItem WorkItem
        {
            get { return workItem; }
            set { workItem = value; OnPropertyChanged(); }
        }

        public ObservableCollection<RevisionFieldChanged> FieldRevisions
        {
            get { return fieldRevisions; }
            set { fieldRevisions = value; OnPropertyChanged(); }
        }

        public ICommand ViewFieldsCommand { get; set; }

        public FieldHistoryViewModel(TfsClientRepository tfsRepository)
        {
            ViewFieldsCommand = new RelayCommand(ViewFieldsCommand_Execute);

            this.clientRepository = tfsRepository;
        }

        public void ChangeField()
        {
            if (SelectedField == null)
                return;

             var revisions = WorkItem.Revisions
                 .Cast<Revision>()
                 .Where(revision => revision.Fields[SelectedField.Name].IsChangedInRevision)
                 .Select(revision => new RevisionFieldChanged
                    {
                        RevisionNumber = (revision.Index + 1).ToString(),
                        RevisedBy = revision.Fields["Changed By"].Value.ToString(),
                        RevisionDate = revision.Fields["Changed Date"].Value.ToString(),
                        NewValue = revision.Fields[SelectedField.Name].Value.ToString(),
                        OldValue = (revision.Fields[SelectedField.Name].OriginalValue ?? String.Empty).ToString()
                    });

             FieldRevisions = new ObservableCollection<RevisionFieldChanged>(revisions);
        }
        
        public void ViewFieldsCommand_Execute()
        {
            WorkItem = clientRepository.GetWorkItem(WorkItemId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}