using System.Windows;
using System.Windows.Controls;
using WpfSignalApp.ViewModels;

namespace WpfSignalApp
{
    /// <summary>
    /// Code-behind для SettingsPage — лише DataContext і виклики VM методів.
    /// Жодного прямого доступу до контролів.
    /// </summary>
    public partial class SettingsPage : Page
    {
        private SettingsViewModel _vm = new SettingsViewModel();

        public SettingsPage()
        {
            InitializeComponent();
            DataContext = _vm;  // ✅ DataContext встановлено
        }

        private void BtnDark_Click(object sender, RoutedEventArgs e)  => _vm.SetDark();
        private void BtnLight_Click(object sender, RoutedEventArgs e) => _vm.SetLight();
    }
}
