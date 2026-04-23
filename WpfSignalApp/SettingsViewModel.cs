using System.Windows.Media;
using System.Windows;

namespace WpfSignalApp.ViewModels
{
    /// <summary>
    /// ViewModel для SettingsPage.
    /// Кнопки та текст теми прив'язані через Binding — без прямого звернення до контролів.
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        private string _currentThemeText = "Поточна тема: 🌙 Темна";
        private Brush  _darkBtnBg;
        private Brush  _lightBtnBg;

        public string CurrentThemeText
        {
            get => _currentThemeText;
            set => SetField(ref _currentThemeText, value);
        }

        public Brush DarkBtnBackground
        {
            get => _darkBtnBg;
            set => SetField(ref _darkBtnBg, value);
        }

        public Brush LightBtnBackground
        {
            get => _lightBtnBg;
            set => SetField(ref _lightBtnBg, value);
        }

        public SettingsViewModel()
        {
            // Підписуємось на зміни теми — VM оновлює свої властивості, UI реагує через Binding
            ThemeManager.ThemeChanged += Refresh;
            Refresh();
        }

        public void SetDark()
        {
            ThemeManager.SetDark();
            // Refresh() викличеться через ThemeChanged
        }

        public void SetLight()
        {
            ThemeManager.SetLight();
        }

        private void Refresh()
        {
            bool dark = ThemeManager.IsDark;

            var defaultBg = Application.Current.Resources["ThemeBtnBg"] as Brush
                            ?? Brushes.Gray;

            DarkBtnBackground  = dark
                ? new SolidColorBrush(Color.FromRgb(70, 70, 120))
                : defaultBg;

            LightBtnBackground = !dark
                ? new SolidColorBrush(Color.FromRgb(200, 180, 80))
                : defaultBg;

            CurrentThemeText = dark ? "Поточна тема: 🌙 Темна" : "Поточна тема: ☀ Світла";
        }
    }
}
