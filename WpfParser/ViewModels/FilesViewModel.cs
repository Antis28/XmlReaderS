using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfParser.Services;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class FilesViewModel : ViewModel
    {
        #region ResponseFiles : IEnumerable<ResponseFileViewModel> - файлы xml
        ///<summary>Выбранный файл xml</summary>
        private ObservableCollection<ResponseFileViewModel> _responseFiles;

        ///<summary>Выбранный файл xml</summary>
        public ObservableCollection<ResponseFileViewModel> ResponseFiles
        {
            get => _responseFiles;
            set
            {
                if (!Set(ref _responseFiles, value)) return;

                _fileXmlCollection.Source = value;
                OnPropertyChanged(nameof(FileXmlCollection));
            }
        }

        /// <summary>
        /// Колекция для фильтрации
        /// </summary>
        private readonly CollectionViewSource _fileXmlCollection = new CollectionViewSource();
        /// <summary>
        /// Колекция для фильтрации
        /// </summary>
        public ICollectionView FileXmlCollection => _fileXmlCollection?.View;
        
        #region FilesFilterText : string - Текст фильтра получателей
        ///<summary>Текст фильтра получателей</summary>
        private string _filesFilterText;

        ///<summary>Текст фильтра получателей</summary>
        public string FilesFilterText
        {
            get => _filesFilterText;
            set
            {
                if (!Set(ref _filesFilterText, value)) return;

                _fileXmlCollection.View.Refresh();
            }
        }

        private void OnPersonFilterAllFiles(object sender, FilterEventArgs e)
        {
            if (!(e.Item is ResponseFileViewModel response))
            {
                e.Accepted = false;
                return;
            }
            
            var filterText = _filesFilterText;
            if (string.IsNullOrEmpty(filterText)) return;

            var reports = response.ReportToRecipient;
            foreach (var report in reports)
            {
                if (report.Surname is null)
                {
                    e.Accepted = false;
                    return;
                }

                if (report.Surname.ToLower().Contains(filterText.ToLower())) return;

                e.Accepted = false;
            }
        }
        #endregion


        #endregion


        public FilesViewModel()
        {
            var rf = DataService.ReadResponseFiles();
            ResponseFiles = new ObservableCollection<ResponseFileViewModel>(rf);

            _fileXmlCollection.Filter += OnPersonFilterAllFiles;
        }
    }
}
