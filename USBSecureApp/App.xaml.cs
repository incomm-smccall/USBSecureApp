using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using USBSecureApp.ViewModels;

namespace USBSecureApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private System.Windows.Forms.NotifyIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            BuildRegistry();

            base.OnStartup(e);
            MainWindow window = new MainWindow();
            var viewModel = new MainWindowViewModel();
            window.DataContext = viewModel;

            //_notifyIcon = new System.Windows.Forms.NotifyIcon();
            //_notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(HandleBalloonClick);
            //_notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
            //_notifyIcon.Visible = true;

            window.Show();
        }

        //private void HandleBalloonClick(object sender, EventArgs e)
        //{
        //    if (MainWindow.IsVisible)
        //    {
        //        MainWindow.WindowState = WindowState.Minimized;
        //        MainWindow.ShowInTaskbar = false;
        //        MainWindow.Hide();
        //    }
        //    else
        //    {
        //        MainWindow.WindowState = WindowState.Normal;
        //        MainWindow.ShowInTaskbar = true;
        //        MainWindow.Activate();
        //        MainWindow.Show();
        //    }
        //}
                
        private void BuildRegistry()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\USBSecure");
            if (key == null)
            {
                RegistryKey uKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                RegistryKey subKey = uKey.CreateSubKey("USBSecure");
                if (subKey != null)
                {
                    subKey.SetValue("IgnoreUSB", "True");
                    subKey.SetValue("SerialNumber", "");
                }
            }
        }
    }
}
