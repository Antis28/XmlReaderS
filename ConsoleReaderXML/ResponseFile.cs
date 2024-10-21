using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReaderXML
{
    /// <summary>
    /// Корневой элемент
    /// </summary>
    internal class ResponseFile
    {
        public string FileName { get; set; }

        public List<ReportToRecipient> ReportToRecipient { get; set; }
    }
}
