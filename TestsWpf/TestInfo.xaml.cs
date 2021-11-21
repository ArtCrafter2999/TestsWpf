using System;
using System.Windows;
using System.Windows.Input;
using Command;

namespace TestsWpf
{
    public partial class TestInfo : Window
    {
        public string UserName { get; set; } = "";
        public ICommand OKButton => new RelayCommand(o => { DialogResult = true; Close(); }, o => UserName != "");
        public TestInfo(MainWindow from)
        {
            InitializeComponent();
            DataContext = this;
            Info.Text = $"Тест \"{from.Title}\"\nНа прохождение {from.TestTime.Hours} часов {from.TestTime.Minutes} минут {from.TestTime.Seconds} секунд\nМаксимальный балл: {from.MaxPoints}";
        }
    }
}
