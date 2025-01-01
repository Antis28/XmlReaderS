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
using WpfParser.Services;
using WpfParser.ViewModels;

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

            //frameworkElement.DataContext
            var r = fileListBox.DataContext as AllFilesViewModel;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                try
                {
                    var rf = DataService.ReadResponseFiles(files);
                    var t = r.ResponseFiles;

                    var coll = new ObservableCollection<ResponseFileViewModel>();

                    foreach (var item in t)
                    {
                        coll.Add(item);
                    }

                    foreach (var item in rf)
                    {
                        coll.Add(item);
                    }

                    r.ResponseFiles = coll;
                    r.UpdateShowingNames();

                }
                catch (Exception ex)
                {
                    ConsoleService.GetInstance().ShowMessage("Произошла ошибка!", ex.Message);
                    return;
                }


            }
            base.OnDrop(e);
        }
    }
}
