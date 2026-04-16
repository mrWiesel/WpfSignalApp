using System.Collections.ObjectModel;

namespace WpfSignalApp.ViewModels
{
    /// <summary>
    /// ViewModel для Page4 (список сигналів).
    /// ListBox прив'язується до Signals — оновлюється автоматично через ObservableCollection.
    /// </summary>
    public class Page4ViewModel : BaseViewModel
    {
        /// <summary>
        /// Посилання на глобальний ObservableCollection з SignalStore.
        /// ListBox.ItemsSource="{Binding Signals}" — оновлення без code-behind.
        /// </summary>
        public ObservableCollection<SignalData> Signals => SignalStore.Signals;

        public Page4ViewModel()
        {
            EnsureTenSignals();
        }

        private void EnsureTenSignals()
        {
            while (Signals.Count < 10)
                Signals.Add(SignalStore.GenerateRandomSignal());

            while (Signals.Count > 10)
                Signals.RemoveAt(Signals.Count - 1);
        }
    }
}
