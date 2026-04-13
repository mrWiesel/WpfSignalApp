using System.Windows;

namespace WpfSignalApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.Initialize();
            MainFrame.Navigate(new HomePage());
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)     => MainFrame.Navigate(new HomePage());
        private void BtnPage1_Click(object sender, RoutedEventArgs e)    => MainFrame.Navigate(new Page1());
        private void BtnPage2_Click(object sender, RoutedEventArgs e)    => MainFrame.Navigate(new Page2());
        private void BtnPage3_Click(object sender, RoutedEventArgs e)    => MainFrame.Navigate(new Page3());
        private void BtnPage4_Click(object sender, RoutedEventArgs e)    => MainFrame.Navigate(new Page4());
        private void BtnSettings_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new SettingsPage());
    }
}
