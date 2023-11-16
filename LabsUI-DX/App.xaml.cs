using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LabsUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Instead of StartupUri="Views/MainWindow.xaml", use OnStartup to prepare for Dependency Injection..
            MainWindow win = new MainWindow();
            win.Show();

            base.OnStartup(e);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
        }
    }
}
