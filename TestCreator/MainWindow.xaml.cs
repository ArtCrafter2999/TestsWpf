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
        public TestField CurrentTest { get; set; }
        public TimeSpan Time => new TimeSpan(Hours, Minutes, Seconds);
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int MaxPoints { get; set; } = 12;
        public bool RandomQuestions { get; set; } = false;
        public TestOptions OptionWindow;
        public ICommand OptionsClose => new RelayCommand(o => { OptionWindow.Close(); });
        public ICommand Options => new RelayCommand(o =>
        {
            OptionWindow.ShowDialog();
        });

        public MainWindow()
        {
            InitializeComponent();
            Title = "Новый тест";
            OptionWindow = new TestOptions(this);
            OptionWindow.ShowDialog();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
