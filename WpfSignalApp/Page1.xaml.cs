using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfSignalApp
{
    public partial class Page1 : Page
    {
        private SignalData _targetSignal;
        private bool _isSearching = false;

        public Page1()
        {
            InitializeComponent();
        }

        private void BtnCatch_Click(object sender, RoutedEventArgs e)
        {
            TxtSignalStatus.Text = "NO SIGNAL";
            TxtSignalStatus.Foreground = Brushes.Red;
            TxtQuality.Text = "Signal quality: 0%";
            TxtObject.Text = "Object: Unknown Anomaly";

            _targetSignal = SignalStore.GenerateRandomSignal();
            TxtPolTarget.Text  = $"Target offset: {_targetSignal.Polarity}.00 deg";
            TxtFreqTarget.Text = $"Target offset: {_targetSignal.Frequency}.00 MHz";

            BtnRepeat.IsEnabled = true;
        }

        private async void BtnRepeat_Click(object sender, RoutedEventArgs e)
        {
            if (_isSearching) return;
            _isSearching = true;
            BtnRepeat.IsEnabled = false;

            TxtSignalStatus.Text = "SCANNING...";
            TxtSignalStatus.Foreground = Brushes.Yellow;

            while (true)
            {
                var attempt = SignalStore.GenerateRandomSignal();
                TxtPolCurrent.Text  = $"Current offset: {attempt.Polarity}.00 deg";
                TxtFreqCurrent.Text = $"Current offset: {attempt.Frequency}.00 MHz";

                int diffPol  = Math.Abs(_targetSignal.Polarity  - attempt.Polarity);
                int diffFreq = Math.Abs(_targetSignal.Frequency - attempt.Frequency);
                int quality  = Math.Max(0, 100 - (diffPol + diffFreq));
                TxtQuality.Text = $"Signal quality: {quality}%";

                if (attempt.Polarity == _targetSignal.Polarity &&
                    attempt.Frequency == _targetSignal.Frequency)
                {
                    TxtSignalStatus.Text = "SIGNAL CAUGHT";
                    TxtSignalStatus.Foreground = Brushes.LimeGreen;
                    TxtQuality.Text = "Signal quality: 100%";

                    SignalStore.Signals.Add(attempt);
                    break;
                }

                await Task.Delay(15);
            }

            _isSearching = false;
            BtnRepeat.IsEnabled = true;
        }
    }
}
