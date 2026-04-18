using System.Windows.Media;
using System.Windows;

namespace WpfSignalApp.ViewModels
{
    /// <summary>
    /// ViewModel для SettingsPage.
    /// Керує темою і мовою через LocalizationManager / ThemeManager.
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        // ── Тема ─────────────────────────────────────────────────────────────────
        private string _currentThemeText = "";
        private Brush  _darkBtnBg  = Brushes.Gray;
        private Brush  _lightBtnBg = Brushes.Gray;

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

        // ── Мова ─────────────────────────────────────────────────────────────────
        private Brush _enBtnBg  = Brushes.Gray;
        private Brush _ukBtnBg  = Brushes.Gray;

        public Brush EnBtnBackground
        {
            get => _enBtnBg;
            set => SetField(ref _enBtnBg, value);
        }

        public Brush UkBtnBackground
        {
            get => _ukBtnBg;
            set => SetField(ref _ukBtnBg, value);
        }

        // ── Локалізовані лейбли ───────────────────────────────────────────────────
        public string SettingsTitleText  => LocalizationManager["settings.title"];
        public string ThemeSectionText   => LocalizationManager["settings.theme"];
        public string DarkBtnText        => LocalizationManager["settings.dark"];
        public string LightBtnText       => LocalizationManager["settings.light"];
        public string LangSectionText    => LocalizationManager["settings.lang"];
        public string AboutSectionText   => LocalizationManager["settings.about"];
        public string AppNameText        => LocalizationManager["settings.appname"];
        public string VersionText        => LocalizationManager["settings.version"];

        // ── Конструктор ──────────────────────────────────────────────────────────
        public SettingsViewModel()
        {
            ThemeManager.ThemeChanged          += Refresh;
            LocalizationManager.LanguageChanged += OnLanguageChanged;
            Refresh();
        }

        // ── Команди теми ─────────────────────────────────────────────────────────
        public void SetDark()  => ThemeManager.SetDark();
        public void SetLight() => ThemeManager.SetLight();

        // ── Команди мови ─────────────────────────────────────────────────────────
        public void SetEnglish()   => LocalizationManager.SetLanguage("en");
        public void SetUkrainian() => LocalizationManager.SetLanguage("uk");

        // ── Оновлення ────────────────────────────────────────────────────────────
        private void Refresh()
        {
            bool dark = ThemeManager.IsDark;

            var defaultBg = Application.Current.Resources["ThemeBtnBg"] as Brush ?? Brushes.Gray;

            DarkBtnBackground  = dark
                ? new SolidColorBrush(Color.FromRgb(70, 70, 120))
                : defaultBg;

            LightBtnBackground = !dark
                ? new SolidColorBrush(Color.FromRgb(200, 180, 80))
                : defaultBg;

            CurrentThemeText = dark
                ? LocalizationManager["settings.theme.dark"]
                : LocalizationManager["settings.theme.light"];

            RefreshLangButtons();
        }

        private void RefreshLangButtons()
        {
            var defaultBg = Application.Current.Resources["ThemeBtnBg"] as Brush ?? Brushes.Gray;
            bool isEn = LocalizationManager.CurrentLanguage == "en";

            EnBtnBackground = isEn
                ? new SolidColorBrush(Color.FromRgb(30, 100, 180))
                : defaultBg;

            UkBtnBackground = !isEn
                ? new SolidColorBrush(Color.FromRgb(30, 100, 180))
                : defaultBg;
        }

        private void OnLanguageChanged()
        {
            // Оновити всі computed лейбли
            OnPropertyChanged(nameof(SettingsTitleText));
            OnPropertyChanged(nameof(ThemeSectionText));
            OnPropertyChanged(nameof(DarkBtnText));
            OnPropertyChanged(nameof(LightBtnText));
            OnPropertyChanged(nameof(LangSectionText));
            OnPropertyChanged(nameof(AboutSectionText));
            OnPropertyChanged(nameof(AppNameText));
            OnPropertyChanged(nameof(VersionText));

            // Тема-текст теж локалізований
            CurrentThemeText = ThemeManager.IsDark
                ? LocalizationManager["settings.theme.dark"]
                : LocalizationManager["settings.theme.light"];

            RefreshLangButtons();
        }
    }
}
