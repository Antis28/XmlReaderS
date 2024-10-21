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
        public string FileName { get; set; }
        public IList<ReportToRecipient> ReportToRecipient { get; set; }
        
    }
}
