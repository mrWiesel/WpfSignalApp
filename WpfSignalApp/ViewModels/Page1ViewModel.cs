using System;
using System.Windows.Media;

namespace WpfSignalApp.ViewModels
{
    public class Page1ViewModel : BaseViewModel
    {
        // Константи
        private const int Threshold = 10;
        private const int MaxPol    = 360;
        private const int MaxFreq   = 1000;

        //Приватні поля
        private int  _targetPolarity;
        private int  _targetFrequency;
        private int  _currentPol;
        private int  _currentFreq;
        private bool _initialized;

        private SignalState _state = SignalState.NoSignal;
        private enum SignalState { NoSignal, Scanning, Lock, Caught }

        // Computed лейбли (оновлюються через OnPropertyChanged)
        public string PolLabelText   => LocalizationManager.Get("p1.polarity.label");
        public string PolTitleText   => LocalizationManager.Get("p1.polarity.title");
        public string FreqLabelText  => LocalizationManager.Get("p1.frequency.label");
        public string FreqTitleText  => LocalizationManager.Get("p1.frequency.title");
        public string DetectorText   => LocalizationManager.Get("p1.detector");
        public string EfficiencyText => LocalizationManager.Get("p1.efficiency");
        public string EnergyText     => LocalizationManager.Get("p1.energy");
        public string BtnInitText    => LocalizationManager.Get("p1.btn.init");
        public string BtnCatchText   => LocalizationManager.Get("p1.btn.catch");

        // Поля зі сповіщенням
        private string _polTargetText  = "";
        private string _polCurrentText = "";
        private string _freqTargetText  = "";
        private string _freqCurrentText = "";
        private string _signalStatus   = "";
        private string _objectText     = "";
        private string _qualityText    = "";
        private Brush  _polCurrentColor;
        private Brush  _freqCurrentColor;
        private Brush  _signalStatusColor = Brushes.Red;
        private bool   _catchEnabled    = false;

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

        // Конструктор
        public Page1ViewModel()
        {
            _polCurrentColor  = DefaultTextBrush();
            _freqCurrentColor = DefaultTextBrush();

            ThemeManager.ThemeChanged           += OnThemeChanged;
            LocalizationManager.LanguageChanged += OnLanguageChanged;

            RefreshAllTexts();
        }

        //Команди
        public void Initialize()
        {
            var rng = new Random();
            _targetPolarity  = rng.Next(0, MaxPol  + 1);
            _targetFrequency = rng.Next(0, MaxFreq + 1);
            _currentPol      = 0;
            _currentFreq     = 0;
            _initialized     = true;
            _state           = SignalState.Scanning;

            CatchEnabled = false;
            ObjectText   = LocalizationManager.Get("p1.object.unknown");

            RefreshAllTexts();
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

            _state            = SignalState.Caught;
            SignalStatus      = LocalizationManager.Get("p1.status.caught");
            SignalStatusColor = Brushes.LimeGreen;
            QualityText       = LocalizationManager.Format("p1.quality", 100);
            CatchEnabled      = false;
            _initialized      = false;
        }

        // Приватна логіка
        private static Brush DefaultTextBrush()
            => ThemeManager.IsDark ? Brushes.White : Brushes.Black;

        private void OnThemeChanged()
        {
            bool polClose  = _initialized && Math.Abs(_targetPolarity  - _currentPol)  <= Threshold;
            bool freqClose = _initialized && Math.Abs(_targetFrequency - _currentFreq) <= Threshold;

            PolCurrentColor  = polClose  ? Brushes.LimeGreen : DefaultTextBrush();
            FreqCurrentColor = freqClose ? Brushes.LimeGreen : DefaultTextBrush();
        }

        private void OnLanguageChanged()
        {
            // Computed лейбли — просто сповіщаємо UI
            OnPropertyChanged(nameof(PolLabelText));
            OnPropertyChanged(nameof(PolTitleText));
            OnPropertyChanged(nameof(FreqLabelText));
            OnPropertyChanged(nameof(FreqTitleText));
            OnPropertyChanged(nameof(DetectorText));
            OnPropertyChanged(nameof(EfficiencyText));
            OnPropertyChanged(nameof(EnergyText));
            OnPropertyChanged(nameof(BtnInitText));
            OnPropertyChanged(nameof(BtnCatchText));

            // Динамічні рядки — перебудовуємо
            RefreshAllTexts();
        }

