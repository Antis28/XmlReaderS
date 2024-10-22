using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfParser.Infrastructure.Commands;
using WpfParser.Models;
using WpfParser.Services;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class AllFilesViewModel : ViewModel
    {
        #region Список Отчет По Получателю
        ///<summary>Список Отчет По Получателю</summary>
        private IEnumerable<ResponseFileViewModel> _ResponseFiles ;


        ///<summary>Список Отчет По Получателю</summary>
        public IEnumerable<ResponseFileViewModel> ResponseFiles { get => _ResponseFiles; set => Set(ref _ResponseFiles, value); }
        #endregion
       
        /// <summary>
        /// Список имен файлов
        /// </summary>
        private IDictionary<int, string> _FileNames;
        public IDictionary<int, string> FileNames { get => _FileNames; set => Set(ref _FileNames, value); }


        #region SelectedFile : ResponseFileViewModel - Выбранный файл xml
        ///<summary>Выбранная группа</summary>
        private ResponseFileViewModel _SelectedFile;
        ///<summary>Выбранная группа</summary>
        public ResponseFileViewModel SelectedFile { get => _SelectedFile; set => Set(ref _SelectedFile, value); }
        #endregion

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
            var rf = DataService.ReadResponseFiles();
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
            tempName = tempName.Remove(0, indStart +5);
            return tempName;
        }

        #endregion

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
            #endregion

            var rf = DataService.ReadResponseFiles();
            ResponseFiles = new ObservableCollection<ResponseFileViewModel>(rf);
            _FileNames = new Dictionary<int, string>();
            CheckVisibleFileName();
        }
    }
}
