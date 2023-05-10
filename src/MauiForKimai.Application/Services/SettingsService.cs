using LiveChartsCore.Themes;
using MauiForKimai.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Services;
public class SettingsService : ObservableObject
{
    private static SettingsService _instance;
    public static SettingsService Instance => _instance ??= new SettingsService();

    private SettingsService()
    {
        Theme = Theme.System;
    }

    private Theme _theme;
    public Theme Theme
    {
        get => _theme;
        set
        {
           if(_theme == value) return;
            _theme = value;
           OnPropertyChanged();
        }
    }
}
