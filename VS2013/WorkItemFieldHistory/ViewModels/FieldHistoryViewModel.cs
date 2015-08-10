using EnvDTE;
using Lambda3.WorkItemFieldHistory.Extensions;
using Lambda3.WorkItemFieldHistory.Models;
using Lambda3.WorkItemFieldHistory.Package;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.MVVM;
using Microsoft.VisualStudio.TeamFoundation.WorkItemTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using TShooter.TeamFoundation.Dialogs;
using VS = Microsoft.VisualStudio.Shell;
using System.IO;

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
        private const string DOCUMENTSERVICE_CLASS = "Microsoft.VisualStudio.TeamFoundation.WorkItemTracking.DocumentService";


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
            set { selectedField = value; OnPropertyChanged(); ChangeField(); 
            }
        }


        public ICommand ViewFieldsCommand { get; private set; }

        public ICommand PickWorkItemCommand { get; private set; }

        public ICommand GoToWorkItemCommand { get; private set; }

        public ICommand ReportWorkItemCommand { get; private set; }

        public FieldHistoryViewModel(TfsClientRepository tfsRepository)
        {
            PickWorkItemCommand = new RelayCommand(async p => await PickWorkItem(),
                                                   p => !IsBusy);
            ViewFieldsCommand = new RelayCommand(async p => await ViewFieldsOfWorkItem(),
                                                 p => WorkItemId > 0 && !IsBusy);

            GoToWorkItemCommand = new RelayCommand(p => GoToWorkItem(),
                                                 p => WorkItemId > 0 && !isBusy);

            ReportWorkItemCommand = new RelayCommand(p => ReportWorkItem(),
                                     p => WorkItemId > 0 && !isBusy);

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

            FieldChangesHistory = GroupAllFieldsBy(field => field.CompleteName);
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

        private void ReportWorkItem()
        {
            IsBusy = true;

            try
            {
                FieldChangesHistory = GroupAllFieldsBy(field => field.CompleteName);
                var excel = new Microsoft.Office.Interop.Excel.Application();
                // for making Excel visible
                excel.Visible = true;
                excel.DisplayAlerts = false;

                // Creation a new Workbook
                var excelworkBook = excel.Workbooks.Add(Type.Missing);

                // Workk sheet
                var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                excelSheet.Name = "Work Item Field History";

                //Headers
                excelSheet.Range["$A$1:$F$1"].Font.Bold = true;
                excelSheet.Range["$A$1:$F$1"].Interior.Color = System.Drawing.Color.LightGray;
                excelSheet.Cells[1, 1] = "Field Name";
                excelSheet.Cells[1, 2] = "Revision Number";
                excelSheet.Cells[1, 3] = "Revised By";
                excelSheet.Cells[1, 4] = "Revision Date";
                excelSheet.Cells[1, 5] = "New Value";
                excelSheet.Cells[1, 6] = "Old Value";

                //Column Width
                for (int j = 1; j <=6; j++)
                {
                    excelSheet.Columns[j].ColumnWidth = 30;
                }

                int i = 2;

                foreach (FieldAtRevision fch in FieldChangesHistory)
                {
                    excelSheet.Cells[i, 1] = fch.CompleteName;
                    excelSheet.Cells[i, 2] = fch.RevisionNumber;
                    excelSheet.Cells[i, 3] = fch.RevisedBy;
                    excelSheet.Cells[i, 4] =  Convert.ToDateTime(fch.RevisionDate);
                    excelSheet.Cells[i, 5] = fch.NewValue;
                    excelSheet.Cells[i, 6] = fch.OldValue;
                    i++;
                }

                var fileName = Path.Combine(Path.GetTempPath(), string.Format("{0}_ID{1}_{2:yyyy_MM_dd}.xlsx", "WorkItemFieldHistory", workItemId, DateTime.Now));

                excelworkBook.SaveAs(fileName);
                System.Windows.MessageBox.Show("Excel file generated!",
                            "Work Item Field Hisotry",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information,
                            MessageBoxResult.OK,
                            System.Windows.MessageBoxOptions.DefaultDesktopOnly | System.Windows.MessageBoxOptions.DefaultDesktopOnly);
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

        private void GoToWorkItem()
        {
            IsBusy = true;

            var dte2 = VS.Package.GetGlobalService(typeof(DTE)) as EnvDTE80.DTE2;
            var witDocumentService = (DocumentService)dte2.DTE.GetObject(DOCUMENTSERVICE_CLASS);

            var widoc = witDocumentService.GetWorkItem(clientRepository.Collection, WorkItemId, this);
            try
            {
                witDocumentService.ShowWorkItem(widoc);
            }
            finally
            {
                widoc.Release(this);
            }

            IsBusy = false;
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