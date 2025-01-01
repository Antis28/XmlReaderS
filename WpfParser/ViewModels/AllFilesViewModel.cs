using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfParser.Infrastructure.Commands;
using WpfParser.Services;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class AllFilesViewModel : ViewModel
    {


        #region ConsoleBoxView : string - My notyfy
        ///<summary>My notyfy</summary>
        private ObservableCollection<string> _ConsoleBoxView;
        ///<summary>My notyfy</summary>
        public ObservableCollection<string> ConsoleBoxView { get => _ConsoleBoxView; set => Set(ref _ConsoleBoxView, value); }
        #endregion






        #region ResponseFiles : IEnumerable<ResponseFileViewModel> - файлы xml
        ///<summary>Выбранный файл xml</summary>
        private IEnumerable<ResponseFileViewModel> _responseFiles;

        ///<summary>Выбранный файл xml</summary>
        public IEnumerable<ResponseFileViewModel> ResponseFiles
        {
            get => _responseFiles;
            set
            {
                if (!Set(ref _responseFiles, value)) return;

                _fileXmlCollection.Source = value;
                OnPropertyChanged(nameof(FileXmlCollection));
            }
        }

        /// <summary>
        /// Колекция для фильтрации
        /// </summary>
        private readonly CollectionViewSource _fileXmlCollection = new CollectionViewSource();
        /// <summary>
        /// Колекция для фильтрации
        /// </summary>
        public ICollectionView FileXmlCollection => _fileXmlCollection?.View;

        #region FilesFilterText : string - Текст фильтра получателей
        ///<summary>Текст фильтра получателей</summary>
        private string _filesFilterText;

        ///<summary>Текст фильтра получателей</summary>
        public string FilesFilterText
        {
            get => _filesFilterText;
            set
            {
                if (!Set(ref _filesFilterText, value)) return;

                _fileXmlCollection?.View?.Refresh();
            }
        }

        private void OnPersonFilterAllFiles(object sender, FilterEventArgs e)
        {
            if (!(e.Item is ResponseFileViewModel response))
            {
                e.Accepted = false;
                return;
            }

            var filterText = _filesFilterText;
            if (string.IsNullOrEmpty(filterText)) return;

            
            var reports = response.ReportToRecipient;
            foreach (var report in reports)
            {
                if (report.Surname is null)
                {
                    e.Accepted = false;
                    return;
                }

                var expr1 = report.Surname.ToLower().Contains(filterText.ToLower());
                var expr2 = report.CodeNoReturn != null && report.CodeNoReturn.ToLower().Contains(filterText.ToLower());
                if (expr1 || expr2) return;
            }
            e.Accepted = false;
        }
        #endregion


        #endregion
        

        /// <summary>
        /// Список имен файлов
        /// </summary>
        private IDictionary<int, string> _fileNames;
        public IDictionary<int, string> FileNames { get => _fileNames; set => Set(ref _fileNames, value); }


        #region SelectedFile : ResponseFileViewModel - Выбранный файл xml
        ///<summary>Выбранный файл xml</summary>
        private ResponseFileViewModel _selectedFile;

        ///<summary>Выбранный файл xml</summary>
        public ResponseFileViewModel SelectedFile
        {
            get => _selectedFile;
            set
            {
                if (!Set(ref _selectedFile, value)) return;

                _selectedXmlFileCollection.Source = value?.ReportToRecipient;
                OnPropertyChanged(nameof(SelectedXmlFileCollection));
            }
        }

        private readonly CollectionViewSource _selectedXmlFileCollection = new CollectionViewSource();
        public ICollectionView SelectedXmlFileCollection => _selectedXmlFileCollection?.View;



        #region PersonFilterText : string - Текст фильтра получателей
        ///<summary>Текст фильтра получателей</summary>
        private string _personFilterText;

        ///<summary>Текст фильтра получателей</summary>
        public string PersonFilterText
        {
            get => _personFilterText;
            set
            {
                if (!Set(ref _personFilterText, value)) return;

                _selectedXmlFileCollection?.View?.Refresh();
                FilesFilterText = value;
            }
        }

        private void OnPersonFiltered(object sender, FilterEventArgs e)
        {
            if (!(e.Item is ReportToRecipientViewModel report))
            {
                e.Accepted = false;
                return;
            }
            var filterText = _personFilterText;
            if (string.IsNullOrEmpty(filterText)) return;

            if (report.Surname is null)
            {
                e.Accepted = false;
                return;
            }
            
            var expr1 = report.Surname.ToLower().Contains(filterText.ToLower());
            var expr2 = report.CodeNoReturn != null && report.CodeNoReturn.ToLower().Contains(filterText.ToLower());
            if (expr1 || expr2) return;

            e.Accepted = false;
        }
        #endregion


        #endregion

        #region Visible Files

        #region IsOnlyDck : bool - Отображать только Dck
        ///<summary>Отображать только Dck</summary>
        private bool _IsOnlyDck = false;
        ///<summary>Отображать только Dck</summary>
        public bool IsOnlyDck { get => _IsOnlyDck; set => Set(ref _IsOnlyDck, value); }
        #endregion

        #region IsOnlyDis : bool - Отображать только Dis
        ///<summary>Отображать только Dck</summary>
        private bool _IsOnlyDis = true;
        ///<summary>Отображать только Dck</summary>
        public bool IsOnlyDis { get => _IsOnlyDis; set => Set(ref _IsOnlyDis, value); }
        #endregion

        #region IsAllVisible : bool - Отображать всё
        ///<summary>Отображать только все</summary>
        private bool _IsAllVisible = false;
        ///<summary>Отображать только все</summary>
        public bool IsAllVisible { get => _IsAllVisible; set => Set(ref _IsAllVisible, value); }
        #endregion 
        #endregion

        #region Commands

        #region OnlyDckCommand

        public ICommand OnlyDckCommand { get; }
        private bool CanOnlyDckCommandExecute(object p) => true;

        private void OnOnlyDckCommandExecuted(object p)
        {
            CheckVisibleFileName();
        }

        #endregion

        #region OnlyDisCommand

        public ICommand OnlyDisCommand { get; }
        private bool CanOnlyDisCommandExecute(object p) => true;

        private void OnOnlyDisCommandExecuted(object p)
        {
            CheckVisibleFileName();
        }




        #endregion

        #region AllVisibleCommand

        public ICommand AllVisibleCommand { get; }
        private bool CanAllVisibleCommandExecute(object p) => true;

        private void OnAllVisibleCommandExecuted(object p)
        {
            CheckVisibleFileName();
        }
        #endregion

        #region CheckVisibleFileNameCommand

        public ICommand CheckVisibleFileNameCommand { get; }
        private bool CanCheckVisibleFileNameCommandExecute(object p) => true;

        private void OnCheckVisibleFileNameCommandExecuted(object p)
        {
            CheckVisibleFileName();
        }
        #endregion

        private void CheckVisibleFileName()
        {
            IEnumerable<ResponseFileViewModel> rf = null;
            try
            {
                rf = DataService.ReadResponseFiles();
            } catch (Exception e)
            {
                ConsoleBoxView.Add(e.Message);
                //Console.WriteLine(e);
                return;
            }
            

            ResponseFiles = new ObservableCollection<ResponseFileViewModel>(rf);

            if (IsOnlyDis)
            {
                FileNames.Clear();
                var index = 0;
                foreach (var file in ResponseFiles)
                {
                    index++;

                    var tempName = ExtractDckFromName(file);
                    var tempName2 = ExtractDisFromName(file);
                    file.FileName = $"{tempName2} - {tempName}";

                    FileNames.Add(index, tempName);
                }
            }
            if (IsOnlyDck)
            {
                FileNames.Clear();
                var index = 0;
                foreach (var file in ResponseFiles)
                {
                    index++;
                    var tempName = ExtractDckFromName(file);
                    var tempName2 = ExtractDisFromName(file);
                    file.FileName = $"{tempName} - Район: {tempName2}";
                    FileNames.Add(index, tempName);
                }
            }
            ResponseFiles = from item in ResponseFiles
                            orderby item.FileName
                            select item;
        }

        private static string ExtractDckFromName(ResponseFileViewModel file)
        {
            var tempName = file.FileName;
            var indEnd = tempName.IndexOf("-001-DOC-", StringComparison.InvariantCultureIgnoreCase);
            tempName = tempName.Remove(indEnd);
            var indStart = tempName.IndexOf("-DCK-", StringComparison.InvariantCultureIgnoreCase);
            tempName = tempName.Remove(0, indStart + 1);
            return tempName;
        }

        private static string ExtractDisFromName(ResponseFileViewModel file)
        {
            var tempName = file.FileName;
            var indEnd = tempName.IndexOf("-DCK-", StringComparison.InvariantCultureIgnoreCase);
            tempName = tempName.Remove(indEnd);
            var indStart = tempName.IndexOf("-DIS-", StringComparison.InvariantCultureIgnoreCase);
            tempName = tempName.Remove(0, indStart + 5);
            return tempName;
        }


        #region LoadBaseCommand - LoadBase

        ///<summary>LoadBase</summary>
        public ICommand LoadBaseCommand { get; }

        private bool CanLoadBaseCommandExecute(object p) => true;

        private void OnLoadBaseCommandExecuted(object p)
        {
            var rf = DataService.ReadResponseFiles();
            ResponseFiles = new ObservableCollection<ResponseFileViewModel>(rf);
        }
        #endregion


        #endregion



        public AllFilesViewModel()
        {
            ConsoleBoxView = new ObservableCollection<string>();

            #region Команды

            OnlyDckCommand =
                new LambdaCommand(OnOnlyDckCommandExecuted, CanOnlyDckCommandExecute);
            AllVisibleCommand =
                new LambdaCommand(OnAllVisibleCommandExecuted, CanAllVisibleCommandExecute);
            OnlyDisCommand
                =
                new LambdaCommand(OnOnlyDisCommandExecuted, CanOnlyDisCommandExecute);
            CheckVisibleFileNameCommand =
                new LambdaCommand(OnCheckVisibleFileNameCommandExecuted, CanCheckVisibleFileNameCommandExecute);
            
            LoadBaseCommand=
                new LambdaCommand(OnLoadBaseCommandExecuted, CanLoadBaseCommandExecute);
            #endregion

            
            _fileNames = new Dictionary<int, string>();
            CheckVisibleFileName();

            _selectedXmlFileCollection.Filter += OnPersonFiltered;
            _fileXmlCollection.Filter += OnPersonFilterAllFiles;

            
        }


    }
}
