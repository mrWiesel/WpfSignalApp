using System.Windows;
using System.Windows.Controls;
using WpfSignalApp.ViewModels;

namespace WpfSignalApp
{
    /// <summary>
    /// Code-behind для Page2 — лише DataContext і виклик async методу.
    /// </summary>
    public partial class Page2 : Page
    {
        private Page2ViewModel _vm = new Page2ViewModel();

        public Page2()
        {
            InitializeComponent();
            DataContext = _vm;  // ✅ DataContext встановлено
        }

        private async void BtnProcess_Click(object sender, RoutedEventArgs e)
            => await _vm.ProcessAsync();
    }
}
