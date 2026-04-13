using System;
using System.Windows;
using System.Windows.Media;

namespace WpfSignalApp
{
    public static class ThemeManager
    {
        public static bool IsDark { get; private set; } = true;

        public static event Action? ThemeChanged;

        public static void SetDark()
        {
            IsDark = true;
            Apply();
        }

        public static void SetLight()
        {
            IsDark = false;
            Apply();
        }

        public static void Toggle()
        {
            IsDark = !IsDark;
            Apply();
        }

        private static void Apply()
        {
            var res = Application.Current.Resources;

            if (IsDark)
            {
                res["ThemeBg"]         = new SolidColorBrush(Color.FromRgb(13,  13,  26));
                res["ThemeNavBg"]      = new SolidColorBrush(Color.FromRgb(26,  26,  46));
                res["ThemeText"]       = new SolidColorBrush(Colors.White);
                res["ThemeSubText"]    = new SolidColorBrush(Color.FromRgb(170, 170, 170));
                res["ThemeCard"]       = new SolidColorBrush(Color.FromRgb(20,  20,  40));
                res["ThemeBorder"]     = new SolidColorBrush(Colors.DarkGray);
                res["ThemeBtnBg"]      = new SolidColorBrush(Color.FromRgb(51,  51,  51));
                res["ThemeBtnFg"]      = new SolidColorBrush(Colors.White);
                res["ThemePageBg"]     = new SolidColorBrush(Colors.Black);
            }
            else
            {
                res["ThemeBg"]         = new SolidColorBrush(Color.FromRgb(235, 235, 245));
                res["ThemeNavBg"]      = new SolidColorBrush(Color.FromRgb(200, 210, 230));
                res["ThemeText"]       = new SolidColorBrush(Colors.Black);
                res["ThemeSubText"]    = new SolidColorBrush(Color.FromRgb(80,  80,  80));
                res["ThemeCard"]       = new SolidColorBrush(Color.FromRgb(220, 225, 240));
                res["ThemeBorder"]     = new SolidColorBrush(Color.FromRgb(130, 130, 160));
                res["ThemeBtnBg"]      = new SolidColorBrush(Color.FromRgb(180, 190, 215));
                res["ThemeBtnFg"]      = new SolidColorBrush(Colors.Black);
                res["ThemePageBg"]     = new SolidColorBrush(Color.FromRgb(245, 245, 255));
            }

            ThemeChanged?.Invoke();
        }

        /// <summary>Call once at startup to seed the resource dictionary.</summary>
        public static void Initialize()
        {
            // Ensure keys exist before first navigation
            Apply();
        }
    }
}
