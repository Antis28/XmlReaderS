using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfParser.Models;
using WpfParser.ViewModels;
using WpfParser.ViewModels.Base;

namespace WpfParser.Services
{
    internal static class StatusService
    {

        private static ViewModel _status;

        public static int FileCounts
        {
            get
            {
                var s = (StatusViewModel)_status;
                return int.Parse(s.FileCounts);
            }
            set
            {
                var s = (StatusViewModel)_status;
                s.FileCounts = value == 0 ? "Нет загруженных файлов." : $"Загружено файлов: {value}";
            }
        }


        public static void SetObservableProperty(ViewModel sModel)
        {
            _status = sModel;
        }
    }
}