        private void RefreshAllTexts()
        {
            // Target рядки
            PolTargetText  = _initialized
                ? BuildTargetPol(_targetPolarity)
                : LocalizationManager.Get("p1.polarity.target");

            FreqTargetText = _initialized
                ? BuildTargetFreq(_targetFrequency)
                : LocalizationManager.Get("p1.freq.target");

            // Current рядки
            PolCurrentText  = _initialized
                ? BuildCurrentPol(_currentPol)
                : LocalizationManager.Get("p1.polarity.current");

            FreqCurrentText = _initialized
                ? BuildCurrentFreq(_currentFreq)
                : LocalizationManager.Get("p1.freq.current");

            // Якість
            if (_initialized)
                UpdateQuality();
            else if (_state != SignalState.Caught)
                QualityText = LocalizationManager.Format("p1.quality", 0);

            // Об'єкт (тільки в початковому стані)
            if (!_initialized && _state == SignalState.NoSignal)
                ObjectText = LocalizationManager.Get("p1.object.none");

            // Статус і колір
            SignalStatus = _state switch
            {
                SignalState.NoSignal => LocalizationManager.Get("p1.status.nosignal"),
                SignalState.Scanning => LocalizationManager.Get("p1.status.scanning"),
                SignalState.Lock     => LocalizationManager.Get("p1.status.lock"),
                SignalState.Caught   => LocalizationManager.Get("p1.status.caught"),
                _                   => ""
            };

            SignalStatusColor = _state switch
            {
                SignalState.NoSignal => Brushes.Red,
                SignalState.Scanning => Brushes.Yellow,
                SignalState.Lock     => Brushes.Green,
                SignalState.Caught   => Brushes.LimeGreen,
                _                   => Brushes.Gray
            };
        }

        private void UpdateDisplays()
        {
            bool polClose  = Math.Abs(_targetPolarity  - _currentPol)  <= Threshold;
            bool freqClose = Math.Abs(_targetFrequency - _currentFreq) <= Threshold;

            PolCurrentText  = BuildCurrentPol(_currentPol);
            FreqCurrentText = BuildCurrentFreq(_currentFreq);

            PolCurrentColor  = polClose  ? Brushes.LimeGreen : DefaultTextBrush();
            FreqCurrentColor = freqClose ? Brushes.LimeGreen : DefaultTextBrush();

            UpdateQuality();

            if (polClose && freqClose)
            {
                _state            = SignalState.Lock;
                CatchEnabled      = true;
                SignalStatus      = LocalizationManager.Get("p1.status.lock");
                SignalStatusColor = Brushes.Green;
            }
            else if (_initialized)
            {
                _state            = SignalState.Scanning;
                CatchEnabled      = false;
                SignalStatus      = LocalizationManager.Get("p1.status.scanning");
                SignalStatusColor = Brushes.Yellow;
            }
        }

        private void UpdateQuality()
        {
            int diffPol  = Math.Abs(_targetPolarity  - _currentPol);
            int diffFreq = Math.Abs(_targetFrequency - _currentFreq);
            int quality  = Math.Max(0, 100 - (diffPol + diffFreq) / 2);
            QualityText  = LocalizationManager.Format("p1.quality", quality);
        }

        // Будівники локалізованих рядків
        private static string BuildTargetPol(int val)
            => LocalizationManager.Get("p1.polarity.target").Replace("---", val.ToString());

        private static string BuildTargetFreq(int val)
            => LocalizationManager.Get("p1.freq.target").Replace("---", val.ToString());

        private static string BuildCurrentPol(int val)
            => ReplaceNumber(LocalizationManager.Get("p1.polarity.current"), val);

        private static string BuildCurrentFreq(int val)
            => ReplaceNumber(LocalizationManager.Get("p1.freq.current"), val);

        /// <summary>
        /// Замінює перше число у локалізованому шаблоні ("Current: 0 deg" → "Current: 42 deg").
        /// </summary>
        private static string ReplaceNumber(string template, int newVal)
        {
            int start = -1, end = -1;
            for (int i = 0; i < template.Length; i++)
            {
                if (char.IsDigit(template[i]))
                {
                    if (start == -1) start = i;
                    end = i;
                }
                else if (start != -1) break;
            }

            if (start == -1) return template + " " + newVal;
            return template[..start] + newVal + template[(end + 1)..];
        }

        private static int Wrap(int value, int min, int max)
        {
            int range = max - min + 1;
            return ((value - min) % range + range) % range + min;
        }
    }
}
