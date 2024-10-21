using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfParser.Infrastructure.Commands;
using WpfParser.Models;
using WpfParser.Services;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class MainViewModel: ViewModel
    {
        #region Status : string - Статус программы
        /// <summary> Статус программы </summary>
        private string _status = "Готов!";


        /// <summary>
        /// Статус программы
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }
        #endregion

        #region SelectedXml : ResponseFileViewModel - Выбранный Xml
        ///<summary>Выбранный Xml</summary>
        private ResponseFileViewModel _selectedXml;

        ///<summary>Выбранный Xml</summary>
        public ResponseFileViewModel SelectedXml
        {
            get => _selectedXml;
            set => Set(ref _selectedXml, value);
        }
        #endregion


        #region constructor

        public MainViewModel()
        {
            
        }
        #endregion
    }
}
