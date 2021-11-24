using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Command;

namespace TestCreator
{
    public partial class TestField : UserControl, INotifyPropertyChanged
    {
        public ICommand SelectCommand => new RelayCommand(o =>
        {
            var PrevTest = From.CurrentTest;
            PrevTest?.ReloadTest();
            ReloadTest();
            From.CurrentTest = this;
            PrevTest?.OnPropertyChanged("ButtonColor");
            OnPropertyChanged("ButtonColor");
        });
        public ICommand AddAnswer => new RelayCommand(o => 
        {
            var Answer = new Answer();
            Test.Answers.Add(Answer);
            Answer.Text = $"Ответ {Test.Answers.Count}";
            Stack.Children.Add(new AnswerModel(Answer, this));
            ReloadTest();
        });
        public string ButtonTitle { get; set; }
        public string ButtonColor { 
            get 
            {
                if (From?.CurrentTest.testButton == testButton)
                {
                    return "SeaGreen";
                }
                else
                {
                    return "DarkSeaGreen";
                }
            }
        }
        public MainWindow From;
        public TestModel Test;
        public TestButton testButton;
        public TestField(MainWindow fromWindow, TestButton fromButton, TestModel test)
        {
            InitializeComponent();
            DataContext = this;
            testButton = fromButton;
            From = fromWindow;
            Test = test;
            OnPropertyChanged("ButtonColor");
        }

        public string Question { get => Test.Question; set { Test.Question = value;} }
        public bool MultipleAnswer { get => Test.MultipleAnswer;
            set { Test.MultipleAnswer = value; ReloadTest(); } }
        public bool StrictAnswer { get => Test.StrictAnswer; set { Test.StrictAnswer = value; } }
        public void ReloadTest()
        {
            Stack.Children.Clear();
            if (Test!=null)
            {
                var TextBox = new TextBox() { TextWrapping = TextWrapping.Wrap, FontSize = 20, DataContext = this };
                TextBox.SetBinding(TextBox.TextProperty, "Question");
                var text = Stack.Children.Add(TextBox);

                var checkBox = new CheckBox() { Content = "Мульти-ответ" };
                checkBox.SetBinding(ToggleButton.IsCheckedProperty, "MultipleAnswer");
                Stack.Children.Add(checkBox);
                if (Test.MultipleAnswer == true)
                {
                    checkBox = new CheckBox() { Content = "Строгий-ответ" };
                    checkBox.SetBinding(ToggleButton.IsCheckedProperty, "StrictAnswer");
                    Stack.Children.Add(checkBox);
                }
                else
                {
                    Test.StrictAnswer = false;
                }
                Stack.Children.Add(new Separator());
                foreach (var Answer in Test.Answers)
                {
                    Stack.Children.Add(new AnswerModel(Answer, this));
                }
                Stack.Children.Add(new Button() { Content = "+ Добавить вариант ответа", FontSize = 16, Command = AddAnswer });
            }
            
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
