using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestCreator
{
    /// <summary>
    /// Логика взаимодействия для TestOptions.xaml
    /// </summary>
    public partial class TestOptions : Window
    {
        public ICommand OptionsClose => new RelayCommand(o => {DialogResult = true; Close(); });
        public MainWindow From { get; set; }
        public TestOptions(MainWindow from)
        {
            InitializeComponent();
            DataContext = this;
            From = from;
        }
    }
}
