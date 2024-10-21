using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleReaderXML
{
    internal class Program
    {
        private static readonly string _path =
            //"OUT-700-Y-2024-ORG-093-009-000003-DIS-017-DCK-90404-001-DOC-ONVZ-FSB-8458-OUTNMB-0000261093.XML";
            "OUT-700-Y-2024-ORG-093-009-000003-DIS-053-DCK-90405-001-DOC-ONVZ-FSB-8458-OUTNMB-0000261095.XML";
        static void Main(string[] args)
        {
            var t =Directory.GetFiles(Environment.CurrentDirectory,"*.xml");
            var files = new List<ResponseFile>();
            foreach (var xmlFile in t)
            {
                var file = ReadResponseFile(xmlFile);
                files.Add(file);
            }

            foreach (var file in files)
            {
                Console.WriteLine(file.FileName);
                foreach (var item in file.ReportToRecipient)
                {
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine(item.AreaCode);
                    Console.WriteLine(item.Snils);
                    Console.WriteLine(item.Surname);
                    Console.WriteLine(item.Name);
                    Console.WriteLine(item.Patronymic);
                    Console.WriteLine(item.CodeNoReturn);

                    Console.WriteLine(new string('-', 80));
                }
            }
        }

        private static ResponseFile ReadResponseFile(string path)
        {
            var file = new ResponseFile
            {
                ReportToRecipient = new List<ReportToRecipient>()
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
                        file.ReportToRecipient.Add(new ReportToRecipient());
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
                    };
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Отчество")
                    {
                        file.ReportToRecipient[numberRecipient].Patronymic = reader.ReadElementContentAsString();
                    };
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ДатаСмерти")
                    {
                        file.ReportToRecipient[numberRecipient].DateOfDeath = reader.ReadElementContentAsString();
                    };
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ОтзываемаяСумма")
                    {
                        file.ReportToRecipient[numberRecipient].SumRecalled = reader.ReadElementContentAsString();
                    };
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "КодНевозврата")
                    {
                        file.ReportToRecipient[numberRecipient].CodeNoReturn = reader.ReadElementContentAsString();
                    };


                }
            }

            return file;
        }
    }
}
