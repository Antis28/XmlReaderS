﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Helpers;
using WpfParser.Models;
using WpfParser.ViewModels;

namespace WpfParser.Services
{
    internal class DataService
    {
        /// <summary>
        /// Читает 1 xml файл
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ResponseFileViewModel ReadResponseFile(string path)
        {
            var file = new ResponseFileViewModel
            {
                ReportToRecipient = new List<ReportToRecipientModel>()
            };

            using (XmlReader reader = XmlReader.Create(path))
            {
                var numberRecipient = -1; //номер ОтчетПоПолучателю
                try
                {
                    while (reader.Read())     // чтение некст элемента
                    {
                        try { numberRecipient = ProcessXml(reader, file, ref numberRecipient); }
                        catch (Exception ex)
                        {
                            ConsoleService.GetInstance().ShowMessage($"Ошибка во время чтения файла:\n {path}", ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ConsoleService.GetInstance().ShowMessage($"Ошибка чтения файла:\n {path}", ex.Message);
                }
            }
            
            return file;
        }

        private static int ProcessXml(XmlReader reader, ResponseFileViewModel file, ref int numberRecipient)
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "ИмяФайла")
            {
                file.FileName = reader.ReadElementContentAsString();
            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "ОтчетПоПолучателю")
            {
                numberRecipient++;
                file.ReportToRecipient.Add(new ReportToRecipientModel());
                file.ReportToRecipient[numberRecipient].Payment = new Payment();
            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "КодРайона")
            {
                file.ReportToRecipient[numberRecipient].AreaCode = reader.ReadElementContentAsString();
            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "СтраховойНомер")
            {
                file.ReportToRecipient[numberRecipient].Snils = reader.ReadElementContentAsString();
            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Фамилия")
            {
                file.ReportToRecipient[numberRecipient].Surname = reader.ReadElementContentAsString();
            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Имя")
            {
                file.ReportToRecipient[numberRecipient].NamePerson = reader.ReadElementContentAsString();
            }


            if (reader.NodeType == XmlNodeType.Element && reader.Name == "Отчество")
            {
                file.ReportToRecipient[numberRecipient].Patronymic = reader.ReadElementContentAsString();
            }


            if (reader.NodeType == XmlNodeType.Element && reader.Name == "ДатаСмерти")
            {
                file.ReportToRecipient[numberRecipient].DateOfDeath = reader.ReadElementContentAsString();
            }


            if (reader.NodeType == XmlNodeType.Element && reader.Name == "ОтзываемаяСумма")
            {
                file.ReportToRecipient[numberRecipient].SumRecalled = reader.ReadElementContentAsString();
            }


            if (reader.NodeType == XmlNodeType.Element && reader.Name == "КодНевозврата")
            {
                file.ReportToRecipient[numberRecipient].CodeNoReturn = reader.ReadElementContentAsString();
                file.ReportToRecipient[numberRecipient].CodeNoReturnTip =
                    CodeToTip(file.ReportToRecipient[numberRecipient].CodeNoReturn);
            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "ДатаНачалаПериода")
            {
                file.ReportToRecipient[numberRecipient].Payment.StartDateOfPeriod = reader.ReadElementContentAsString();
            }
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "ДатаКонцаПериода")
            {
                file.ReportToRecipient[numberRecipient].Payment.EndDateOfPeriod = reader.ReadElementContentAsString();
            }
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "НомерСчета")
            {
                file.ReportToRecipient[numberRecipient].Payment.AccountNumber = reader.ReadElementContentAsString();
            }

            return numberRecipient;
        }
        /// <summary>
        /// Расшифровка кода не возврата
        /// </summary>
        /// <param name="codeNoReturn"></param>
        /// <returns></returns>
        private static string CodeToTip(string codeNoReturn)
        {
            switch (codeNoReturn)
            {
                case "НВ1": return "Не возврат сумм - средства возвращены ранее";
                case "НВ2": return "Не возврат сумм - средства перечислены на счет получателя, \r\nв том числе по его длительному поручению";
                case "НВ3": return "Не возврат сумм - средства переведены на имя другого лица, \r\nв том числе по длительному поручению получателя";
                case "НВ4": return "Не возврат сумм - средства перечислены в пользу организации";
                case "НВ5": return "Не возврат сумм - нарушены условия по вкладу";
                case "НВ6": return "Не возврат сумм - невозможно идентифицировать лицо, \r\nсовершившее расходную операцию по счёту банковской карты";
                case "НВ7": return "Не возврат сумм - получение средств наследником";
                case "НВ8": return "Не возврат сумм - получение средств доверенным лицом";
                case "НВ9": return "Не возврат сумм - пенсия за указанный период не зачислялась";
                case "НВ10": return "Не возврат сумм — неверно указан период возврата";
                case "НВ11": return "Не возврат сумм - неверно указан номер счёта";
                case "НВ12": return "Не возврат сумм - неверно указана сумма \r\nпенсии за указанный период";
                case "НВ14": return "Не возврат сумм - банкротство владельца счета";
                case "НВ15": return "Не возврат сумм - неверно указаны \r\nФИО получателя/наименование организации, \r\nоткрывшей номинальный счет";
                case "НВ17": return "Не возврат сумм - счёт получателя закрыт";
                case "НВ18": return "Не возврат сумм - на счёт наложен арест";
                case "НВ19": return "Не возврат сумм - средства перечислены \r\nв адрес исполнительных органов";
                case "НВ20": return "Средства перечислены в счёт погашения ссудной задолженности";
                case "НВ21": return "Не возврат сумм - получение средств владельцем счета";
                case "НВ22": return "Резерв";
                default:    return "Код не распознан";
            }
        }

        public static IEnumerable<ResponseFileViewModel> ReadResponseFiles()
        {
            var fileNames = Directory.GetFiles(Environment.CurrentDirectory, "*.xml", SearchOption.AllDirectories);
            return ReadResponseFiles(fileNames);
        }

        private static string[] FilterXmName(string[] addedElements)
        {
            List<string> fileNames = new List<string>();

            foreach (string fileName in addedElements)
            {
                var isDirectory = fileName.IsDirectory();
                var isXmlFile = Path.GetExtension(fileName).ToLower() == ".xml";
                if (isDirectory)
                {
                    // Переданный элемент является каталогом
                    var files = Directory.GetFiles(fileName, "*.xml", SearchOption.AllDirectories);
                    fileNames.AddRange(files);
                }
                else
                {
                    // Переданный элемент является файлом
                    fileNames.Add(fileName);
                }
            }

            return fileNames.ToArray();
        }

        public static IEnumerable<ResponseFileViewModel> ReadResponseFiles(string[] addedElements)
        {
            var fileNames = FilterXmName(addedElements);


            var progressLoading = 0;
            var oldProgress = 0;
            var currentPosition = 0;
            var contentLength = fileNames.Length;

            var files = new List<ResponseFileViewModel>();
            foreach (var xmlFile in fileNames)
            {
                currentPosition++;
                var file = ReadResponseFile(xmlFile);
                files.Add(file);

                oldProgress = progressLoading;
                progressLoading = (int)(currentPosition * 100 / contentLength);
                //так как значение от 0 до 100, нет особого смысла повтороно обновлять интерфейс, если значение не изменилось.
                if (oldProgress != progressLoading)
                    StatusService.ProgressLoading = progressLoading;
            }
            double ratioBytesOnMb = 1.049e+6;
            StatusService.FileSize = $"{(100000 / ratioBytesOnMb):0.0} Mb";
            return files;
        }

    }
}
