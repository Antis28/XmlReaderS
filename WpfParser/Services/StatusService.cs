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
        private static ViewModel _allFiles;

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

        public static int ProgressLoading
        {
            get
            {
                var s = (StatusViewModel)_allFiles;
                return s.UploadProgress;
            }
            set
            {
                var s = (StatusViewModel)_allFiles;
                s.UploadProgress = value;
            }
        }
        public static string FileSize
        {
            get
            {
                var s = (StatusViewModel)_allFiles;
                return s.FileSize;
            }
            set
            {
                var s = (StatusViewModel)_allFiles;
                s.FileSize = value;
            }
        }


        public static void SetStatusProperty(ViewModel sModel)
        {
            _status = sModel;
        }
        public static void SetFilesProperty(ViewModel allFiles)
        {
            _allFiles = allFiles;
        }
    }
}
