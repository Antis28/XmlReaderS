using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfParser.Models;
using WpfParser.Services;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class AllFilesViewModel : ViewModel
    {
        public ObservableCollection<ResponseFileViewModel> ResponseFiles { get; }

        #region SelectedFile : ResponseFileViewModel - Выбранный файл xml
        ///<summary>Выбранная группа</summary>
        private ResponseFileViewModel _SelectedFile;
        ///<summary>Выбранная группа</summary>
        public ResponseFileViewModel SelectedFile { get => _SelectedFile; set => Set(ref _SelectedFile, value); }
        #endregion


        public AllFilesViewModel()
        {
            var rf = DataService.ReadResponseFiles();
            ResponseFiles = new ObservableCollection<ResponseFileViewModel>(rf);
        }
    }
}
