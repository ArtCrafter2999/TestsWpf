using System;
using System.Windows;
using System.Windows.Input;
using Command;
using Microsoft.Win32;

namespace TestCreator
{
    /// <summary>
    /// Логика взаимодействия для OpenOrEditWindow.xaml
    /// </summary>
    public partial class OpenOrEditWindow : Window
    {
        public MainWindow From { get; set; }
        public ICommand NewCommand => new RelayCommand(o => { DialogResult = true; Close(); From.NewCommand.Execute(this); });
        public ICommand EditCommand => new RelayCommand( o=> { DialogResult = true; Close(); From.EditCommand.Execute(this); });
        public ICommand CloseCommand => new RelayCommand( o=> { DialogResult = false; Close(); });

        public OpenOrEditWindow(MainWindow from)
        {
            InitializeComponent();
            DataContext = this;
            From = from;
        }
    }
}
