using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using WpfParser.Services;
using WpfParser.ViewModels;
using Path = System.IO.Path;

namespace WpfParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //var b = ShowDialog();
            // MessageBox.Show(e.ToString(), "Ошибка!");
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (fileListBox?.Items.Count > 0)
            {
                fileListBox.SelectedIndex = 0;
            }

        }

        private void SetDropCommand(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) { return; }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (LoadNewFilesInBase(files)) return;


            base.OnDrop(e);
        }

       

        private void OpenFileItemOnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Multiselect = true };
            bool? responce = openFileDialog.ShowDialog();
            if (responce == false) return;

            //Get selected files
            string[] files = openFileDialog.FileNames;

            LoadNewFilesInBase(files);
        }

        private bool LoadNewFilesInBase(string[] files)
        {
            var allFilesViewModel = fileListBox.DataContext as AllFilesViewModel;
            string[] fileNames = null;
            try
            {
                if (Path.HasExtension(files[0]))
                {
                    fileNames = files;
                }
                else
                {
                    fileNames = Directory.GetFiles(Environment.CurrentDirectory, "*.xml", SearchOption.AllDirectories);
                }


                var rf = DataService.ReadResponseFiles(fileNames);
                var t = allFilesViewModel.ResponseFiles;

                var coll = new ObservableCollection<ResponseFileViewModel>();

                foreach (var item in t)
                {
                    coll.Add(item);
                }

                foreach (var item in rf)
                {
                    coll.Add(item);
                }

                allFilesViewModel.ResponseFiles = coll;
                allFilesViewModel.UpdateShowingNames();

            }
            catch (Exception ex)
            {
                ConsoleService.GetInstance().ShowMessage("Произошла ошибка!", ex.Message);
                return true;
            }

            return false;
        }
    }
}
