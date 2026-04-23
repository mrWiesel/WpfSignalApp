using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfSignalApp.Models;
using WpfSignalApp.Services;

namespace WpfSignalApp
{
    public partial class AuthPage : Page
    {
        private bool _isLoginMode = true;

        public AuthPage()
        {
            InitializeComponent();
            LocalizationManager.LanguageChanged += UpdateTexts;
            UpdateTexts();
            SetLoginMode(true);
        }

        private void UpdateTexts()
        {
            TbTitle.Text    = LocalizationManager.Get("auth.title");
            TbSubtitle.Text = LocalizationManager.Get("auth.subtitle");
            TbUsername.Text = LocalizationManager.Get("auth.username");
            TbEmail.Text    = LocalizationManager.Get("auth.email");
            TbPassword.Text = LocalizationManager.Get("auth.password");
            BtnTabLogin.Content    = LocalizationManager.Get("auth.tab.login");
            BtnTabRegister.Content = LocalizationManager.Get("auth.tab.register");
            BtnSubmit.Content = _isLoginMode
                ? LocalizationManager.Get("auth.btn.login")
                : LocalizationManager.Get("auth.btn.register");
        }

        private void SetLoginMode(bool loginMode)
        {
            _isLoginMode = loginMode;
            PanelEmail.Visibility = loginMode ? Visibility.Collapsed : Visibility.Visible;

            // Tab highlight
            BtnTabLogin.Background    = loginMode ? new SolidColorBrush(Color.FromRgb(0, 122, 204)) : (Brush)FindResource("ThemeBtnBg");
            BtnTabRegister.Background = loginMode ? (Brush)FindResource("ThemeBtnBg") : new SolidColorBrush(Color.FromRgb(0, 122, 204));

            TbStatus.Visibility = Visibility.Collapsed;
            BtnSubmit.Content = loginMode
                ? LocalizationManager.Get("auth.btn.login")
                : LocalizationManager.Get("auth.btn.register");
        }

        private void BtnTabLogin_Click(object sender, RoutedEventArgs e)    => SetLoginMode(true);
        private void BtnTabRegister_Click(object sender, RoutedEventArgs e) => SetLoginMode(false);

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            string email    = TxtEmail.Text.Trim();
            string password = TxtPassword.Password;

            if (_isLoginMode)
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ShowStatus(LocalizationManager.Get("auth.err.empty"), isError: true);
                    return;
                }

                User? user = UserService.Login(username, password);
                if (user == null)
                {
                    ShowStatus(LocalizationManager.Get("auth.err.invalid"), isError: true);
                    return;
                }

                SessionManager.Login(user);
                ShowStatus(LocalizationManager.Format("auth.ok.loggedin", user.Username), isError: false);
                NavigateToSendSignal();
            }
            else
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ShowStatus(LocalizationManager.Get("auth.err.empty"), isError: true);
                    return;
                }

                bool ok = UserService.Register(username, email, password);
                if (!ok)
                {
                    ShowStatus(LocalizationManager.Get("auth.err.exists"), isError: true);
                    return;
                }

                // Auto-login after register
                User? newUser = UserService.Login(username, password);
                if (newUser != null) SessionManager.Login(newUser);

                ShowStatus(LocalizationManager.Get("auth.ok.registered"), isError: false);
                NavigateToSendSignal();
            }
        }

        private void ShowStatus(string msg, bool isError)
        {
            TbStatus.Text = msg;
            TbStatus.Foreground = isError
                ? new SolidColorBrush(Colors.OrangeRed)
                : new SolidColorBrush(Colors.LimeGreen);
            TbStatus.Visibility = Visibility.Visible;
        }

        private void NavigateToSendSignal()
        {
            // Small delay so user sees the success message, then navigate
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = System.TimeSpan.FromMilliseconds(800)
            };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                NavigationService?.Navigate(new SendSignalPage());
            };
            timer.Start();
        }
    }
}
