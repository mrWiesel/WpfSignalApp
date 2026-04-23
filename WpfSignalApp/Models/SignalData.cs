using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfSignalApp
{
    /// <summary>
    /// Модель одного сигналу з INotifyPropertyChanged —
    /// UI автоматично реагує на зміну Polarity / Frequency.
    /// </summary>
    public class SignalData : INotifyPropertyChanged
    {
        private int _polarity;
        private int _frequency;

        public int Polarity
        {
            get => _polarity;
            set { if (_polarity != value) { _polarity = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); } }
        }

        public int Frequency
        {
            get => _frequency;
            set { if (_frequency != value) { _frequency = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); } }
        }

        /// <summary>Текст для відображення у ListBox через Binding.</summary>
        public string DisplayText => $"Polarity: {Polarity}.00 deg  |  Frequency: {Frequency}.00 MHz";

        public override string ToString() => DisplayText;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
