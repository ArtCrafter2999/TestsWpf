using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TestsWpf
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool TestComplited = true;
        void App_Startup(object sender, StartupEventArgs e)
        {
            string fileName = e.Args?.FirstOrDefault();
            MainWindow mainWindow;
            if (!string.IsNullOrWhiteSpace(fileName))
                mainWindow = new MainWindow(fileName);
            else
                mainWindow = new MainWindow();

            mainWindow.Show();
        }
        public void WindowDeactivated(object sender, EventArgs e)
        {
            if (!TestComplited)
            {
                Log.Write("Окно не активно");
            }
            
        }
        public void WindowActivated(object sender, EventArgs e)
        {
            if (!TestComplited)
            {
                Log.Write("Окно активно"); // add comment
            }
        }
    }
}
