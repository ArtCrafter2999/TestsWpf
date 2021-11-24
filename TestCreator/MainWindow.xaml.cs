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
        public TestField CurrentTest { get => _currentTest; set { _currentTest = value; OnPropertyChanged("CurrentTest"); } }

        public byte[] Crypt(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= 1;
            return bytes;
        }

        public List<TestModel> Tests { get; set; } = new List<TestModel>();
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int MaxPoints { get; set; } = 12;
        public bool RandomQuestions { get; set; } = false;
        public string SavePath { get; set; } = null;
        public ICommand AddTest => new RelayCommand(o =>
        {
            var newTest = new TestModel();
            newTest.Answers = new List<Answer>() { new Answer() { Text = "Ответ 1" }, new Answer() { Text = "Ответ 2" } };
            Tests.Add(newTest);
            ReloadTest();
        });
        public ICommand Options => new RelayCommand(o =>
        {
            new TestOptions(this).ShowDialog();
        });
        public ICommand RemoveTest => new RelayCommand(o =>
        {
            Tests.Remove(_currentTest.Test);
            _currentTest.Test = null;
            _currentTest.ReloadTest();
            _currentTest = null;
            ReloadTest();
        }, o => _currentTest != null && Tests.Count > 1);
        public ICommand SaveCommand => new RelayCommand(o =>
        {
            if (SavePath == null)
            {
                var SaveD = new SaveFileDialog();
                SaveD.FileName = Title + ".test";
                SaveD.Filter = "Test File|*.test";
                if (SaveD.ShowDialog() == true)
                {
                    SavePath = SaveD.FileName;
                    Save();
                }
            }
            else
            {
                Save();
            }
            
        });
        public ICommand SaveAsCommand => new RelayCommand(o => { SavePath = null; SaveCommand.Execute(this); });
        public void Save(object o = null)
        {
            _currentTest.ReloadTest();
            var xDoc = new XmlDocument();
            var TestsNode = xDoc.CreateElement("Tests");

            var TitleAttr = xDoc.CreateAttribute("Title");
            var TitleText = xDoc.CreateTextNode(Title);
            TitleAttr.AppendChild(TitleText);

            var TimeAttr = xDoc.CreateAttribute("Time");
            var TimeText = xDoc.CreateTextNode($"{Hours}h{Minutes}m{Seconds}s");
            TimeAttr.AppendChild(TimeText);

            var MaxPointsAttr = xDoc.CreateAttribute("MaxPoints");
            var MaxPointsText = xDoc.CreateTextNode(MaxPoints.ToString());
            MaxPointsAttr.AppendChild(MaxPointsText);

            var RandomQuestionsAttr = xDoc.CreateAttribute("RandomQuestions");
            var RandomQuestionsText = xDoc.CreateTextNode(RandomQuestions.ToString());
            RandomQuestionsAttr.AppendChild(RandomQuestionsText);

            TestsNode.Attributes.Append(TitleAttr);
            TestsNode.Attributes.Append(TimeAttr);
            TestsNode.Attributes.Append(MaxPointsAttr);
            TestsNode.Attributes.Append(RandomQuestionsAttr);

            foreach (var Test in Tests)
            {
                var TestElement = xDoc.CreateElement("Test");

                var MultipleAnswerAttr = xDoc.CreateAttribute("MultipleAnswer");
                var MultipleAnswerText = xDoc.CreateTextNode(Test.MultipleAnswer.ToString());
                MultipleAnswerAttr.AppendChild(MultipleAnswerText);

                var StrictAnswerAttr = xDoc.CreateAttribute("StrictAnswer");
                var StrictAnswerText = xDoc.CreateTextNode(Test.StrictAnswer.ToString());
                StrictAnswerAttr.AppendChild(StrictAnswerText);

                var QuestionElement = xDoc.CreateElement("Question");
                var QuestionText = xDoc.CreateTextNode(Test.Question);
                QuestionElement.AppendChild(QuestionText);

                TestElement.Attributes.Append(MultipleAnswerAttr);
                TestElement.Attributes.Append(StrictAnswerAttr);
                TestElement.AppendChild(QuestionElement);

                foreach (var Answer in Test.Answers)
                {
                    var AnswerElement = xDoc.CreateElement("Answer");

                    var IsCorrectAttr = xDoc.CreateAttribute("IsCorrect");
                    var IsCorrectText = xDoc.CreateTextNode(Answer.IsCorrect.ToString());
                    IsCorrectAttr.AppendChild(IsCorrectText);

                    var AnswerText = xDoc.CreateTextNode(Answer.Text);

                    AnswerElement.Attributes.Append(IsCorrectAttr);
                    AnswerElement.AppendChild(AnswerText);

                    TestElement.AppendChild(AnswerElement);
                }
                TestsNode.AppendChild(TestElement);
            }
            xDoc.AppendChild(TestsNode);
            byte[] DocumentBytes = Encoding.UTF8.GetBytes(xDoc.OuterXml);
            byte[] CryptDocument = Crypt(DocumentBytes);
            File.WriteAllBytes(SavePath, CryptDocument);
        }
        public ICommand NewCommand => new RelayCommand(o =>
        {
            AddTest.Execute(this);
            ReloadTest();
        });
        public ICommand EditCommand => new RelayCommand(o =>
        {
            var Open = new OpenFileDialog();
            Open.Filter = "Test File|*.test";
            if (Open.ShowDialog() == true)
            {
                Edit(Open.FileName);
            }
            else
            {
                Close();
            }
        });

        public void Edit(string FilePath)
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
                Hours = int.Parse(matches[0].Value);
                Minutes = int.Parse(matches[1].Value);
                Seconds = int.Parse(matches[2].Value);
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
                            Test.Answers.Add(new Answer() { Text = ChildNode.InnerText, IsCorrect = bool.Parse(ChildNode.Attributes.GetNamedItem("IsCorrect").Value) });
                        }
                    }
                    Tests.Add(Test);
                }
                SavePath = FilePath;
                ReloadTest();
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
            Title = "Новый тест";
            if (!new OpenOrEditWindow(this).ShowDialog().Value) Close();
        }
        public MainWindow(string FilePath)
        {
            InitializeComponent();
            DataContext = this;
            Edit(FilePath);
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
