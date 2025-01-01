using System;
using System.Windows;
using System.Windows.Input;

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
    }
}
