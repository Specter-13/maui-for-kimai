using Microsoft.Maui.Graphics.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Converters;
public class StringToColorConverter : IValueConverter
{
  private ColorTypeConverter _converter = new ColorTypeConverter();
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    Color color = Colors.Transparent;
    if(value != null) 
        color = (Color)(_converter.ConvertFromInvariantString((string)value));
    return color;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    string colorString = "White"; //TODO

    return colorString;
  }
}