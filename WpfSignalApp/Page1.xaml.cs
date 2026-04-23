using System.Windows;
using System.Windows.Controls;
using WpfSignalApp.ViewModels;

namespace WpfSignalApp
{
    /// <summary>
    /// Code-behind для Page1 — лише DataContext та тонкі обробники кнопок.
    /// Вся логіка знаходиться у Page1ViewModel.
    /// </summary>
    public partial class Page1 : Page
    {
        private Page1ViewModel _vm = new Page1ViewModel();

        public Page1()
        {
            InitializeComponent();
            DataContext = _vm;  // ✅ DataContext встановлено
        }

        // ── Ініціалізація ────────────────────────────────────────────────────────
        private void BtnInit_Click(object sender, RoutedEventArgs e)
            => _vm.Initialize();

        // ── Полярність ───────────────────────────────────────────────────────────
        private void BtnPolPlus100_Click(object sender, RoutedEventArgs e)  => _vm.AdjustPol(+100);
        private void BtnPolPlus10_Click(object sender,  RoutedEventArgs e)  => _vm.AdjustPol(+10);
        private void BtnPolPlus1_Click(object sender,   RoutedEventArgs e)  => _vm.AdjustPol(+1);
        private void BtnPolMinus1_Click(object sender,  RoutedEventArgs e)  => _vm.AdjustPol(-1);
        private void BtnPolMinus10_Click(object sender, RoutedEventArgs e)  => _vm.AdjustPol(-10);
        private void BtnPolMinus100_Click(object sender,RoutedEventArgs e)  => _vm.AdjustPol(-100);

        // ── Частота ──────────────────────────────────────────────────────────────
        private void BtnFreqPlus100_Click(object sender, RoutedEventArgs e)  => _vm.AdjustFreq(+100);
        private void BtnFreqPlus10_Click(object sender,  RoutedEventArgs e)  => _vm.AdjustFreq(+10);
        private void BtnFreqPlus1_Click(object sender,   RoutedEventArgs e)  => _vm.AdjustFreq(+1);
        private void BtnFreqMinus1_Click(object sender,  RoutedEventArgs e)  => _vm.AdjustFreq(-1);
        private void BtnFreqMinus10_Click(object sender, RoutedEventArgs e)  => _vm.AdjustFreq(-10);
        private void BtnFreqMinus100_Click(object sender,RoutedEventArgs e)  => _vm.AdjustFreq(-100);

        // ── Зловити сигнал ───────────────────────────────────────────────────────
        private void BtnCatch_Click(object sender, RoutedEventArgs e)
            => _vm.CatchSignal();
    }
}
