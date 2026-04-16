using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfSignalApp
{
    public partial class Page3 : Page
    {
        private readonly Random _random = new Random();

        public Page3()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                DrawSignal();
                UpdateWaveColor();
            };
            ThemeManager.ThemeChanged += UpdateWaveColor;
            Unloaded += (_, _) => ThemeManager.ThemeChanged -= UpdateWaveColor;
        }

        private void UpdateWaveColor()
        {
            waveLine.Stroke = ThemeManager.IsDark
                ? Brushes.Lime
                : Brushes.Purple;
        }

        private void DrawSignal()
        {
            //Зображення
            try
            {
                int imageNumber = _random.Next(1, 11);
                string baseDir  = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = System.IO.Path.Combine(baseDir, "Images", $"s{imageNumber}.png");

                if (System.IO.File.Exists(imagePath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource    = new Uri(imagePath, UriKind.Absolute);
                    bitmap.CacheOption  = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    MyImage.Source = bitmap;
                }
                else
                {
                    MessageBox.Show($"Файл не знайдено:\n{imagePath}", "Помилка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження зображення:\n{ex.Message}", "Помилка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            //Синусоїда
            try
            {
                waveLine.Points.Clear();

                switch (_random.Next(3))
                {
                    case 0:
                        for (int x = 0; x < 800; x++)
                            waveLine.Points.Add(new Point(x, 120 + 50 * Math.Sin(x * 0.05)));
                        break;
                    case 1:
                        for (int x = 0; x < 800; x++)
                            waveLine.Points.Add(new Point(x, 120 + 50 * Math.Cos(x * 0.05)));
                        break;
                    case 2:
                        for (int x = 0; x < 800; x++)
                            waveLine.Points.Add(new Point(x, 120 + 50 * Math.Sin(x * 0.05) * Math.Cos(x * 0.03)));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка малювання сигналу:\n{ex.Message}", "Помилка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
