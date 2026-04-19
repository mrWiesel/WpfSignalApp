using System;
using System.Collections.Generic;

namespace WpfSignalApp
{
    /// <summary>
    /// Менеджер локалізації.
    /// Зберігає поточну мову і надає доступ до рядків через статичний метод Get().
    /// При зміні мови стріляє подія LanguageChanged.
    /// </summary>
    public static class LocalizationManager
    {
        // Доступні мови
        public static readonly string[] AvailableLanguages = { "en", "uk" };

        private static string _currentLanguage = "en";
        public static string CurrentLanguage => _currentLanguage;

        //Подія зміни мови
        public static event Action? LanguageChanged;

        // Словники
        private static readonly Dictionary<string, Dictionary<string, string>> _strings = new()
        {
            // Англійська локалізація
            ["en"] = new Dictionary<string, string>
            {
                // MainWindow / Navigation
                ["nav.appSubtitle"] = "Mine Radiotelescope App",
                ["nav.home"] = "🏠 Main",
                ["nav.catch"] = "📡 Catch\nsignal",
                ["nav.processing"] = "⚙ Processing",
                ["nav.results"] = "📊 Results",
                ["nav.settings.tip"] = "Settings",
                ["nav.send"] = "📨 Send\nsignal",

                // HomePage
                ["home.title"] = "Mine Radiotelescope app",
                ["home.subtitle"] = "Select a section in the navigation bar at the top",

                // Page1
                ["p1.polarity.label"] = "POLARITY  (deg)",
                ["p1.polarity.title"] = "Polarity filter:",
                ["p1.polarity.target"] = "Target:  --- deg",
                ["p1.polarity.current"] = "Current: 0 deg",
                ["p1.frequency.label"] = "FREQUENCY  (MHz)",
                ["p1.frequency.title"] = "Frequency filter:",
                ["p1.freq.target"] = "Target:  --- MHz",
                ["p1.freq.current"] = "Current: 0 MHz",
                ["p1.status.nosignal"] = "NO SIGNAL",
                ["p1.status.scanning"] = "SCANNING...",
                ["p1.status.lock"] = "LOCK ACQUIRED",
                ["p1.status.caught"] = "SIGNAL CAUGHT",
                ["p1.detector"] = "Detector status: Active",
                ["p1.object.none"] = "Object: none",
                ["p1.object.unknown"] = "Object: Unknown Anomaly",
                ["p1.quality"] = "Signal quality: {0}%",
                ["p1.efficiency"] = "Efficiency: 0.000 B\\s",
                ["p1.energy"] = "Energy consumption: 100.00%",
                ["p1.btn.init"] = "INITIALIZE SCANNER",
                ["p1.btn.catch"] = "CATCH SIGNAL",

                // Page2
                ["p2.title"] = "⚙ Signal Processing",
                ["p2.status.ready"] = "Ready to process",
                ["p2.status.processing"] = "Processing...",
                ["p2.status.done"] = "Signal processed successfully!",
                ["p2.btn.start"] = "▶  Start Processing",

                // Page3
                ["p3.title"] = "📊 Signal Result",

                // SettingsPage
                ["settings.title"] = "⚙ Settings",
                ["settings.theme"] = "Interface theme",
                ["settings.dark"] = "🌙  Dark",
                ["settings.light"] = "☀  Light",
                ["settings.theme.dark"] = "Current theme: 🌙 Dark",
                ["settings.theme.light"] = "Current theme: ☀ Light",
                ["settings.lang"] = "Language",
                ["settings.about"] = "About",
                ["settings.appname"] = "Mine Radiotelescope app",
                ["settings.version"] = "Version 1.2.0",

                // AuthPage
                ["auth.title"] = "Access Required",
                ["auth.subtitle"] = "Register or log in to send signals",
                ["auth.tab.login"] = "Log In",
                ["auth.tab.register"] = "Register",
                ["auth.username"] = "Username",
                ["auth.email"] = "Email",
                ["auth.password"] = "Password",
                ["auth.btn.login"] = "Log In",
                ["auth.btn.register"] = "Register",
                ["auth.err.empty"] = "Please fill in all fields.",
                ["auth.err.exists"] = "Username or email already exists.",
                ["auth.err.invalid"] = "Invalid username or password.",
                ["auth.ok.registered"] = "Registered! You are now logged in.",
                ["auth.ok.loggedin"] = "Welcome back, {0}!",

                // SendSignalPage
                ["send.title"] = "📨 Send Signal",
                ["send.user"] = "Logged in as: {0}",
                ["send.label.autoresponder"] = "Signal Autoresponder",
                ["send.autoresponder.text"] =
                    "📡 SIGNAL AUTORESPONDER ONLINE\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "System: MRtA Signal Gateway v2.1\n" +
                    "Status: READY TO RECEIVE\n" +
                    "Mode: Autonomous Response\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "All transmissions are acknowledged.\n" +
                    "Your signal will be confirmed.\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "Awaiting transmission...",
                ["send.btn.send"] = "📡 Send Last Signal",
                ["send.btn.sending"] = "Transmitting...",
                ["send.countdown"] = "Signal delivered in {0}s...",
                ["send.thanks"] =
                    "✅ TRANSMISSION COMPLETE\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "Thank you for sending your signal!\n" +
                    "Your transmission has been received.\n\n" +
                    "Signal ID: {0}\n" +
                    "Timestamp: {1}\n" +
                    "Status: CONFIRMED\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━",
                ["send.logout"] = "Log Out",
            },

            // Українська локалізація
            ["uk"] = new Dictionary<string, string>
            {
                // MainWindow / Navigation
                ["nav.appSubtitle"] = "Mine Radiotelescope app",
                ["nav.home"] = "🏠 Головна",
                ["nav.catch"] = "📡 Зловити\nсигнал",
                ["nav.processing"] = "⚙ Обробка",
                ["nav.results"] = "📊 Результат",
                ["nav.settings.tip"] = "Налаштування",
                ["nav.send"] = "📨 Надіслати\nсигнал",

                // HomePage
                ["home.title"] = "Mine Radiotelescope app",
                ["home.subtitle"] = "Оберіть розділ у навігаційній панелі вгорі",

                // Page1
                ["p1.polarity.label"] = "ПОЛЯРНІСТЬ  (град)",
                ["p1.polarity.title"] = "Фільтр полярності:",
                ["p1.polarity.target"] = "Ціль:    --- град",
                ["p1.polarity.current"] = "Поточна: 0 град",
                ["p1.frequency.label"] = "ЧАСТОТА  (МГц)",
                ["p1.frequency.title"] = "Фільтр частоти:",
                ["p1.freq.target"] = "Ціль:    --- МГц",
                ["p1.freq.current"] = "Поточна: 0 МГц",
                ["p1.status.nosignal"] = "НЕМАЄ СИГНАЛУ",
                ["p1.status.scanning"] = "СКАНУВАННЯ...",
                ["p1.status.lock"] = "СИГНАЛ ЗАХОПЛЕНО",
                ["p1.status.caught"] = "СИГНАЛ ЗБЕРЕЖЕНО",
                ["p1.detector"] = "Детектор: Активний",
                ["p1.object.none"] = "Об'єкт: відсутній",
                ["p1.object.unknown"] = "Об'єкт: Невідома Аномалія",
                ["p1.quality"] = "Якість сигналу: {0}%",
                ["p1.efficiency"] = "Швидкість: 0.000 Б\\с",
                ["p1.energy"] = "Споживання енергії: 100.00%",
                ["p1.btn.init"] = "ІНІЦІАЛІЗУВАТИ СКАНЕР",
                ["p1.btn.catch"] = "ЗЛОВИТИ СИГНАЛ",

                // Page2
                ["p2.title"] = "⚙ Обробка сигналу",
                ["p2.status.ready"] = "Готово до обробки",
                ["p2.status.processing"] = "Обробка...",
                ["p2.status.done"] = "Сигнал оброблено успішно!",
                ["p2.btn.start"] = "▶  Почати обробку",

                // Page3
                ["p3.title"] = "📊 Результат сигналу",

                // SettingsPage
                ["settings.title"] = "⚙ Налаштування",
                ["settings.theme"] = "Тема інтерфейсу",
                ["settings.dark"] = "🌙  Темна",
                ["settings.light"] = "☀  Світла",
                ["settings.theme.dark"] = "Поточна тема: 🌙 Темна",
                ["settings.theme.light"] = "Поточна тема: ☀ Світла",
                ["settings.lang"] = "Мова",
                ["settings.about"] = "Про програму",
                ["settings.appname"] = "Mine Radiotelescope app",
                ["settings.version"] = "Версія 1.1.1 BETA",

                // AuthPage
                ["auth.title"] = "Необхідна авторизація",
                ["auth.subtitle"] = "Зареєструйтесь або увійдіть, щоб надсилати сигнали",
                ["auth.tab.login"] = "Увійти",
                ["auth.tab.register"] = "Реєстрація",
                ["auth.username"] = "Ім'я користувача",
                ["auth.email"] = "Email",
                ["auth.password"] = "Пароль",
                ["auth.btn.login"] = "Увійти",
                ["auth.btn.register"] = "Зареєструватись",
                ["auth.err.empty"] = "Будь ласка, заповніть всі поля.",
                ["auth.err.exists"] = "Ім'я або email вже використовуються.",
                ["auth.err.invalid"] = "Невірне ім'я або пароль.",
                ["auth.ok.registered"] = "Реєстрацію завершено! Ви увійшли.",
                ["auth.ok.loggedin"] = "З поверненням, {0}!",

                // SendSignalPage
                ["send.title"] = "📨 Надіслати сигнал",
                ["send.user"] = "Авторизований: {0}",
                ["send.label.autoresponder"] = "Автовідповідач сигналів",
                ["send.autoresponder.text"] =
                    "📡 АВТОВІДПОВІДАЧ ОНЛАЙН\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "Система: MRtA Gateway v2.1\n" +
                    "Статус: ГОТОВИЙ ДО ПРИЙОМУ\n" +
                    "Режим: Автономна відповідь\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "Всі вхідні сигнали автоматично\n" +
                    "підтверджуються системою.\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "Очікування передачі...",
                ["send.btn.send"] = "📡 Надіслати останній сигнал",
                ["send.btn.sending"] = "Передача...",
                ["send.countdown"] = "Сигнал доставлено за {0}с...",
                ["send.thanks"] =
                    "✅ ПЕРЕДАЧУ ЗАВЕРШЕНО\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                    "Дякуємо за відправлений сигнал!\n" +
                    "Вашу передачу отримано та\n" +
                    "збережено в базі MRtA.\n\n" +
                    "ID сигналу: {0}\n" +
                    "Часова мітка: {1}\n" +
                    "Статус: ПІДТВЕРДЖЕНО\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━",
                ["send.logout"] = "Вийти",
            }
        };

        // Основний метод доступу
        public static string Get(string key)
        {
            if (_strings.TryGetValue(_currentLanguage, out var dict) &&
                dict.TryGetValue(key, out var value))
                return value;

            // fallback → англійська
            if (_strings["en"].TryGetValue(key, out var fallback))
                return fallback;

            return $"[{key}]"; // явна помилка, щоб легко помітити
        }

        // Зміна мови
        public static void SetLanguage(string lang)
        {
            if (!Array.Exists(AvailableLanguages, l => l == lang)) return;
            if (_currentLanguage == lang) return;
            _currentLanguage = lang;
            LanguageChanged?.Invoke();
        }

        //Хелпер: рядок з форматуванням
        public static string Format(string key, params object[] args)
            => string.Format(Get(key), args);
    }
}