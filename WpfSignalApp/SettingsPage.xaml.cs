using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfSignalApp
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            UpdateButtonStates();
        }

        private void BtnDark_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SetDark();
            UpdateButtonStates();
        }

        private void BtnLight_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SetLight();
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool dark = ThemeManager.IsDark;

            // Highlight active button
            BtnDark.Background  = dark
                ? new SolidColorBrush(Color.FromRgb(70, 70, 120))
                : (SolidColorBrush)Application.Current.Resources["ThemeBtnBg"];

            BtnLight.Background = !dark
                ? new SolidColorBrush(Color.FromRgb(200, 180, 80))
                : (SolidColorBrush)Application.Current.Resources["ThemeBtnBg"];

            BtnDark.Foreground  = (SolidColorBrush)Application.Current.Resources["ThemeBtnFg"];
            BtnLight.Foreground = (SolidColorBrush)Application.Current.Resources["ThemeBtnFg"];

            TxtCurrentTheme.Text = dark ? "Поточна тема: 🌙 Темна" : "Поточна тема: ☀ Світла";
        }
    }
}
