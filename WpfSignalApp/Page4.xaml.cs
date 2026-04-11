using System.Windows.Controls;

namespace WpfSignalApp
{
    public partial class Page4 : Page
    {
        public Page4()
        {
            InitializeComponent();
            LoadSignals();
        }

        private void LoadSignals()
        {
            // Доповнюємо до 10 якщо менше
            while (SignalStore.Signals.Count < 10)
                SignalStore.Signals.Add(SignalStore.GenerateRandomSignal());

            // Обрізаємо до 10 якщо більше
            while (SignalStore.Signals.Count > 10)
                SignalStore.Signals.RemoveAt(SignalStore.Signals.Count - 1);

            SignalList.ItemsSource = SignalStore.Signals;
        }
    }
}
