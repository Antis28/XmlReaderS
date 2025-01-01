using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfParser.Models;

namespace WpfParser.Services
{
    internal class ConsoleService
    {
        // Коллекция для отображения в WPF
        private ObservableCollection<ConsoleMessage> _consoleBoxView;

        #region Singleton
        private static ConsoleService _instance;

        private ConsoleService() { }

        public static ConsoleService GetInstance()
        {
            return _instance ?? (_instance = new ConsoleService());
        }
        #endregion

        
        public static void SetObservableCollection(ObservableCollection<ConsoleMessage> consoleBoxView)
        {
            GetInstance()._consoleBoxView = consoleBoxView;
        }

        public void ShowMessage(ConsoleMessage message)
        {
            _consoleBoxView.Add(message);
        }
        public void ShowMessage(string nameMessage, string message)
        {
            var cMessage = new ConsoleMessage
            {
                Name = nameMessage,
                Description = "Описание ошибки: " + message
            };
            _consoleBoxView.Add(cMessage);
        }
    }
}
