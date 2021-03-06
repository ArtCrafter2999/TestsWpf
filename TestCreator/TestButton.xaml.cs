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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestCreator
{
    /// <summary>
    /// Логика взаимодействия для TestButton.xaml
    /// </summary>
    public partial class TestButton : UserControl
    {
        public TestButton(string Title, MainWindow From, TestModel Question)
        {
            InitializeComponent();
            DataContext = new TestField(From, this, Question) { ButtonTitle = Title };
        }
    }
}
