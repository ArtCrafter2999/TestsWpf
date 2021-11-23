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
using Command;
namespace TestCreator
{
    /// <summary>
    /// Логика взаимодействия для AnswerModel.xaml
    /// </summary>
    public partial class AnswerModel : UserControl
    {
        public Answer Answer { get; set; }
        public UIElementCollection Collection;
        public ICommand Minus => new RelayCommand(o => { Collection.Remove(this);});
        public AnswerModel(Answer answer, UIElementCollection collection)
        {
            InitializeComponent();
            DataContext = this;
            Answer = answer;
            Collection = collection;
        }
    }
}
