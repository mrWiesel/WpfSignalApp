using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfSignalApp
{
    /// <summary>
    /// Code-behind для Page3.
    /// Малювання Canvas (Polyline) і завантаження зображень не може бути замінено Binding-ом —
    /// це легітимний виняток, де code-behind виправданий.
    /// DataContext тут не потрібен, бо немає даних-властивостей для відображення.
    /// </summary>
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
            waveLine.Stroke = ThemeManager.IsDark ? Brushes.Lime : Brushes.Purple;
        }

        private void DrawSignal()
        {
            // Зображення через pack URI — працює незалежно від папки
            try
            {
                int imageNumber = _random.Next(1, 11);

                // pack:// URI — зображення вбудоване в .exe після компіляції
                var uri = new Uri($"pack://application:,,,/Assets/Images/s{imageNumber}.png");

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = uri;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                MyImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження зображення:\n{ex.Message}");
            }

            // Синусоїда
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
                MessageBox.Show($"Помилка малювання сигналу:\n{ex.Message}");
            }
        }
    }
}