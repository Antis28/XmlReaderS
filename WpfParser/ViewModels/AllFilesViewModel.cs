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
        ///<summary>Список Отчет По Получателю</summary>
        private IEnumerable<ResponseFileViewModel> _ResponseFiles ;
        ///<summary>Список Отчет По Получателю</summary>
        public IEnumerable<ResponseFileViewModel> ResponseFiles { get => _ResponseFiles; set => Set(ref _ResponseFiles, value); }
       

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

        #region IsAllVisible : bool - Отображать только все
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

        #region CheckVisibleFileName

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
                foreach (var file in ResponseFiles)
                {
                    var indEnd = file.FileName.IndexOf("-DCK-", StringComparison.InvariantCultureIgnoreCase);
                    file.FileName = file.FileName.Remove(indEnd);
                    var indStart = file.FileName.IndexOf("-DIS-", StringComparison.InvariantCultureIgnoreCase);
                    file.FileName = file.FileName.Remove(0, indStart +5);
                }
            }
            if (IsOnlyDck)
            {
                foreach (var file in ResponseFiles)
                {
                    var indEnd = file.FileName.IndexOf("-001-DOC-", StringComparison.InvariantCultureIgnoreCase);
                    file.FileName = file.FileName.Remove(indEnd);
                    var indStart = file.FileName.IndexOf("-DCK-", StringComparison.InvariantCultureIgnoreCase);
                    file.FileName = file.FileName.Remove(0, indStart + 1);
                }
            }
            ResponseFiles = from item in ResponseFiles
                                orderby item.FileName
                                select item;
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
            CheckVisibleFileName();
        }
    }
}
