using System.Collections.Generic;
using WpfParser.Models;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    /// <summary>
    /// Корневой элемент
    /// </summary>
    internal class ResponseFileViewModel : ViewModel
    {

        #region FileName
        private string _FileName;
        /// <summary>Имя Xml файла</summary>
        public string FileName
        {
            get => _FileName;
            set => Set(ref _FileName, value);
        }
        #endregion
        public IList<ReportToRecipientViewModel> ReportToRecipient { get; set; }
        
    }
}
