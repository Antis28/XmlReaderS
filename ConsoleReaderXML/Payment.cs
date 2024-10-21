using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReaderXML
{
    internal class Payment
    {
        /// <summary>
        /// Сумма к выплате
        /// </summary>
        public string AmountToPaid { get; set; }
        /// <summary>
        /// Дата Начала Периода
        /// </summary>
        public string StartDateOfPeriod { get; set; }
        /// <summary>
        /// Дата Конца Периода
        /// </summary>
        public string EndDateOfPeriod { get; set; }
        /// <summary>
        /// Вид Выплаты По ПЗ
        /// </summary>
        public string TypeOfPayment { get; set; }

       
    }
}
