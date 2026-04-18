using System;
using System.Collections.Generic;

namespace WpfSignalApp
{
    /// <summary>
    /// Singleton-менеджер локалізації.
    /// Зберігає поточну мову і надає доступ до рядків через індексатор.
    /// При зміні мови стріляє подія LanguageChanged — всі ViewModel-и оновлюють свої властивості.
    /// </summary>
    public static class LocalizationManager
    {
        // ── Доступні мови ────────────────────────────────────────────────────────
        public static readonly string[] AvailableLanguages = { "en", "uk" };

        private static string _currentLanguage = "en";
        public static string CurrentLanguage => _currentLanguage;

        // ── Подія зміни мови ─────────────────────────────────────────────────────
        public static event Action? LanguageChanged;

        // ── Словники ─────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, Dictionary<string, string>> _strings = new()
        {
            // Англійська локалізація
            ["en"] = new Dictionary<string, string>
            {
                // MainWindow
                ["nav.appSubtitle"]     = "Mine Radiotelescope App",
                ["nav.home"]            = "🏠 Main",
                ["nav.catch"]           = "📡 Catch\nsignal",
                ["nav.processing"]      = "⚙ Processing",
                ["nav.results"]         = "📊 Results",
                ["nav.settings.tip"]    = "Settings",

                // HomePage
                ["home.title"]          = "Mine Radiotelescope app",
                ["home.subtitle"]       = "Select a section in the navigation bar at the top",

                // Page1
                ["p1.polarity.label"]   = "POLARITY  (deg)",
                ["p1.polarity.title"]   = "Polarity filter:",
                ["p1.polarity.target"]  = "Target:  --- deg",
                ["p1.polarity.current"] = "Current: 0 deg",
                ["p1.frequency.label"]  = "FREQUENCY  (MHz)",
                ["p1.frequency.title"]  = "Frequency filter:",
                ["p1.freq.target"]      = "Target:  --- MHz",
                ["p1.freq.current"]     = "Current: 0 MHz",
                ["p1.status.nosignal"]  = "NO SIGNAL",
                ["p1.status.scanning"]  = "SCANNING...",
                ["p1.status.lock"]      = "LOCK ACQUIRED",
                ["p1.status.caught"]    = "SIGNAL CAUGHT",
                ["p1.detector"]         = "Detector status: Active",
                ["p1.object.none"]      = "Object: none",
                ["p1.object.unknown"]   = "Object: Unknown Anomaly",
                ["p1.quality"]          = "Signal quality: {0}%",
                ["p1.efficiency"]       = "Efficiency: 0.000 B\\s",
                ["p1.energy"]           = "Energy consumption: 100.00%",
                ["p1.btn.init"]         = "INITIALIZE SCANNER",
                ["p1.btn.catch"]        = "CATCH SIGNAL",

                // Page2
                ["p2.title"]            = "⚙ Signal Processing",
                ["p2.status.ready"]     = "Ready to process",
                ["p2.status.processing"]= "Processing...",
                ["p2.status.done"]      = "Signal processed successfully!",
                ["p2.btn.start"]        = "▶  Start Processing",

                // Page3
                ["p3.title"]            = "📊 Signal Result",

                // SettingsPage 
                ["settings.title"]      = "⚙ Settings",
                ["settings.theme"]      = "Interface theme",
                ["settings.dark"]       = "🌙  Dark",
                ["settings.light"]      = "☀  Light",
                ["settings.theme.dark"] = "Current theme: 🌙 Dark",
                ["settings.theme.light"]= "Current theme: ☀ Light",
                ["settings.lang"]       = "Language",
                ["settings.about"]      = "About",
                ["settings.appname"]    = "Mine Radiotelescope app",
                ["settings.version"]    = "Version 1.1.0 BETA",
            },

            // Українська локалізація
            ["uk"] = new Dictionary<string, string>
            {
                // MainWindow
                ["nav.appSubtitle"]     = "Mine Radiotelescope app",
                ["nav.home"]            = "🏠 Головна",
                ["nav.catch"]           = "📡 Зловити\nсигнал",
                ["nav.processing"]      = "⚙ Обробка",
                ["nav.results"]         = "📊 Результат",
                ["nav.settings.tip"]    = "Налаштування",

                // HomePage
                ["home.title"]          = "Mine Radiotelescope app",
                ["home.subtitle"]       = "Оберіть розділ у навігаційній панелі вгорі",

                // Page1
                ["p1.polarity.label"]   = "ПОЛЯРНІСТЬ  (град)",
                ["p1.polarity.title"]   = "Фільтр полярності:",
                ["p1.polarity.target"]  = "Ціль:    --- град",
                ["p1.polarity.current"] = "Поточна: 0 град",
                ["p1.frequency.label"]  = "ЧАСТОТА  (МГц)",
                ["p1.frequency.title"]  = "Фільтр частоти:",
                ["p1.freq.target"]      = "Ціль:    --- МГц",
                ["p1.freq.current"]     = "Поточна: 0 МГц",
                ["p1.status.nosignal"]  = "НЕМАЄ СИГНАЛУ",
                ["p1.status.scanning"]  = "СКАНУВАННЯ...",
                ["p1.status.lock"]      = "СИГНАЛ ЗАХОПЛЕНО",
                ["p1.status.caught"]    = "СИГНАЛ ЗБЕРЕЖЕНО",
                ["p1.detector"]         = "Детектор: Активний",
                ["p1.object.none"]      = "Об'єкт: відсутній",
                ["p1.object.unknown"]   = "Об'єкт: Невідома Аномалія",
                ["p1.quality"]          = "Якість сигналу: {0}%",
                ["p1.efficiency"]       = "Швидкість: 0.000 Б\\с",
                ["p1.energy"]           = "Споживання енергії: 100.00%",
                ["p1.btn.init"]         = "ІНІЦІАЛІЗУВАТИ СКАНЕР",
                ["p1.btn.catch"]        = "ЗЛОВИТИ СИГНАЛ",

                // Page2
                ["p2.title"]            = "⚙ Обробка сигналу",
                ["p2.status.ready"]     = "Готово до обробки",
                ["p2.status.processing"]= "Обробка...",
                ["p2.status.done"]      = "Сигнал оброблено успішно!",
                ["p2.btn.start"]        = "▶  Почати обробку",

                // Page3
                ["p3.title"]            = "📊 Результат сигналу",

                // SettingsPage
                ["settings.title"]      = "⚙ Налаштування",
                ["settings.theme"]      = "Тема інтерфейсу",
                ["settings.dark"]       = "🌙  Темна",
                ["settings.light"]      = "☀  Світла",
                ["settings.theme.dark"] = "Поточна тема: 🌙 Темна",
                ["settings.theme.light"]= "Поточна тема: ☀ Світла",
                ["settings.lang"]       = "Мова",
                ["settings.about"]      = "Про програму",
                ["settings.appname"]    = "Мій Радіотелескоп",
                ["settings.version"]    = "Версія 1.1.0 BETA",
            }
        };

        // Публічний індексатор
        public  string this[string key]
        {
            get
            {
                if (_strings.TryGetValue(_currentLanguage, out var dict) &&
                    dict.TryGetValue(key, out var value))
                    return value;

                // fallback → англійська
                if (_strings["en"].TryGetValue(key, out var fallback))
                    return fallback;

                return $"[{key}]"; // явна помилка, щоб легко помітити
            }
        }

        // ── Зміна мови ────────────────────────────────────────────────────────────
        public static void SetLanguage(string lang)
        {
            if (!Array.Exists(AvailableLanguages, l => l == lang)) return;
            if (_currentLanguage == lang) return;
            _currentLanguage = lang;
            LanguageChanged?.Invoke();
        }

        // ── Хелпер: отримати рядок з форматуванням ───────────────────────────────
        public static string Format(string key, params object[] args)
            => string.Format(LocalizationManager[key], args);
    }
}
