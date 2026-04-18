# Localization — інструкція з інтеграції

## Що додано / змінено

### Новий файл
| Файл | Куди покласти у проєкті |
|------|------------------------|
| `LocalizationManager.cs` | `WpfSignalApp/` (поряд з `ThemeManager.cs`) |

### Змінені файли (замінити повністю)
| Файл | Розташування |
|------|-------------|
| `MainWindow.xaml` + `.cs` | `WpfSignalApp/` |
| `HomePage.xaml` + `.cs` | `WpfSignalApp/` |
| `Page1.xaml` | `WpfSignalApp/` |
| `Page2.xaml` | `WpfSignalApp/` |
| `Page3.xaml` + `.cs` | `WpfSignalApp/` |
| `SettingsPage.xaml` + `.cs` | `WpfSignalApp/` |
| `ViewModels/Page1ViewModel.cs` | `WpfSignalApp/ViewModels/` |
| `ViewModels/Page2ViewModel.cs` | `WpfSignalApp/ViewModels/` |
| `ViewModels/SettingsViewModel.cs` | `WpfSignalApp/ViewModels/` |

---

## Як це працює

```
LocalizationManager          (статичний клас-singleton)
    ├── CurrentLanguage      "en" | "uk"
    ├── this[key]            → повертає локалізований рядок
    ├── Format(key, args)    → string.Format поверх this[key]
    ├── SetLanguage(lang)    → змінює мову + стріляє LanguageChanged
    └── LanguageChanged      → подія, на яку підписуються VM і code-behind
```

### Як ViewModel реагує на зміну мови
```csharp
// У конструкторі VM:
LocalizationManager.LanguageChanged += OnLanguageChanged;

// Обробник:
private void OnLanguageChanged()
{
    OnPropertyChanged(nameof(SomeComputedLabel));  // computed prop
    StatusText = LocalizationManager["some.key"];   // динамічний стан
}
```

### Computed-властивості (статичні лейбли кнопок)
```csharp
// Без поля — просто getter:
public string BtnInitText => LocalizationManager["p1.btn.init"];

// OnPropertyChanged викликається вручну в OnLanguageChanged()
```

---

## Додавання нової мови

1. Відкрити `LocalizationManager.cs`
2. Додати новий ключ у масив:
```csharp
public static readonly string[] AvailableLanguages = { "en", "uk", "de" };
```
3. Додати словник:
```csharp
["de"] = new Dictionary<string, string>
{
    ["nav.home"] = "🏠 Startseite",
    // ... всі ключі
}
```
4. У `SettingsPage.xaml` — додати кнопку `BtnGerman_Click`
5. У `SettingsViewModel` — додати метод `SetGerman()` і кнопку з фоном

---

## Додавання нового рядка

1. Додати ключ у обидва словники (`en` і `uk`) у `LocalizationManager.cs`
2. У ViewModel — або `public string MyLabel => LocalizationManager["my.key"];`
   + `OnPropertyChanged(nameof(MyLabel))` в `OnLanguageChanged()`
3. У XAML — `Text="{Binding MyLabel}"`
