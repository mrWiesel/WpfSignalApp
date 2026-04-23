using System.Windows.Controls;
using WpfSignalApp.ViewModels;

namespace WpfSignalApp
{
    /// <summary>
    /// Code-behind для Page4 — лише DataContext.
    /// ItemsSource ListBox-у встановлюється через Binding у XAML.
    /// </summary>
    public partial class Page4 : Page
    {
        public Page4()
        {
            InitializeComponent();
            DataContext = new Page4ViewModel();  // ✅ DataContext встановлено
        }
    }
}
