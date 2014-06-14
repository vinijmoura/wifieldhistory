using Lambda3.WorkItemFieldHistory.Extensions;
using Lambda3.WorkItemFieldHistory.Models;
using Microsoft.TeamFoundation.MVVM;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Lambda3.WorkItemFieldHistory.ViewModels
{
    class FieldHistoryViewModel : INotifyPropertyChanged
    {
        private readonly TfsClientRepository clientRepository;

        private int workItemId;
        private Field selectedField;
        private RevisionHistory revisionHistory;
        private BindingList<FieldAtRevision> fieldChangesHistory;


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

        public RevisionHistory RevisionHistory
        {
            get { return revisionHistory; }
            set { revisionHistory = value; OnPropertyChanged(); }
        }

        public BindingList<FieldAtRevision> FieldChangesHistory
        {
            get { return fieldChangesHistory; }
            set { fieldChangesHistory = value; OnPropertyChanged(); }
        }


        public ICommand ViewFieldsCommand { get; private set; }


        public FieldHistoryViewModel(TfsClientRepository tfsRepository)
        {
            ViewFieldsCommand = new RelayCommand((p) => ViewFieldsOfWorkItem(),
                                                 (p) => WorkItemId > 0);

            this.clientRepository = tfsRepository;
        }


        private void ChangeField()
        {
            if (SelectedField == null)
                return;

            FieldChangesHistory = new BindingList<FieldAtRevision>(RevisionHistory.GetFieldHistory(SelectedField).ToList());
        }

        private void ViewFieldsOfWorkItem()
        {
            try
            {
                RevisionHistory = new RevisionHistory(clientRepository.GetWorkItem(WorkItemId));
                SelectedField = RevisionHistory.Fields.FirstOrDefault();
            }
            catch (Exception error)
            {
                error.Show("Work Item Field Manager");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}