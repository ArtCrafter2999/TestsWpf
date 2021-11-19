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

namespace TestsWpf
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public TestField CurrentTest { get; set; }
        public ICommand Complite => new RelayCommand(o =>
        {
            if (MessageBox.Show("Вы уверены что хотите зваершить тест?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
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
                        else if (Test.Test.MultipleAnswer && Test.Test.StrictAnswer)
                        {
                            bool Correct = true;
                            foreach (var Answer in Test.UserAnswers)
                            {
                                if (Answer.IsCorrect == false)
                                {
                                    Correct = false;
                                    break;
                                }
                            }
                            if (Correct) PointsSum++;
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
                            PointsSum += res;
                        }
                    }
                }
                MessageBox.Show($"Тест завершён: \nПравильно отвечено на {PointsSum:0.##} вопросов из {Tests.Count}\nПроцент ответов: {PointsSum / Tests.Count * 100:0.##}%\nПолучено балов: {(PointsSum / Tests.Count * MaxPoints):0}/{MaxPoints}", "Тест завершён", MessageBoxButton.OK);
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
                foreach (UIElement button in CurrentTest.Stack.Children)
                {
                    ToggleButton toggleButton = button as ToggleButton;
                    if (toggleButton != null)
                    {
                        if (toggleButton.IsChecked.Value)
                        {
                            CurrentTest.UserAnswers.Add(CurrentTest.Test.Answers.Find(o => o == toggleButton.Content));
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

        public List<TestModel> Tests { get; set; } = new List<TestModel>();
        public string Time;
        public int MaxPoints;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var Open = new OpenFileDialog();
            Open.Filter = "Text File|*.txt|Test File|*.test";
            if (Open.ShowDialog() == true)
            {
                try
                {
                    string xml = File.ReadAllText(Open.FileName);
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xml);
                    XmlElement TestsNode = xDoc.DocumentElement;
                    Title = TestsNode.Attributes.GetNamedItem("Title").Value;
                    Time = TestsNode.Attributes.GetNamedItem("Time").Value;
                    MaxPoints = int.Parse(TestsNode.Attributes.GetNamedItem("MaxPoints").Value);
                    foreach (XmlNode TestNode in TestsNode.ChildNodes)
                    {
                        var Test = new TestModel();
                        Test.MultipleAnswer = bool.Parse(TestNode.Attributes.GetNamedItem("MultipleAnswers").Value);
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
                    ShowTests();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
            else
            {
                Close();
            }

        }

        public void ShowTests()
        {
            int i = 1;
            foreach (var Test in Tests)
            {
                Wrap.Children.Add(new TestButton(i++.ToString(), this, Test));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
