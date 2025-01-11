using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfParser.Infrastructure.Commands;
using WpfParser.Models;
using WpfParser.Services;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class ConsoleViewModel : ViewModel
    {
        #region ConsoleBoxView : string - ConsoleBox
        ///<summary>ConsoleBox</summary>
        private ObservableCollection<ConsoleMessage> _consoleBoxView;
        ///<summary>ConsoleBox</summary>
        public ObservableCollection<ConsoleMessage> ConsoleBoxView { get => _consoleBoxView; set => Set(ref _consoleBoxView, value); }
        #endregion


        
        #region ShowMessageBoxCommand - Показывать всплывающее сообщение ?

        ///<summary>Показывать всплывающее сообщение ?</summary>
        public ICommand ShowMessageBoxCommand { get; }

        private bool CanShowMessageBoxCommandExecute(object p) => true;

        private void OnShowMessageBoxCommandExecuted(object p)
        {
            ConsoleService.IsShowMessageBox = !ConsoleService.IsShowMessageBox;
        }
        #endregion

        




        public ConsoleViewModel()
        {
            ConsoleBoxView = new ObservableCollection<ConsoleMessage>();
            ConsoleService.SetObservableCollection(ConsoleBoxView);

            ShowMessageBoxCommand = new LambdaCommand(OnShowMessageBoxCommandExecuted, CanShowMessageBoxCommandExecute);
        }
    }
}
