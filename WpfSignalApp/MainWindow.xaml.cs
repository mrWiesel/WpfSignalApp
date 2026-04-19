using System.Windows;
using WpfSignalApp.Services;

namespace WpfSignalApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.Initialize();

            LocalizationManager.LanguageChanged += UpdateNavTexts;
            UpdateNavTexts();

            MainFrame.Navigate(new HomePage());
        }

        private void UpdateNavTexts()
        {
            TbAppSubtitle.Text    = LocalizationManager.Get("nav.appSubtitle");
            BtnHome.Content       = LocalizationManager.Get("nav.home");
            BtnPage1.Content      = LocalizationManager.Get("nav.catch");
            BtnPage2.Content      = LocalizationManager.Get("nav.processing");
            BtnPage3.Content      = LocalizationManager.Get("nav.results");
            BtnSendSignal.Content = LocalizationManager.Get("nav.send");
            BtnSettings.ToolTip   = LocalizationManager.Get("nav.settings.tip");
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)     => MainFrame.Navigate(new HomePage());
        private void BtnPage1_Click(object sender, RoutedEventArgs e)    => MainFrame.Navigate(new Page1());
        private void BtnPage2_Click(object sender, RoutedEventArgs e)    => MainFrame.Navigate(new Page2());
        private void BtnPage3_Click(object sender, RoutedEventArgs e)    => MainFrame.Navigate(new Page3());
        private void BtnSettings_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new SettingsPage());

        private void BtnSendSignal_Click(object sender, RoutedEventArgs e)
        {
            // Guard: require login
            if (SessionManager.IsLoggedIn)
                MainFrame.Navigate(new SendSignalPage());
            else
                MainFrame.Navigate(new AuthPage());
        }
    }
}
