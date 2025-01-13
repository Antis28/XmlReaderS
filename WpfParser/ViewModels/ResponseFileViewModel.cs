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


        #region Dck : string - Для DCK
        ///<summary>Для DCK</summary>
        private string _dck;
        ///<summary>Для DCK</summary>
        public string Dck { get => _dck; set => Set(ref _dck, value); }
        #endregion

        #region District : string - Район
        ///<summary>Район</summary>
        private string _District;
        ///<summary>Район</summary>
        public string District { get => _District; set => Set(ref _District, value); }
        #endregion




        public IList<ReportToRecipientModel> ReportToRecipient { get; set; }
        
    }
}
