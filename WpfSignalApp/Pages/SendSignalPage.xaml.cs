using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfSignalApp.Services;

namespace WpfSignalApp
{
    public partial class SendSignalPage : Page
    {
        private DispatcherTimer? _timer;
        private int _secondsLeft;
        private bool _isSending = false;

        public SendSignalPage()
        {
            InitializeComponent();
            LocalizationManager.LanguageChanged += UpdateTexts;
            UpdateTexts();
            SetIdleState();
        }

        private void UpdateTexts()
        {
            TbTitle.Text = LocalizationManager.Get("send.title");
            TbUser.Text  = LocalizationManager.Format("send.user",
                SessionManager.CurrentUser?.Username ?? "");
            TbAutoresponderLabel.Text = LocalizationManager.Get("send.label.autoresponder");
            TxtAutoresponder.Text     = LocalizationManager.Get("send.autoresponder.text");
            BtnLogout.Content         = LocalizationManager.Get("send.logout");
            BtnSend.Content           = _isSending
                ? LocalizationManager.Get("send.btn.sending")
                : LocalizationManager.Get("send.btn.send");

            string hint = LocalizationManager.Get("send.btn.send");
            TbIdleHint.Text  = hint;
            TbIdleHint2.Text = hint;

            // Re-render countdown text if currently counting
            if (_isSending && _timer != null)
                TbCountdown.Text = LocalizationManager.Format("send.countdown", _secondsLeft);
        }

        private void SetIdleState()
        {
            PanelIdle.Visibility      = Visibility.Visible;
            PanelCountdown.Visibility = Visibility.Collapsed;
            TxtThanks.Visibility      = Visibility.Collapsed;
            BtnSend.IsEnabled         = true;
            BtnSend.Content           = LocalizationManager.Get("send.btn.send");
            BtnSend.Background        = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(0, 122, 204));
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (_isSending) return;

            _isSending    = true;
            _secondsLeft  = 5;

            BtnSend.Content   = LocalizationManager.Get("send.btn.sending");
            BtnSend.IsEnabled = false;
            BtnSend.Background = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(80, 80, 80));

            // Show countdown panel, hide others
            PanelIdle.Visibility      = Visibility.Collapsed;
            TxtThanks.Visibility      = Visibility.Collapsed;
            PanelCountdown.Visibility = Visibility.Visible;

            PbCountdown.Value = 0;
            TbCountdown.Text  = LocalizationManager.Format("send.countdown", _secondsLeft);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _secondsLeft--;
            PbCountdown.Value = 5 - _secondsLeft;
            TbCountdown.Text  = LocalizationManager.Format("send.countdown", _secondsLeft);

            if (_secondsLeft <= 0)
            {
                _timer!.Stop();
                _timer = null;
                ShowThanks();
            }
        }

        private void ShowThanks()
        {
            _isSending = false;

            string signalId   = Guid.NewGuid().ToString()[..8].ToUpper();
            string timestamp  = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            TxtThanks.Text = LocalizationManager.Format("send.thanks", signalId, timestamp);

            PanelCountdown.Visibility = Visibility.Collapsed;
            PanelIdle.Visibility      = Visibility.Collapsed;
            TxtThanks.Visibility      = Visibility.Visible;

            // Re-enable button so user can send again
            BtnSend.Content   = LocalizationManager.Get("send.btn.send");
            BtnSend.IsEnabled = true;
            BtnSend.Background = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(0, 122, 204));
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            SessionManager.Logout();
            NavigationService?.Navigate(new AuthPage());
        }
    }
}
