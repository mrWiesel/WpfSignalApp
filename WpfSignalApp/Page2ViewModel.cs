using System.Threading.Tasks;

namespace WpfSignalApp.ViewModels
{
    /// <summary>
    /// ViewModel для Page2 (обробка сигналу).
    /// </summary>
    public class Page2ViewModel : BaseViewModel
    {
        private double _progress;
        private bool   _isProcessing;
        private string _statusText = "Готово до обробки";

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
                // IsEnabled кнопки — інверсія IsProcessing
                OnPropertyChanged(nameof(CanProcess));
            }
        }

        /// <summary>Binding для IsEnabled кнопки — обчислювана властивість.</summary>
        public bool CanProcess => !_isProcessing;

        public string StatusText
        {
            get => _statusText;
            set => SetField(ref _statusText, value);
        }

        public async Task ProcessAsync()
        {
            IsProcessing = true;
            Progress     = 0;
            StatusText   = "Обробка...";

            for (int i = 1; i <= 100; i++)
            {
                Progress = i;
                await Task.Delay(30);
            }

            StatusText   = "Сигнал оброблено успішно!";
            IsProcessing = false;
        }
    }
}
