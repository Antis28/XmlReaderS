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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
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

            var allFilesViewModel = fileListBox.DataContext as AllFilesViewModel;
            allFilesViewModel?.LoadFilesInBase(files);
        }



        private void OpenFileItemOnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = true ,
                FileName = "Document.XML", // Default file name
                InitialDirectory = Environment.CurrentDirectory,
                DefaultExt = ".xml",
                Filter = "xml Files (*.xml)|*.XML",
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,

            };

            bool? response = openFileDialog.ShowDialog();
            if (response == false) return;

            //Get selected files
            string[] files = openFileDialog.FileNames;
            

            var allFilesViewModel = fileListBox.DataContext as AllFilesViewModel;
            allFilesViewModel?.LoadFilesInBase(files);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            personField.Focus();
        }
    }
}
