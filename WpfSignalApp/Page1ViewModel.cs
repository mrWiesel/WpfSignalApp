using System;
using System.Windows.Media;

namespace WpfSignalApp.ViewModels
{
    /// <summary>
    /// ViewModel для Page1 (сканер сигналу).
    /// Підтримує локалізацію через LocalizationManager.
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

        // Внутрішній стан статусу (не рядок, а enum-like)
        private SignalState _state = SignalState.NoSignal;

        private enum SignalState { NoSignal, Scanning, Lock, Caught }

        // ── Публічні властивості (Binding у XAML) ───────────────────────────────

        public string PolLabelText    => L["p1.polarity.label"];
        public string PolTitleText    => L["p1.polarity.title"];
        public string FreqLabelText   => L["p1.frequency.label"];
        public string FreqTitleText   => L["p1.frequency.title"];
        public string DetectorText    => L["p1.detector"];
        public string EfficiencyText  => L["p1.efficiency"];
        public string EnergyText      => L["p1.energy"];
        public string BtnInitText     => L["p1.btn.init"];
        public string BtnCatchText    => L["p1.btn.catch"];

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

        // ── Хелпер ───────────────────────────────────────────────────────────────
        private static LocalizationManager L => null!; // trick — використовуємо індексатор статичного класу

        // ── Конструктор ──────────────────────────────────────────────────────────

        public Page1ViewModel()
        {
            _polCurrentColor  = DefaultTextBrush();
            _freqCurrentColor = DefaultTextBrush();

            ThemeManager.ThemeChanged          += OnThemeChanged;
            LocalizationManager.LanguageChanged += OnLanguageChanged;

            RefreshAllTexts();
        }

        // ── Команди ──────────────────────────────────────────────────────────────

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
            ObjectText   = LocalizationManager["p1.object.unknown"];

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
            SignalStatus      = LocalizationManager["p1.status.caught"];
            SignalStatusColor = Brushes.LimeGreen;
            QualityText       = LocalizationManager.Format("p1.quality", 100);
            CatchEnabled      = false;
            _initialized      = false;
        }

        // ── Приватна логіка ──────────────────────────────────────────────────────

        private static Brush DefaultTextBrush()
            => ThemeManager.IsDark ? Brushes.White : Brushes.Black;

        private void OnThemeChanged()
        {
            bool polClose  = _initialized && Math.Abs(_targetPolarity  - _currentPol)  <= Threshold;
            bool freqClose = _initialized && Math.Abs(_targetFrequency - _currentFreq) <= Threshold;

            PolCurrentColor  = polClose  ? Brushes.LimeGreen : DefaultTextBrush();
            FreqCurrentColor = freqClose ? Brushes.LimeGreen : DefaultTextBrush();
        }

        /// <summary>
        /// Викликається при зміні мови — перебудовує всі локалізовані рядки.
        /// </summary>
        private void OnLanguageChanged()
        {
            // Сповістити UI про зміну computed-рядків (назви кнопок, лейбли)
            OnPropertyChanged(nameof(PolLabelText));
            OnPropertyChanged(nameof(PolTitleText));
            OnPropertyChanged(nameof(FreqLabelText));
            OnPropertyChanged(nameof(FreqTitleText));
            OnPropertyChanged(nameof(DetectorText));
            OnPropertyChanged(nameof(EfficiencyText));
            OnPropertyChanged(nameof(EnergyText));
            OnPropertyChanged(nameof(BtnInitText));
            OnPropertyChanged(nameof(BtnCatchText));

            RefreshAllTexts();
        }

        /// <summary>
        /// Оновлює всі динамічні рядки згідно з поточним станом і мовою.
        /// </summary>
        private void RefreshAllTexts()
        {
            // Target рядки
            if (_initialized)
            {
                PolTargetText  = BuildTargetPol(_targetPolarity);
                FreqTargetText = BuildTargetFreq(_targetFrequency);
            }
            else
            {
                PolTargetText  = LocalizationManager["p1.polarity.target"];
                FreqTargetText = LocalizationManager["p1.freq.target"];
            }

            // Current рядки
            PolCurrentText  = _initialized
                ? BuildCurrentPol(_currentPol)
                : LocalizationManager["p1.polarity.current"];

            FreqCurrentText = _initialized
                ? BuildCurrentFreq(_currentFreq)
                : LocalizationManager["p1.freq.current"];

            // Якість
            if (!_initialized && _state != SignalState.Caught)
                QualityText = LocalizationManager.Format("p1.quality", 0);
            else if (_initialized)
                UpdateQuality();

            // Об'єкт
            if (!_initialized && _state == SignalState.NoSignal)
                ObjectText = LocalizationManager["p1.object.none"];

            // Статус
            SignalStatus = _state switch
            {
                SignalState.NoSignal => LocalizationManager["p1.status.nosignal"],
                SignalState.Scanning => LocalizationManager["p1.status.scanning"],
                SignalState.Lock     => LocalizationManager["p1.status.lock"],
                SignalState.Caught   => LocalizationManager["p1.status.caught"],
                _                   => ""
            };

            SignalStatusColor = _state switch
            {
                SignalState.NoSignal => Brushes.Red,
                SignalState.Scanning => Brushes.Yellow,
                SignalState.Lock     => Brushes.Cyan,
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
                SignalStatus      = LocalizationManager["p1.status.lock"];
                SignalStatusColor = Brushes.Cyan;
            }
            else if (_initialized)
            {
                _state            = SignalState.Scanning;
                CatchEnabled      = false;
                SignalStatus      = LocalizationManager["p1.status.scanning"];
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

        // ── Локалізовані рядки значень ───────────────────────────────────────────

        // Будуємо рядки вручну, щоб одиниці виміру теж локалізувались.
        // В англійській: "Target: 180 deg" / "Current: 90 deg"
        // В українській: "Ціль:   180 град" / "Поточна: 90 град"
        private static string BuildTargetPol(int val)
        {
            string template = LocalizationManager["p1.polarity.target"]; // "Target:  --- deg" або "Ціль:    --- grad"
            // замінюємо "---" на число
            return template.Replace("---", val.ToString());
        }

        private static string BuildTargetFreq(int val)
        {
            string template = LocalizationManager["p1.freq.target"];
            return template.Replace("---", val.ToString());
        }

        private static string BuildCurrentPol(int val)
        {
            // "Current: 0 deg" → "Current: {val} deg"
            string template = LocalizationManager["p1.polarity.current"];
            return ReplaceNumber(template, val);
        }

        private static string BuildCurrentFreq(int val)
        {
            string template = LocalizationManager["p1.freq.current"];
            return ReplaceNumber(template, val);
        }

        /// <summary>
        /// Замінює перше число (або 0) у рядку на нове значення.
        /// Це дозволяє зберегти локалізований формат рядка.
        /// </summary>
        private static string ReplaceNumber(string template, int newVal)
        {
            // Шаблон: "Current: 0 deg" або "Поточна: 0 МГц"
            // Знаходимо перше число і замінюємо його
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
