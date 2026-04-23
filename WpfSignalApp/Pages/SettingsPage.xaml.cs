using System.Windows;
using System.Windows.Controls;
using WpfSignalApp.ViewModels;

namespace WpfSignalApp
{
    /// <summary>
    /// Code-behind для SettingsPage — лише DataContext і виклики VM методів.
    /// </summary>
    public partial class SettingsPage : Page
    {
        private SettingsViewModel _vm = new SettingsViewModel();

        public SettingsPage()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        private void BtnDark_Click(object sender, RoutedEventArgs e)       => _vm.SetDark();
        private void BtnLight_Click(object sender, RoutedEventArgs e)      => _vm.SetLight();
        private void BtnEnglish_Click(object sender, RoutedEventArgs e)    => _vm.SetEnglish();
        private void BtnUkrainian_Click(object sender, RoutedEventArgs e)  => _vm.SetUkrainian();
    }
}
