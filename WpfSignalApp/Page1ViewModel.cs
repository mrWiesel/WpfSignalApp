using System;
using System.Windows.Media;

namespace WpfSignalApp.ViewModels
{
    /// <summary>
    /// ViewModel для Page1 (сканер сигналу).
    /// Вся логіка тут — code-behind містить лише InitializeComponent() і DataContext = new Page1ViewModel().
    /// </summary>
    public class Page1ViewModel : BaseViewModel
    {
        // ── Константи ───────────────────────────────────────────────────────────
        private const int Threshold = 10;
        private const int MaxPol    = 360;
        private const int MaxFreq   = 1000;

        // ── Приватні поля ────────────────────────────────────────────────────────
        private int  _targetPolarity;
        private int  _targetFrequency;
        private int  _currentPol;
        private int  _currentFreq;
        private bool _initialized;

        private string _polTargetText  = "Target:  --- deg";
        private string _polCurrentText = "Current: 0 deg";
        private string _freqTargetText  = "Target:  --- MHz";
        private string _freqCurrentText = "Current: 0 MHz";
        private string _signalStatus   = "NO SIGNAL";
        private string _objectText     = "Object: none";
        private string _qualityText    = "Signal quality: 0%";
        private Brush  _polCurrentColor  = Brushes.White;
        private Brush  _freqCurrentColor = Brushes.White;
        private Brush  _signalStatusColor = Brushes.Red;
        private bool   _catchEnabled    = false;

        // ── Публічні властивості (Binding у XAML) ───────────────────────────────

        public string PolTargetText
        {
            get => _polTargetText;
            set => SetField(ref _polTargetText, value);
        }

        public string PolCurrentText
        {
            get => _polCurrentText;
            set => SetField(ref _polCurrentText, value);
        }

        public string FreqTargetText
        {
            get => _freqTargetText;
            set => SetField(ref _freqTargetText, value);
        }

        public string FreqCurrentText
        {
            get => _freqCurrentText;
            set => SetField(ref _freqCurrentText, value);
        }

        public string SignalStatus
        {
            get => _signalStatus;
            set => SetField(ref _signalStatus, value);
        }

        public string ObjectText
        {
            get => _objectText;
            set => SetField(ref _objectText, value);
        }

        public string QualityText
        {
            get => _qualityText;
            set => SetField(ref _qualityText, value);
        }

        public Brush PolCurrentColor
        {
            get => _polCurrentColor;
            set => SetField(ref _polCurrentColor, value);
        }

        public Brush FreqCurrentColor
        {
            get => _freqCurrentColor;
            set => SetField(ref _freqCurrentColor, value);
        }

        public Brush SignalStatusColor
        {
            get => _signalStatusColor;
            set => SetField(ref _signalStatusColor, value);
        }

        public bool CatchEnabled
        {
            get => _catchEnabled;
            set => SetField(ref _catchEnabled, value);
        }

        // ── Команди (викликаються з code-behind через методи) ───────────────────

        public void Initialize()
        {
            var rng = new Random();
            _targetPolarity  = rng.Next(0, MaxPol  + 1);
            _targetFrequency = rng.Next(0, MaxFreq + 1);
            _currentPol      = 0;
            _currentFreq     = 0;
            _initialized     = true;

            PolTargetText  = $"Target:  {_targetPolarity} deg";
            FreqTargetText = $"Target:  {_targetFrequency} MHz";

            SignalStatus      = "SCANNING...";
            SignalStatusColor = Brushes.Yellow;
            ObjectText        = "Object: Unknown Anomaly";
            CatchEnabled      = false;

            UpdateDisplays();
        }

        public void AdjustPol(int delta)
        {
            if (!_initialized) return;
            _currentPol = Wrap(_currentPol + delta, 0, MaxPol);
            UpdateDisplays();
        }

        public void AdjustFreq(int delta)
        {
            if (!_initialized) return;
            _currentFreq = Wrap(_currentFreq + delta, 0, MaxFreq);
            UpdateDisplays();
        }

        public void CatchSignal()
        {
            SignalStore.Signals.Add(new SignalData
            {
                Polarity  = _currentPol,
                Frequency = _currentFreq
            });

            SignalStatus      = "SIGNAL CAUGHT";
            SignalStatusColor = Brushes.LimeGreen;
            QualityText       = "Signal quality: 100%";
            CatchEnabled      = false;
            _initialized      = false;
        }

        // ── Приватна логіка ──────────────────────────────────────────────────────

        private void UpdateDisplays()
        {
            bool polClose  = Math.Abs(_targetPolarity  - _currentPol)  <= Threshold;
            bool freqClose = Math.Abs(_targetFrequency - _currentFreq) <= Threshold;

            PolCurrentText  = $"Current: {_currentPol} deg";
            FreqCurrentText = $"Current: {_currentFreq} MHz";

            PolCurrentColor  = polClose  ? Brushes.LimeGreen : Brushes.White;
            FreqCurrentColor = freqClose ? Brushes.LimeGreen : Brushes.White;

            int diffPol  = Math.Abs(_targetPolarity  - _currentPol);
            int diffFreq = Math.Abs(_targetFrequency - _currentFreq);
            int quality  = Math.Max(0, 100 - (diffPol + diffFreq) / 2);
            QualityText = $"Signal quality: {quality}%";

            if (polClose && freqClose)
            {
                CatchEnabled      = true;
                SignalStatus      = "LOCK ACQUIRED";
                SignalStatusColor = Brushes.Cyan;
            }
            else if (_initialized)
            {
                CatchEnabled      = false;
                SignalStatus      = "SCANNING...";
                SignalStatusColor = Brushes.Yellow;
            }
        }

        private static int Wrap(int value, int min, int max)
        {
            int range = max - min + 1;
            return ((value - min) % range + range) % range + min;
        }
    }
}
