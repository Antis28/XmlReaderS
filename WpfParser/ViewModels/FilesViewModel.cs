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
        #region FilesXml : IEnumerable<ResponseFileViewModel> - файлы xml
        ///<summary>Выбранный файл xml</summary>
        private ObservableCollection<ResponseFileViewModel> _filesXml;

        ///<summary>Выбранный файл xml</summary>
        public ObservableCollection<ResponseFileViewModel> FilesXml
        {
            get => _filesXml;
            set
            {
                if (!Set(ref _filesXml, value)) return;

                _fileXmlCollection.Source = value;
                OnPropertyChanged(nameof(FileXmlCollection));
            }
        }

        private readonly CollectionViewSource _fileXmlCollection = new CollectionViewSource();
        public ICollectionView FileXmlCollection => _fileXmlCollection?.View;
        
        #region PersonFilterText : string - Текст фильтра получателей
        ///<summary>Текст фильтра получателей</summary>
        private string _personFilterText;

        ///<summary>Текст фильтра получателей</summary>
        public string PersonFilterText
        {
            get => _personFilterText;
            set
            {
                if (!Set(ref _personFilterText, value)) return;

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
            
            var filterText = _personFilterText;
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
            FilesXml = new ObservableCollection<ResponseFileViewModel>(rf);

            _fileXmlCollection.Filter += OnPersonFilterAllFiles;
        }
    }
}
