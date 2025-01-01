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

        #region File name visible
        private string _VisibleName;
        /// <summary>Отображаемое имя Xml файла</summary>
        public string VisibleName
        {
            get => _VisibleName;
            set => Set(ref _VisibleName, value);
        }
        #endregion
        public IList<ReportToRecipientViewModel> ReportToRecipient { get; set; }
        
    }
}
