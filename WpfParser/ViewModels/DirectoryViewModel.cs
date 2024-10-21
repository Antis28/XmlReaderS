using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfParser.ViewModels.Base;

namespace WpfParser.ViewModels
{
    internal class DirectoryViewModel
    {
        private readonly DirectoryInfo _directoryInfo;

        public IEnumerable<DirectoryViewModel> SubDirectories
        {
            get
            {
                try
                {
                    return _directoryInfo
                            .EnumerateDirectories()
                            .Select(dirInfo => new DirectoryViewModel(dirInfo.FullName));
                }
                catch (UnauthorizedAccessException e)
                {

                    Debug.WriteLine(e.Message);
                }
                return Enumerable.Empty<DirectoryViewModel>();
            }
        }

        public IEnumerable<FileViewModel> Files
        {
            get
            {
                try
                {
                    return _directoryInfo
                       .EnumerateFiles()
                       .Select(file => new FileViewModel(file.FullName));
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.Message);
                }
                return Enumerable.Empty<FileViewModel>();
            }
        }

        public IEnumerable<object> DirectoryItems
        {
            get
            {
                try
                {
                    return SubDirectories.Cast<object>().Concat(Files);
                }
                catch (UnauthorizedAccessException e)
                {

                    Debug.WriteLine(e.Message);
                }
                return Enumerable.Empty<object>();
            }
        }

        public string Name => _directoryInfo.Name;
        public string Path => _directoryInfo.FullName;
        public DateTime CreationDate => _directoryInfo.CreationTime;

        public DirectoryViewModel(string path) => _directoryInfo = new DirectoryInfo(path);
    }

    internal class FileViewModel : ViewModel
    {
        private readonly FileInfo _fileInfo;

        public string Name => _fileInfo.Name;
        public string Path => _fileInfo.FullName;
        public DateTime CreationDate => _fileInfo.CreationTime;


        public FileViewModel(string path) => _fileInfo = new FileInfo(path);
    }
}
