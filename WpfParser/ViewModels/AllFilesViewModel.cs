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
using WpfParser.Models;
using WpfParser.Services;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class AllFilesViewModel : ViewModel
    {


        #region ConsoleBoxView : string - ConsoleBox
        ///<summary>My notyfy</summary>
        private ObservableCollection<ConsoleMessage> _ConsoleBoxView;
        ///<summary>My notyfy</summary>
        public ObservableCollection<ConsoleMessage> ConsoleBoxView { get => _ConsoleBoxView; set => Set(ref _ConsoleBoxView, value); }
        #endregion



        #region Видимость закладки консоль
            #region IsVisibleConsole : bool - Видимость закладки консоль
        ///<summary>Выбранный файл xml</summary>
        private Visibility _IsVisibleConsole = Visibility.Collapsed;

        ///<summary>Выбранный файл xml</summary>
        public Visibility IsVisibleConsole
        { get => _IsVisibleConsole; set => Set(ref _IsVisibleConsole, value); }

            #endregion


            #region CheckVisibleConsoleCommand - Переключить видимость закладки консоль

        ///<summary>Переключить видимость закладки консоль</summary>
        public ICommand CheckVisibleConsoleCommand { get; }

        private bool CanCheckVisibleConsoleCommandExecute(object p) => true;

        private void OnCheckVisibleConsoleCommandExecuted(object p)
        {
            IsVisibleConsole = IsVisibleConsole == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        #endregion
        #endregion



        #region Видимость поля поиска по файлам
        #region IsVisibleConsole : bool - Видимость поля поиска по файлам
        ///<summary>Выбранный файл xml</summary>
        private Visibility _IsVisibleSerchInFiles = Visibility.Collapsed;

        ///<summary>Выбранный файл xml</summary>
        public Visibility IsVisibleSearchInFiles
        { get => _IsVisibleSerchInFiles; set => Set(ref _IsVisibleSerchInFiles, value); }

        #endregion


        #region CheckVisibleSearchInFilesCommand - Переключить видимость закладки консоль

        ///<summary>Переключить видимость закладки консоль</summary>
        public ICommand CheckVisibleSearchInFilesCommand { get; }

        private bool CanCheckVisibleSearchInFilesCommandExecute(object p) => true;

        private void OnCheckVisibleSearchInFilesCommandExecuted(object p)
        {
            IsVisibleSearchInFiles = IsVisibleSearchInFiles == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        #endregion
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


        #region ClearPersonFieldCommand - 

        ///<summary>My notyfy</summary>
        public ICommand ClearPersonFieldCommand { get; }

        private bool CanClearPersonFieldCommandExecute(object p) => true;

        private void OnClearPersonFieldCommandExecuted(object p)
        {
            PersonFilterText = string.Empty;
        }
        #endregion



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
            if (ResponseFiles == null)
            {
                try
                {
                    rf = DataService.ReadResponseFiles();
                }
                catch (Exception e)
                {
                    ConsoleService.GetInstance().ShowMessage("Произошла ошибка!", e.Message);
                    return;
                }
                if (rf == null) { return; }
                ResponseFiles = new ObservableCollection<ResponseFileViewModel>(rf);
            }
            UpdateShowingNames();
        }

        public void UpdateShowingNames()
        {
            if (ResponseFiles == null)
            {
                return;
            }

            if (IsOnlyDis)
            {
                FileNames.Clear();
                var index = 0;
                foreach (var file in ResponseFiles)
                {
                    index++;

                    var tempName = ExtractDckFromName(file);
                    var tempName2 = ExtractDisFromName(file);
                    file.VisibleName = $"{tempName2} - {tempName}";

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
                    file.VisibleName = $"{tempName} - Район: {tempName2}";
                    FileNames.Add(index, tempName);
                }
            }
            if (IsAllVisible)
            {
                FileNames.Clear();
                var index = 0;
                foreach (var file in ResponseFiles)
                {
                    index++;
                    var tempName = ExtractDckFromName(file);
                    var tempName2 = ExtractDisFromName(file);
                    file.VisibleName = file.FileName;
                    FileNames.Add(index, tempName);
                }
            }
            ResponseFiles = from item in ResponseFiles
                            orderby item.VisibleName
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
            CheckVisibleFileName();
        }
        #endregion

        #region ClearBaseCommand - LoadBase

        ///<summary>LoadBase</summary>
        public ICommand ClearBaseCommand { get; }

        private bool CanClearBaseCommandExecute(object p) => true;

        private void OnClearBaseCommandExecuted(object p)
        {
            ResponseFiles = new ObservableCollection<ResponseFileViewModel>();
            CheckVisibleFileName();
        }
        #endregion


        #endregion


        #region DragDropCommand - Gets and Sets the ICommand that manages dragging and dropping.

        ///<summary>For drag and drop</summary>
        public ICommand DragDropCommand { get; }

        private bool CanDragDropCommandExecute(object p) => true;

        private void OnDragDropCommandExecuted(object p)
        {
            throw new NotImplementedException();
        }


        #endregion






        public AllFilesViewModel()
        {
            ConsoleBoxView = new ObservableCollection<ConsoleMessage>();
            ConsoleService.SetObservableCollection(ConsoleBoxView);


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

            LoadBaseCommand =
                new LambdaCommand(OnLoadBaseCommandExecuted, CanLoadBaseCommandExecute);

            ClearBaseCommand =
                new LambdaCommand(OnClearBaseCommandExecuted, CanClearBaseCommandExecute);

            DragDropCommand =
                new LambdaCommand(OnDragDropCommandExecuted, CanDragDropCommandExecute);

            CheckVisibleConsoleCommand =
                new LambdaCommand(OnCheckVisibleConsoleCommandExecuted, CanCheckVisibleConsoleCommandExecute);

            CheckVisibleSearchInFilesCommand =
                new LambdaCommand(OnCheckVisibleSearchInFilesCommandExecuted, CanCheckVisibleSearchInFilesCommandExecute);

            ClearPersonFieldCommand =
                new LambdaCommand(OnClearPersonFieldCommandExecuted, CanClearPersonFieldCommandExecute);
            #endregion


            _fileNames = new Dictionary<int, string>();

            ResponseFiles = new ObservableCollection<ResponseFileViewModel>();
            //CheckVisibleFileName();

            _selectedXmlFileCollection.Filter += OnPersonFiltered;
            _fileXmlCollection.Filter += OnPersonFilterAllFiles;


        }


    }
}
