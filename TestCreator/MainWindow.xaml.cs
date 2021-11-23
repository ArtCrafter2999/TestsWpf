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
using System.Xml;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Command;
using Microsoft.Win32;
using System.IO;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace TestCreator
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private TestField _currentTest;
        public TestField CurrentTest { get { MyGroup.Content = _currentTest; return _currentTest; }
            set { _currentTest = value; OnPropertyChanged("CurrentTest"); MyGroup.Content = _currentTest; } }

        public List<TestModel> Tests { get; set; } = new List<TestModel>();
        public TimeSpan Time => new TimeSpan(Hours, Minutes, Seconds);
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int MaxPoints { get; set; } = 12;
        public bool RandomQuestions { get; set; } = false;
        public TestOptions OptionWindow { get; set; }
        public ICommand AddTest => new RelayCommand(o =>
        {
            var newTest = new TestModel();
            newTest.Answers = new List<Answer>() { new Answer() { Text = "Вопрос 1"}, new Answer() { Text = "Вопрос 2" } };
            Tests.Add(newTest);
            ReloadTest();
        });
        public ICommand OptionsClose => new RelayCommand(o => { OptionWindow.DialogResult = true; OptionWindow.Close(); });
        public ICommand Options => new RelayCommand(o =>
        {
            OptionWindow.ShowDialog();
        });

        public MainWindow()
        {
            InitializeComponent();
            Title = "Новый тест";
            OptionWindow = new TestOptions(this);
            if (!OptionWindow.ShowDialog().Value) Close();
            ReloadTest();
        }
        
        public void ReloadTest()
        {
            Wrap.Children.Clear();
            var i = 1;
            foreach (var Test in Tests)
            {
                Wrap.Children.Add(new TestButton(i++.ToString(), this, Test));
            }
            Wrap.Children.Add(new TestAddButton(this));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
