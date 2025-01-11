using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfParser.Services;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class StatusViewModel : ViewModel
    {
        #region FileCounts : string - Количество загруженных файлов
        ///<summary>Количество загруженных файлов</summary>
        private string _fileCounts;
        ///<summary>Количество загруженных файлов</summary>
        public string FileCounts
        {
            get => _fileCounts;
            set => Set(ref _fileCounts, value);
        }
        #endregion
        

        public StatusViewModel()
        {
            StatusService.SetStatusProperty(  this);
            StatusService.FileCounts = 0;
        }
    }
}
