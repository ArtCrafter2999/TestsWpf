﻿using System;
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
            From.CurrentTest = this;
            From.OnPropertyChanged("CurrentTest");
        });
        public string ButtonTitle { get; set; }
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
            var random = new Random();
            Stack.Children.Add(new TextBlock() { Text = Test.Question, TextWrapping = TextWrapping.Wrap, FontSize = 20 });
            bool[] getted = new bool[Test.Answers.Count];
            for (int i = 0; i < Test.Answers.Count; i++)
            {
                getted[i] = false;
            }
            bool access = false;
            while (!access)
            {
                access = true;
                int i;
                do
                {
                    i = random.Next(0, Test.Answers.Count);
                } while (getted[i] == true);
                if (Test.MultipleAnswer)
                {
                    Stack.Children.Add(new CheckBox() { Content = Test.Answers[i], FontSize = 16 });
                    getted[i] = true;
                }
                else
                {
                    Stack.Children.Add(new RadioButton() { Content = Test.Answers[i], FontSize = 16 });
                    getted[i] = true;
                }
                foreach (var item in getted)
                {
                    if (item == false)
                    {
                        access = false;
                        break;
                    }
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
