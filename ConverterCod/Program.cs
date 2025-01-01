using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConverterCod
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Encoding encoding1251 = Encoding.GetEncoding("windows-1251");
            Encoding encoding866 = Encoding.GetEncoding(866);
            Encoding defaultEncodingIfNoBom = Encoding.GetEncoding(866);
            Encoding detectEncoding = Encoding.GetEncoding("windows-1251");
            var endMarker = ".e.txt";
            //var rusAlp = new List<char>()
            //{
            //    ''Ђ'''‘ћ’€Ќ Ћ‹…Љ‘ЂЌ„ђ ЏЂЌЂ‘Ћ‚€—

            //};
            //'о',  'и', 'н', 'т', 'с', 'р',  'л', 'к', 'м',   'у','е', 'а','в','д','п',
            //var text1 =
            //    "Как эффективнее всего привлечь к себе этих людей? Создать собственную биржу контента. Алгоритм проверки уникальности стал, по сути, классным лид–магнитом, который вскоре вырос в самостоятельный качественный продукт.\r\n\r\nПрим. ред: Теперь text.ru – это мультиинструмент проверки текстового контента. Появились функции проверки на плагиат, расширение для браузера, телеграм-бот, проверка сайта и документа и даже собственное API проверки – некоторые из перечисленных услуг Text.ru сделал платными.";


            //var pa = Environment.CurrentDirectory + "\\t.txt";
            ////полная перезапись файла
            //using (StreamWriter writer = new StreamWriter(pa, false, Encoding.GetEncoding(866)))
            //{
            //    await writer.WriteLineAsync(text1);
            //}
            try
            {
                var fileNames = Directory
                    .GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(file =>
                               file.ToLower().EndsWith(".txt")
                              || file.ToLower().EndsWith(".1ls"))
                               .ToArray();

                foreach (var path in fileNames)
                {
                    if (path.EndsWith(endMarker))
                    {
                        continue;
                    }
                    string text;
                    using (StreamReader reader = new StreamReader(path, encoding866))
                    {
                        text = await reader.ReadToEndAsync();
                        Console.WriteLine(text);
                    }

                    byte[] bytes = encoding866.GetBytes(text);

                    byte[] resultBytes = Encoding.Convert(encoding866, encoding1251, bytes);
                    //byte[] resultBytes = Encoding.Convert(encoding866, Encoding.UTF8, bytes);
                    var myString = encoding1251.GetString(resultBytes);

                    // полная перезапись файла
                    var newName =Path.GetFileNameWithoutExtension(path) + endMarker;
                    File.Delete(path);
                    using (StreamWriter writer = new StreamWriter(newName, false, encoding1251))
                    {
                        await writer.WriteLineAsync(myString);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        private static bool pred(string arg)
        {
            var b = arg.EndsWith(".txt");
            return b;
        }
    }
}
