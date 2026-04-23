using System.Threading.Tasks;

namespace WpfSignalApp.ViewModels
{
    /// <summary>
    /// ViewModel для Page2 (обробка сигналу).
    /// Підтримує локалізацію через LocalizationManager.
    /// </summary>
    public class Page2ViewModel : BaseViewModel
    {
        private double _progress;
        private bool _isProcessing;
        private string _statusText = "";

        // Локалізовані computed-властивості
        public string PageTitle => LocalizationManager.Get("p2.title");
        public string BtnStartText => LocalizationManager.Get("p2.btn.start");

        public double Progress
        {
            get => _progress;
            set => SetField(ref _progress, value);
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                SetField(ref _isProcessing, value);
                OnPropertyChanged(nameof(CanProcess));
            }
        }

        public bool CanProcess => !_isProcessing;

        public string StatusText
        {
            get => _statusText;
            set => SetField(ref _statusText, value);
        }

        public Page2ViewModel()
        {
            LocalizationManager.LanguageChanged += OnLanguageChanged;
            StatusText = LocalizationManager.Get("p2.status.ready");
        }

        private void OnLanguageChanged()
        {
            OnPropertyChanged(nameof(PageTitle));
            OnPropertyChanged(nameof(BtnStartText));

            // Оновити динамічний статус відповідно до поточного стану
            if (!_isProcessing && _progress == 0)
                StatusText = LocalizationManager.Get("p2.status.ready");
            else if (_isProcessing)
                StatusText = LocalizationManager.Get("p2.status.processing");
            else
                StatusText = LocalizationManager.Get("p2.status.done");
        }

        public async Task ProcessAsync()
        {
            IsProcessing = true;
            Progress = 0;
            StatusText = LocalizationManager.Get("p2.status.processing");

            for (int i = 1; i <= 100; i++)
            {
                Progress = i;
                await Task.Delay(30);
            }

            StatusText = LocalizationManager.Get("p2.status.done");
            IsProcessing = false;
        }
    }
}