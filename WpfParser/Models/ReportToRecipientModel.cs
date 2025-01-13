using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WpfParser.Models;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    /// <summary>
    /// Отчет По Получателю
    /// </summary>
    internal class ReportToRecipientModel 
    {
        /// <summary>
        /// Код района
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// Страховой номер
        /// </summary>
        public string Snils { get; set; }

        /// <summary>
        /// Отзываемая Сумма
        /// </summary>
        public string SumRecalled { get; set; }

        /// <summary>
        /// КодНевозврата
        /// </summary>
        public string CodeNoReturn { get; set; }
        
        /// <summary>
        /// ДатаСмерти
        /// </summary>
        public string DateOfDeath { get; set; }

        public Payment Payment {get; set; }

        public string Surname { get; set; }
        public string NamePerson { get; set; }
        public string Patronymic { get; set; }


    }
}
