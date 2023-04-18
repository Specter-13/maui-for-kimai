using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Helpers;
public sealed class Theme
{
    public static Theme Dark = new(AppTheme.Dark, "Dark mode");
    public static Theme Light = new(AppTheme.Light, "Ligth mode");
    public static Theme System = new(AppTheme.Unspecified, "Follow System");

    public static List<Theme> AvailableThemes { get; } = new()
    {
        Dark,
        Light,
        System
    };

    public AppTheme AppTheme { get; set;}
    public string DisplayName { get; }

    private Theme(AppTheme theme, string displayName)
    {
        AppTheme = theme;
        DisplayName = displayName;
    }
}
