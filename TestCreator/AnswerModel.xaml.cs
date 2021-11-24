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
        public TestField Test;
        public ICommand Minus => new RelayCommand(o => { Test.Test.Answers.Remove(Answer); Test.ReloadTest(); }, o => Index > 1);
        public int Index;
        public AnswerModel(Answer answer, TestField test)
        {
            InitializeComponent();
            DataContext = this;
            Answer = answer;
            Test = test;
            Index = Test.Test.Answers.FindIndex(o => o == Answer);
        }
    }
}
