using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfParser.Models;
using WpfParser.Services;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class ConsoleViewModel : ViewModel
    {
        #region ConsoleBoxView : string - ConsoleBox
        ///<summary>My notyfy</summary>
        private ObservableCollection<ConsoleMessage> _consoleBoxView;
        ///<summary>My notyfy</summary>
        public ObservableCollection<ConsoleMessage> ConsoleBoxView { get => _consoleBoxView; set => Set(ref _consoleBoxView, value); }
        #endregion

        public ConsoleViewModel()
        {
            ConsoleBoxView = new ObservableCollection<ConsoleMessage>();
            ConsoleService.SetObservableCollection(ConsoleBoxView);
        }
    }
}
