using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Command;

namespace TestCreator
{
    public partial class TestField : UserControl, INotifyPropertyChanged
    {
        public ICommand SelectCommand => new RelayCommand(o =>
        {
            ReloadTest();
            From.CurrentTest = this;
        });
        public ICommand AddAnswer => new RelayCommand(o => 
        {
            var Answer = new Answer();
            Test.Answers.Add(Answer);
            Stack.Children.Add(new AnswerModel(Answer, Stack.Children));
            ReloadTest();
        });
        public string ButtonTitle { get; set; }
        public MainWindow From;
        public TestModel Test;
        public TestField(MainWindow from, TestModel test)
        {
            InitializeComponent();
            DataContext = this;
            From = from;
            Test = test;
        }
        public void ReloadTest()
        {
            Form();
            Stack.Children.Clear();
            Stack.Children.Add(new TextBox() { Text = Test.Question, TextWrapping = TextWrapping.Wrap, FontSize = 20 });
            Stack.Children.Add(new CheckBox() { Content = "Мульти-ответ", IsChecked = Test.MultipleAnswer });
            if (Test.MultipleAnswer == true)
            {
                Stack.Children.Add(new CheckBox() { Content = "Строгий-ответ", IsChecked = Test.StrictAnswer });
            }
            else
            {
                Test.StrictAnswer = false;
            }
            Stack.Children.Add(new Separator());
            foreach (var Answer in Test.Answers)
            {
                Stack.Children.Add(new AnswerModel(Answer, Stack.Children));
            }
            Stack.Children.Add(new Button() { Content = "+ Добавить вопрос", FontSize = 16, Command = AddAnswer });
        }
        public void Form()
        {
            if (Stack.Children.Count != 0)
            {
                Test = new TestModel();
                Test.Question = (Stack.Children[0] as TextBox).Text;
                Test.MultipleAnswer = (Stack.Children[1] as CheckBox).IsChecked.Value;
                if (Test.MultipleAnswer)
                {
                    Test.StrictAnswer = (Stack.Children[2] as CheckBox).IsChecked.Value;
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
