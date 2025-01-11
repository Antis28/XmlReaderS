using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
                ReportToRecipient = new List<ReportToRecipientViewModel>()
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
                file.ReportToRecipient.Add(new ReportToRecipientViewModel());
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
                file.ReportToRecipient[numberRecipient].Name = reader.ReadElementContentAsString();
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

        public static IEnumerable<ResponseFileViewModel> ReadResponseFiles()
        {
            var fileNames = Directory.GetFiles(Environment.CurrentDirectory, "*.xml", SearchOption.AllDirectories);
            return ReadResponseFiles(fileNames);
        }

        public static IEnumerable<ResponseFileViewModel> ReadResponseFiles(string[] fileNames)
        {
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
