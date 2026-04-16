using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfSignalApp
{
    public partial class Page1 : Page
    {
        private int _targetPolarity  = 0;
        private int _targetFrequency = 0;
        private int _currentPol      = 0;
        private int _currentFreq     = 0;
        private bool _initialized    = false;

        private const int Threshold = 10;   // ±10 → highlight green
        private const int MaxPol    = 360;
        private const int MaxFreq   = 1000;

        public Page1()
        {
            InitializeComponent();
        }

        //INITIALIZE
        private void BtnInit_Click(object sender, RoutedEventArgs e)
        {
            var rng = new Random();
            _targetPolarity  = rng.Next(0, MaxPol  + 1);
            _targetFrequency = rng.Next(0, MaxFreq + 1);

            _currentPol  = 0;
            _currentFreq = 0;
            _initialized = true;

            TxtPolTarget.Text  = $"Target:  {_targetPolarity} deg";
            TxtFreqTarget.Text = $"Target:  {_targetFrequency} MHz";

            UpdateDisplays();

            TxtSignalStatus.Text       = "SCANNING...";
            TxtSignalStatus.Foreground = Brushes.Yellow;
            TxtObject.Text             = "Object: Unknown Anomaly";
            BtnCatch.IsEnabled         = false;
        }

        //POLARITY
        private void BtnPolPlus100_Click(object sender, RoutedEventArgs e)  => AdjustPol(+100);
        private void BtnPolPlus10_Click(object sender,  RoutedEventArgs e)  => AdjustPol(+10);
        private void BtnPolPlus1_Click(object sender,   RoutedEventArgs e)  => AdjustPol(+1);
        private void BtnPolMinus1_Click(object sender,  RoutedEventArgs e)  => AdjustPol(-1);
        private void BtnPolMinus10_Click(object sender, RoutedEventArgs e)  => AdjustPol(-10);
        private void BtnPolMinus100_Click(object sender,RoutedEventArgs e)  => AdjustPol(-100);

        private void AdjustPol(int delta)
        {
            if (!_initialized) return;
            _currentPol = Wrap(_currentPol + delta, 0, MaxPol);
            UpdateDisplays();
        }

        //FREQUENCY
        private void BtnFreqPlus100_Click(object sender, RoutedEventArgs e)  => AdjustFreq(+100);
        private void BtnFreqPlus10_Click(object sender,  RoutedEventArgs e)  => AdjustFreq(+10);
        private void BtnFreqPlus1_Click(object sender,   RoutedEventArgs e)  => AdjustFreq(+1);
        private void BtnFreqMinus1_Click(object sender,  RoutedEventArgs e)  => AdjustFreq(-1);
        private void BtnFreqMinus10_Click(object sender, RoutedEventArgs e)  => AdjustFreq(-10);
        private void BtnFreqMinus100_Click(object sender,RoutedEventArgs e)  => AdjustFreq(-100);

        private void AdjustFreq(int delta)
        {
            if (!_initialized) return;
            _currentFreq = Wrap(_currentFreq + delta, 0, MaxFreq);
            UpdateDisplays();
        }

        //WRAP helper
        private static int Wrap(int value, int min, int max)
        {
            int range = max - min + 1;
            value = ((value - min) % range + range) % range + min;
            return value;
        }

        //UPDATE DISPLAYS
        private void UpdateDisplays()
        {
            bool polClose  = Math.Abs(_targetPolarity  - _currentPol)  <= Threshold;
            bool freqClose = Math.Abs(_targetFrequency - _currentFreq) <= Threshold;

            TxtPolCurrent.Text       = $"Current: {_currentPol} deg";
            TxtPolCurrent.Foreground = polClose ? Brushes.LimeGreen : Brushes.White;

            TxtFreqCurrent.Text       = $"Current: {_currentFreq} MHz";
            TxtFreqCurrent.Foreground = freqClose ? Brushes.LimeGreen : Brushes.White;

            int diffPol  = Math.Abs(_targetPolarity  - _currentPol);
            int diffFreq = Math.Abs(_targetFrequency - _currentFreq);
            int quality  = Math.Max(0, 100 - (diffPol + diffFreq) / 2);
            TxtQuality.Text = $"Signal quality: {quality}%";

            if (polClose && freqClose)
            {
                BtnCatch.IsEnabled             = true;
                TxtSignalStatus.Text           = "LOCK ACQUIRED";
                TxtSignalStatus.Foreground     = Brushes.Cyan;
            }
            else if (_initialized)
            {
                BtnCatch.IsEnabled             = false;
                TxtSignalStatus.Text           = "SCANNING...";
                TxtSignalStatus.Foreground     = Brushes.Yellow;
            }
        }

        // ── CATCH ────────────────────────────────────────────────────────────────
        private void BtnCatch_Click(object sender, RoutedEventArgs e)
        {
            SignalStore.Signals.Add(new SignalData
            {
                Polarity  = _currentPol,
                Frequency = _currentFreq
            });

            TxtSignalStatus.Text       = "SIGNAL CAUGHT";
            TxtSignalStatus.Foreground = Brushes.LimeGreen;
            TxtQuality.Text            = "Signal quality: 100%";

            BtnCatch.IsEnabled = false;
            _initialized       = false;
        }
    }
}
