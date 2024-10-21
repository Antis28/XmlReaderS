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
        public ObservableCollection<ResponseFileViewModel> ResponseFiles { get; private set; }

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

        #region Commands

        #region OnlyDckCommand

        public ICommand OnlyDckCommand { get; } 
        private bool CanOnlyDckCommandExecute(object p) => true;

        private void OnOnlyDckCommandExecuted(object p)
        {
            if (IsOnlyDck)
            {
                foreach (var file in ResponseFiles)
                {
                    var indEnd = file.FileName.IndexOf("-DOC-", StringComparison.InvariantCultureIgnoreCase);
                    file.FileName = file.FileName.Remove(indEnd);
                    var indStart = file.FileName.IndexOf("-DCK-", StringComparison.InvariantCultureIgnoreCase);
                    file.FileName = file.FileName.Remove(0, indStart+1);
                }
            }
            else
            {
                var rf = DataService.ReadResponseFiles();
                ResponseFiles = new ObservableCollection<ResponseFileViewModel>(rf);
            }
        }
        #endregion

        #endregion

        public AllFilesViewModel()
        {
            #region Команды

            OnlyDckCommand =
                new LambdaCommand(OnOnlyDckCommandExecuted, CanOnlyDckCommandExecute);


            #endregion

            var rf = DataService.ReadResponseFiles();
            ResponseFiles = new ObservableCollection<ResponseFileViewModel>(rf);
        }
    }
}
