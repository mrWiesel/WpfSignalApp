using System;
using System.Collections.Generic;

namespace WpfSignalApp
{
    /// <summary>
    /// Модель одного сигналу.
    /// </summary>
    public class SignalData
    {
        public int Polarity  { get; set; }
        public int Frequency { get; set; }

        public override string ToString() =>
            $"Polarity: {Polarity}.00 deg | Frequency: {Frequency}.00 MHz";
    }

    /// <summary>
    /// Глобальне сховище сигналів — доступне з усіх сторінок.
    /// </summary>
    public static class SignalStore
    {
        public static List<SignalData> Signals { get; } = new List<SignalData>();

        private static readonly Random _rnd = new Random();

        public static SignalData GenerateRandomSignal() => new SignalData
        {
            Polarity  = _rnd.Next(1, 101),
            Frequency = _rnd.Next(1, 101)
        };
    }
}
