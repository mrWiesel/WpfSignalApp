using System.Windows;

namespace WpfSignalApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.Initialize();

            // Підписуємось на зміну мови для оновлення навігаційних кнопок
            LocalizationManager.LanguageChanged += UpdateNavTexts;
            UpdateNavTexts();

            MainFrame.Navigate(new HomePage());
        }

        /// <summary>
        /// Оновлює текст кнопок навігаційної панелі та tooltip Settings.
        /// Викликається при старті і при зміні мови.
        /// </summary>
        private void UpdateNavTexts()
        {
            TbAppSubtitle.Text = LocalizationManager.Get("nav.appSubtitle");
            BtnHome.Content = LocalizationManager.Get("nav.home");
            BtnPage1.Content = LocalizationManager.Get("nav.catch");
            BtnPage2.Content = LocalizationManager.Get("nav.processing");
            BtnPage3.Content = LocalizationManager.Get("nav.results");
            BtnSettings.ToolTip = LocalizationManager.Get("nav.settings.tip");
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new HomePage());
        private void BtnPage1_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Page1());
        private void BtnPage2_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Page2());
        private void BtnPage3_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Page3());
        private void BtnSettings_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new SettingsPage());
    }
}