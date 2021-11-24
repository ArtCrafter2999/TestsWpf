using System;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Command;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Linq;

namespace TestsWpf
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public TestField CurrentTest { get; set; }
        public ICommand Complite => new RelayCommand(o =>
        {
            
            Log.Write("Предупреждение: \"Вы уверены что хотите зваершить тест ? \"");
            if (MessageBox.Show("Вы уверены что хотите зваершить тест?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.TestComplited = true;
                Close();
                UserAnswers();
                decimal PointsSum = 0m;
                foreach (TestButton TestButton in Wrap.Children)
                {
                    var Test = TestButton.DataContext as TestField;
                    if (Test.UserAnswers.Count != 0)
                    {
                        if (!Test.Test.MultipleAnswer)
                        {
                            if (Test.UserAnswers[0].IsCorrect) PointsSum++;
                        }
                        else
                        {
                            decimal Correct = 0;
                            decimal Incorrect = 0;
                            decimal res = 0;
                            foreach (var Answer in Test.Test.Answers)
                            {
                                if (Answer.IsCorrect) Correct++;
                                else Incorrect++;
                            }
                            foreach (var Answer in Test.UserAnswers)
                            {
                                if (Answer.IsCorrect) res += 1m / Correct;
                                else res -= 1m / Incorrect;
                            }
                            if (res < 0) res = 0;
                            if (Test.Test.StrictAnswer) res = (int)res;
                            PointsSum += res;
                        }
                    }
                }
                Log.Write($"Тест завершён: Правильно отвечено на {PointsSum:0.##} вопросов из {Tests.Count}");
                MessageBox.Show($"<{Log.Name}> Тест завершён: \nВремя: {_timePassed.ToString(@"hh\:mm\:ss")}\nПравильно отвечено на {PointsSum:0.##} вопросов из {Tests.Count}\nПроцент ответов: {PointsSum / Tests.Count * 100:0.##}%\nПолучено балов: {(PointsSum / Tests.Count * MaxPoints):0}/{MaxPoints}", "Тест завершён", MessageBoxButton.OK);
            }
        }, o =>
        {
            foreach (TestButton TestButton in Wrap.Children)
            {
                var Test = TestButton.DataContext as TestField;
                if (Test.State == TestState.NotViewed)
                {
                    return false;
                }
            }
            return true;
        });
        public void UserAnswers()
        {
            if (CurrentTest != null)
            {
                bool NotToggled = true;
                int id = 0;
                string AnswersStr = "";
                foreach (UIElement button in CurrentTest.Stack.Children)
                {
                    ToggleButton toggleButton = button as ToggleButton;
                    if (toggleButton != null)
                    {
                        if (toggleButton.IsChecked.Value)
                        {
                            CurrentTest.UserAnswers.Add(CurrentTest.Test.Answers.Find(o => o == toggleButton.Content));
                            if (AnswersStr != "") AnswersStr += ",";
                            AnswersStr += "\"" + CurrentTest.UserAnswers.Last().ToString() + "\"";
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
                    Log.Write($"Вопрос \"{CurrentTest.Test.Question}\" Пропущен");
                }
                else
                {
                    CurrentTest.State = TestState.Answered;
                    Log.Write($"Вопрос \"{CurrentTest.Test.Question}\". Ответы: {AnswersStr}");
                }
                CurrentTest.OnPropertyChanged("ButtonColor");
            }
        }

        public byte[] Crypt(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= 1;
            return bytes;
        }

        public List<TestModel> Tests { get; set; } = new List<TestModel>();
        public string Time;
        public int MaxPoints;
        public bool RandomQuestions;
        public void Start(string FilePath)
        {
            try
            {
                byte[] ByteDocument = File.ReadAllBytes(FilePath);
                string xml = Encoding.UTF8.GetString(Crypt(ByteDocument));
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xml);
                XmlElement TestsNode = xDoc.DocumentElement;
                Title = TestsNode.Attributes.GetNamedItem("Title").Value;
                var Time = TestsNode.Attributes.GetNamedItem("Time").Value;
                var matches = Regex.Matches(Time, @"[^hms]+");
                TestTime = new TimeSpan(int.Parse(matches[0].Value), int.Parse(matches[1].Value), int.Parse(matches[2].Value));
                MaxPoints = int.Parse(TestsNode.Attributes.GetNamedItem("MaxPoints").Value);
                RandomQuestions = bool.Parse(TestsNode.Attributes.GetNamedItem("RandomQuestions").Value);
                foreach (XmlNode TestNode in TestsNode.ChildNodes)
                {
                    var Test = new TestModel();
                    Test.MultipleAnswer = bool.Parse(TestNode.Attributes.GetNamedItem("MultipleAnswer").Value);
                    Test.StrictAnswer = bool.Parse(TestNode.Attributes.GetNamedItem("StrictAnswer").Value);
                    foreach (XmlNode ChildNode in TestNode.ChildNodes)
                    {
                        if (ChildNode.Name == "Question")
                        {
                            Test.Question = ChildNode.InnerText;
                        }
                        else if (ChildNode.Name == "Answer")
                        {
                            Test.Answers.Add(new Answer(ChildNode.InnerText) { IsCorrect = bool.Parse(ChildNode.Attributes.GetNamedItem("IsCorrect").Value) });
                        }
                    }
                    Tests.Add(Test);
                }
                var InfoW = new TestInfo(this);
                if (InfoW.ShowDialog() == true)
                {
                    Log.Name = InfoW.UserName;
                }
                else
                {
                    Close();
                }
                ShowTests();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
            var Open = new OpenFileDialog();
            Open.Filter = "Test File|*.test";
            if (Open.ShowDialog() == true)
            {
                Start(Open.FileName);
                App.TestComplited = false;
            }
            else
            {
                Close();
            }
        }
        public MainWindow(string FilePath)
        {
            InitializeComponent();
            DataContext = this;
            Start(FilePath);
            App.TestComplited = false;
        }

        private DispatcherTimer dispatcherTimer;
        public void ShowTests()
        {
            Log.Write($"Тест \"{Title}\" запущен");
            if (RandomQuestions)
            {
                var random = new Random();
                bool[] getted = new bool[Tests.Count];
                for (int i = 0; i < Tests.Count; i++)
                {
                    getted[i] = false;
                }
                bool access = false;
                for (int num = 1; num <= Tests.Count;)
                {
                    while (!access)
                    {
                        access = true;
                        int i;
                        do
                        {
                            i = random.Next(0, Tests.Count);
                        } while (getted[i] == true);
                        Wrap.Children.Add(new TestButton(num++.ToString(), this, Tests[i]));
                        getted[i] = true;
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

            }
            else
            {
                int i = 1;
                foreach (var Test in Tests)
                {
                    Wrap.Children.Add(new TestButton(i++.ToString(), this, Test));
                }
            }
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            OnPropertyChanged("TimeString");
        }

        TimeSpan _timePassed = new TimeSpan(0, 0, 0);
        public TimeSpan TestTime = new TimeSpan(0, 0, 0);
        public string TimeString { get => _timePassed.ToString(@"hh\:mm\:ss")+"\\"+TestTime.ToString(@"hh\:mm\:ss"); }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            _timePassed += dispatcherTimer.Interval;
            if (_timePassed >= TestTime && !App.TestComplited)
            {
                Close();
                MessageBox.Show("Тест не здан вовремя", "Тест не сдан", MessageBoxButton.OK, MessageBoxImage.Information);
                Log.Write("Тест не здан вовремя");
            }
            OnPropertyChanged("TimeString");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
