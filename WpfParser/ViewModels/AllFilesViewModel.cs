using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfParser.Infrastructure.Commands;
using WpfParser.Models;
using WpfParser.Services;
using WpfParser.ViewModels.Base;
using Helpers;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;

namespace WpfParser.ViewModels
{
    internal class AllFilesViewModel : ViewModel
    {
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
                var expr2 = ContainsCodeNoreturn(report.CodeNoReturn, filterText);
                // Искать в Dck
                var expr3 = response.Dck != null && filterText.Length > 3 && response.Dck.ToLower().Contains(filterText.ToLower());
                // Искать в районе -008
                var expr4 = response.District != null && filterText.Length == 3 && response.District.ToLower().Contains(filterText.ToLower());

                if (expr1 || expr2 || expr3 || expr4) return;
            }
            e.Accepted = false;
        }
        #endregion


        #endregion


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
            if (!(e.Item is ReportToRecipientModel report))
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
            // Искать по фамилии
            var expr1 = report.Surname.ToLower().Contains(filterText.ToLower());
            // Искать по коду возврата
            var expr2 = ContainsCodeNoreturn(report.CodeNoReturn, filterText);
            int integer;
            var b = int.TryParse(filterText, out integer);
            if (b) return;
            
            if (expr1 || expr2) return;

            e.Accepted = false;
        }

        private static bool ContainsCodeNoreturn(string codeNoReturn, string filterText)
        {
            return codeNoReturn != null 
                   && codeNoReturn.ToLower().Contains(filterText.ToLower()) 
                   && codeNoReturn.Length == filterText.Length;
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
                ScanDckAndDistrict();
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
                foreach (var file in ResponseFiles)
                {
                    file.VisibleName = $"Район: {file.District} - {file.Dck}";
                }
            }
            if (IsOnlyDck)
            {
                foreach (var file in ResponseFiles)
                {
                    file.VisibleName = $"{file.Dck} - Район: {file.District}";
                }
            }
            if (IsAllVisible)
            {
                foreach (var file in ResponseFiles)
                {
                    file.VisibleName = file.FileName;
                }
            }
            ResponseFiles = from item in ResponseFiles
                            orderby item.VisibleName
                            select item;
        }

        private static string ExtractDckFromName(ResponseFileViewModel file)
        {
            var tempName = file.FileName;

            var indEnd = tempName.IndexOf("-DOC-", StringComparison.InvariantCultureIgnoreCase)-4;
            if (indEnd >= 0)
                tempName = tempName.Remove(indEnd);
            else
            {
                return tempName;
            }
            
            var indStart = tempName.IndexOf("-DCK-", StringComparison.InvariantCultureIgnoreCase);
            if (indStart >= 0)
                tempName = tempName.Remove(0, indStart + 1);
            else
            {
                return tempName;
            }
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
            StatusService.FileCounts = 0;
        }
        #endregion


        #endregion


        
        public bool LoadFilesInBase(string[] addedElements)
        {
            if (addedElements == null) return false;
            try
            {
                List<string> fileNames = new List<string>();
                
                foreach (string fileName in addedElements)
                {
                    var isDirectory = fileName.IsDirectory();
                    var isXmlFile = Path.GetExtension(fileName).ToLower() == ".xml";
                    if (isDirectory)
                    {
                        // Переданный элемент является каталогом
                        var files = Directory.GetFiles(fileName, "*.xml", SearchOption.AllDirectories);
                        fileNames.AddRange(files);
                    }
                    else
                    {
                        // Переданный элемент является файлом
                        fileNames.Add(fileName);
                    }
                }
                
                var rf = DataService.ReadResponseFiles(fileNames.ToArray());

                var coll = new ObservableCollection<ResponseFileViewModel>();
                
                foreach (var item in ResponseFiles)
                {
                    coll.Add(item);
                }

                foreach (var item in rf)
                {
                    coll.Add(item);
                }
                ResponseFiles = coll;
                StatusService.FileCounts = coll.Count;

                ScanDckAndDistrict();
                UpdateShowingNames();
            }
            catch (Exception ex)
            {
                ConsoleService.GetInstance().ShowMessage("Произошла ошибка!", ex.Message);
                return false;
            }

            return true;
        }

        private void ScanDckAndDistrict()
        {
            foreach (var file in ResponseFiles)
            {
                file.Dck = ExtractDckFromName(file);
                file.District = $"{ExtractDisFromName(file)}";
            }
        }

        public AllFilesViewModel()
        {
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

            CheckVisibleSearchInFilesCommand =
                new LambdaCommand(OnCheckVisibleSearchInFilesCommandExecuted, CanCheckVisibleSearchInFilesCommandExecute);

            ClearPersonFieldCommand =
                new LambdaCommand(OnClearPersonFieldCommandExecuted, CanClearPersonFieldCommandExecute);
            #endregion

            ResponseFiles = new ObservableCollection<ResponseFileViewModel>();
            //CheckVisibleFileName();

            _selectedXmlFileCollection.Filter += OnPersonFiltered;
            _fileXmlCollection.Filter += OnPersonFilterAllFiles;

            
        }
    }
}
