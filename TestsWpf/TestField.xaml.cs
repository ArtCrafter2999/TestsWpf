using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Command;

namespace TestsWpf
{
    public enum TestState
    {
        NotViewed,
        Slected,
        Skipped,
        Answered
    }
    public partial class TestField : UserControl, INotifyPropertyChanged
    {
        private TestState _state = TestState.NotViewed;
        public TestState State { get => _state; set { _state = value; OnPropertyChanged("ButtonColor"); } }
        public ICommand SelectCommand => new RelayCommand(o =>
        {
            From.UserAnswers();
            From.CurrentTest = this;
            State = TestState.Slected;
            OnPropertyChanged("ButtonColor");
            From.OnPropertyChanged("CurrentTest");
        });
        public string ButtonTitle { get; set; }
        public string ButtonColor
        {
            get
            {
                switch (_state)
                {
                    case TestState.NotViewed:
                        return "WhiteSmoke";
                    case TestState.Slected:
                        return "Blue";
                    case TestState.Skipped:
                        return "DarkGray";
                    case TestState.Answered:
                        return "LightGreen";
                    default:
                        return "WhiteSmoke";
                }
            }
        }
        public List<Answer> UserAnswers = new List<Answer>();
        public MainWindow From;
        public TestModel Test;
        public TestField(MainWindow from, TestModel Question)
        {
            InitializeComponent();
            DataContext = this;
            From = from;
            Test = Question;
            ReloadTest();
        }
        public void ReloadTest()
        {
            Stack.Children.Add(new TextBlock() { Text = Test.Question, IsHyphenationEnabled = true, FontSize=20 });
            foreach (var Answer in Test.Answers)
            {
                if (Test.MultipleAnswer)
                {
                    Stack.Children.Add(new CheckBox() { Content = Answer, FontSize = 16 });
                }
                else
                {
                    Stack.Children.Add(new RadioButton() { Content = Answer, FontSize = 16 });
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
