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
using System.Threading;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestsWpf
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public TestField CurrentTest { get; set; }
        public void UserAnswers()
        {
            if (CurrentTest != null)
            {
                bool NotToggled = true;
                int id = 0;
                foreach (UIElement button in CurrentTest.Stack.Children)
                {
                    ToggleButton toggleButton = button as ToggleButton;
                    if (toggleButton != null)
                    {
                        if (toggleButton.IsChecked.Value)
                        {
                            CurrentTest.UserAnswers.Add(CurrentTest.Test.Answers.Find(o=> o == toggleButton.Content));
                            NotToggled = false;
                        }
                        else
                        {
                            id++;
                        }
                    }
                }
                if (NotToggled)
                {
                    CurrentTest.State = TestState.Skipped;
                }
                else
                {
                    CurrentTest.State = TestState.Answered;
                }
                CurrentTest.OnPropertyChanged("ButtonColor");
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Wrap.Children.Add(new TestButton("1", this, new TestModel()
            {
                Question = "Тестовый вопрос",
                MultipleAnswer = false,
                Answers = new List<Answer>()
            {
                new Answer("вариант ответа 1"),
                new Answer("вариант ответа 2"),
                new Answer("вариант ответа 3")
            }
            }));
            Wrap.Children.Add(new TestButton("2", this, new TestModel()
            {
                Question = "Тестовый вопрос 2",
                MultipleAnswer = true,
                Answers = new List<Answer>()
            {
                new Answer("вариант ответа 1"),
                new Answer("вариант ответа 2"),
                new Answer("вариант ответа 3")
            }
            }));

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
