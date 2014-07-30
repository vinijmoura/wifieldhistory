using System.Windows;
using System.Windows.Forms;
using Lambda3.WorkItemFieldHistory.Extensions;
using Lambda3.WorkItemFieldHistory.Models;
using Lambda3.WorkItemFieldHistory.Package;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.MVVM;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using TShooter.TeamFoundation.Dialogs;

namespace Lambda3.WorkItemFieldHistory.ViewModels
{
    class FieldHistoryViewModel : INotifyPropertyChanged
    {
        private readonly TfsClientRepository clientRepository;

        private int workItemId;
        private RevisionHistory revisionHistory;
        private ICollectionView fieldChangesHistory;


        public int WorkItemId
        {
            get { return workItemId; }
            set { workItemId = value; OnPropertyChanged(); }
        }

        public RevisionHistory RevisionHistory
        {
            get { return revisionHistory; }
            set { revisionHistory = value; OnPropertyChanged(); }
        }

        public ICollectionView FieldChangesHistory
        {
            get { return fieldChangesHistory; }
            set { fieldChangesHistory = value; OnPropertyChanged(); }
        }


        public ICommand ViewFieldsCommand { get; private set; }

        public ICommand PickWorkItemCommand { get; private set; }


        public FieldHistoryViewModel(TfsClientRepository tfsRepository)
        {
            ViewFieldsCommand = new RelayCommand((p) => ViewFieldsOfWorkItem(),
                                                 (p) => WorkItemId > 0);

            PickWorkItemCommand = new RelayCommand((p) => PickWorkItem());

            this.clientRepository = tfsRepository;
        }

        private void ViewFieldsOfWorkItem()
        {
            try
            {
                RevisionHistory = new RevisionHistory(clientRepository.GetWorkItem(WorkItemId));
                FieldChangesHistory = new ListCollectionView(RevisionHistory.GetAllFieldHistory().ToList());
                FieldChangesHistory.GroupDescriptions.Add(new PropertyGroupDescription("FieldName"));
            }
            catch (Exception error)
            {
                error.Show("Work Item Field History");
            }
        }

        private void PickWorkItem()
        {
            try
            {
                using (var dlg = new WorkItemPickerDialog(clientRepository.WorkItemStore)
                {
                    MultiSelect = false,
                    TeamProjectName = clientRepository.SelectedProject
                })
                {
                    var windowWrapper = new Win32WindowWrapper(new IntPtr(WorkItemFieldHistoryPackage.DTE.Application.MainWindow.HWnd));
                    if (dlg.ShowDialog(windowWrapper) != DialogResult.OK) return;

                    WorkItemId = dlg.SelectedWorkItems[0].Id;
                    ViewFieldsOfWorkItem();
                }
            }
            catch (Exception ex)
            {
                ex.Show("Work Item Field History");
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