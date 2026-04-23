using System;
using System.Collections.ObjectModel;

namespace WpfSignalApp
{
    /// <summary>
    /// Глобальне сховище сигналів.
    /// ObservableCollection автоматично нотифікує ListBox при додаванні/видаленні елементів.
    /// </summary>
    public static class SignalStore
    {
        /// <summary>
        /// ObservableCollection замість List — ListBox оновлюється автоматично через Binding.
        /// </summary>
        public static ObservableCollection<SignalData> Signals { get; } = new ObservableCollection<SignalData>();

        private static readonly Random _rnd = new Random();

        public static SignalData GenerateRandomSignal() => new SignalData
        {
            Polarity  = _rnd.Next(1, 101),
            Frequency = _rnd.Next(1, 101)
        };
    }
}
