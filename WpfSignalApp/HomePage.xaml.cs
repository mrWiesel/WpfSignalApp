using System.Windows.Controls;

namespace WpfSignalApp
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            LocalizationManager.LanguageChanged += UpdateTexts;
            Unloaded += (_, _) => LocalizationManager.LanguageChanged -= UpdateTexts;
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            TbTitle.Text    = LocalizationManager["home.title"];
            TbSubtitle.Text = LocalizationManager["home.subtitle"];
        }
    }
}
