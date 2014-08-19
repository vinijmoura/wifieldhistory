using System.Windows;
using System.Windows.Forms;
using Lambda3.WorkItemFieldHistory.Extensions;
using Lambda3.WorkItemFieldHistory.Models;
using Lambda3.WorkItemFieldHistory.Package;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.MVVM;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using TShooter.TeamFoundation.Dialogs;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lambda3.WorkItemFieldHistory.ViewModels
{
    class FieldHistoryViewModel : INotifyPropertyChanged
    {
        private readonly TfsClientRepository clientRepository;

        private int workItemId;
        private bool isBusy;
        private RevisionHistory revisionHistory;
        private IEnumerable fieldChangesHistory;
        private object selectedField;


        public int WorkItemId
        {
            get { return workItemId; }
            set { workItemId = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
        }

        public RevisionHistory RevisionHistory
        {
            get { return revisionHistory; }
            set { revisionHistory = value; OnPropertyChanged(); }
        }

        public IEnumerable FieldChangesHistory
        {
            get { return fieldChangesHistory; }
            set { fieldChangesHistory = value; OnPropertyChanged(); }
        }

        public object SelectedField
        {
            get { return selectedField; }
            set { selectedField = value; OnPropertyChanged(); ChangeField(); }
        }


        public ICommand ViewFieldsCommand { get; private set; }

        public ICommand PickWorkItemCommand { get; private set; }


        public FieldHistoryViewModel(TfsClientRepository tfsRepository)
        {
            PickWorkItemCommand = new RelayCommand(async p => await PickWorkItem(),
                                                   p => !IsBusy);
            ViewFieldsCommand = new RelayCommand(async p => await ViewFieldsOfWorkItem(),
                                                 p => WorkItemId > 0 && !IsBusy);

            clientRepository = tfsRepository;
        }

        private void ChangeField()
        {
            if (RevisionHistory == null)
                return;

            if (SelectedField is Field)
            {
                FieldChangesHistory = RevisionHistory.GetFieldHistory(SelectedField as Field).ToList();
                return;
            }

            FieldChangesHistory = GroupAllFieldsBy(field => field.FieldName);
        }

        private async Task ViewFieldsOfWorkItem()
        {
            IsBusy = true;

            try
            {
                RevisionHistory = new RevisionHistory(await clientRepository.GetWorkItem(WorkItemId));
                SelectedField = null;
            }
            catch (Exception error)
            {
                error.Show();
            }

            IsBusy = false;
        }

        private async Task PickWorkItem()
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
                    if (dlg.ShowDialog(windowWrapper) != DialogResult.OK)
                        return;

                    WorkItemId = dlg.SelectedWorkItems.First().Id;
                    await ViewFieldsOfWorkItem();
                }
            }
            catch (Exception ex)
            {
                ex.Show();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ListCollectionView GroupAllFieldsBy(Expression<Func<FieldAtRevision, object>> key)
        {
            var propertyName = (key.Body as MemberExpression).Member.Name;

            var allFields = new ListCollectionView(RevisionHistory.GetAllFieldHistory().ToList());
            allFields.GroupDescriptions.Add(new PropertyGroupDescription(propertyName));

            return allFields;
        }
    }
}