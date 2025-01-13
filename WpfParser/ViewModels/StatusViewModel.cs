using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        #region FileFoundCounts : string - Количество найденных файлов
        ///<summary>Количество найденных файлов</summary>
        private string _fileFoundCounts;
        ///<summary>Количество найденных файлов</summary>
        public string FileFoundCounts
        {
            get => _fileFoundCounts;
            set => Set(ref _fileFoundCounts, value);
        }
        #endregion

        #region UploadProgress : int - Upload Progress
        ///<summary>Upload Progress</summary>
        public int UploadProgress
        {
            get => (int)GetValue(UploadProgressProperty);
            set => SetValue(UploadProgressProperty, value);
        }
        // Using a DependencyProperty as the backing store for UploadProgress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UploadProgressProperty =
            DependencyProperty.Register(nameof(UploadProgress), typeof(int), typeof(StatusViewModel));
        #endregion

        #region FileSize : string - File size
        ///<summary>File size</summary>
        public string FileSize
        {
            get => (string)GetValue(FileSizeProperty);
            set => SetValue(FileSizeProperty, value);
        }
        // Using a DependencyProperty as the backing store for FileSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileSizeProperty =
            DependencyProperty.Register(nameof(FileSize), typeof(string), typeof(StatusViewModel));
        #endregion

        #region UploadProgress : int - Значение прогресса загрузки
        /////<summary>Значение прогресса загрузки</summary>
        //private int _uploadProgress;
        /////<summary>Значение прогресса загрузки</summary>
        //public int UploadProgress { get => _uploadProgress; set => Set(ref _uploadProgress, value); }
        #endregion
        public StatusViewModel()
        {
            StatusService.SetStatusProperty(  this);
            StatusService.SetFilesProperty(this);
            StatusService.FileCounts = 0;
        }
    }
}
