using System;
using System.IO;

namespace Helpers
{
    public static class FileWorker
    {
        public static bool IsDirectory(this string str)
        {
            bool isDirectory = false;
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(str);

            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                isDirectory = true;

            return isDirectory;
        }
    }
}
