using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                while (reader.Read())     // чтение некст элемента
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

                    

                }
            }

            return file;
        }

        public static IEnumerable<ResponseFileViewModel> ReadResponseFiles()
        {
            var fileNames = Directory.GetFiles(Environment.CurrentDirectory, "*.xml", SearchOption.AllDirectories);
            return ReadResponseFiles(fileNames);
        }

        public static IEnumerable<ResponseFileViewModel> ReadResponseFiles(string[] fileNames)
        {
            var files = new List<ResponseFileViewModel>();
            foreach (var xmlFile in fileNames)
            {
                try
                {
                    var file = ReadResponseFile(xmlFile);
                    files.Add(file);
                }
                catch (Exception ex)
                {
                    ConsoleService.GetInstance().ShowMessage($"Ошибка при чтении файла:\n {xmlFile}", ex.Message);
                }

            }

            return files;
        }

    }
}
