using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfSignalApp
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private async void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            BtnProcess.IsEnabled = false;
            ProgBar.Value = 0;

            for (int i = 1; i <= 100; i++)
            {
                ProgBar.Value = i;
                await Task.Delay(30);
            }

            MessageBox.Show("Signal processed successfully!");
            BtnProcess.IsEnabled = true;
        }
    }
}
