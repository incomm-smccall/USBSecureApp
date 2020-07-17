using System.Windows;
using USBSecureApp.Models;

namespace USBSecureApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MinimizeToTray.Enable(this);
        }
    }
}
