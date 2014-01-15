using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace ClientWPF.Utils
{
    public class StringToHtmlConverter : IValueConverter
    {
        private string header = "<! doctype html><html><head><meta charset='UTF-8'/></head><body><div>";
        private string footer = "</div></body></html>";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value as string;
            strValue = header + strValue + footer;
            return strValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    [ValueConversion(typeof(DateTimeOffset), typeof(String))]
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTimeOffset date = (DateTimeOffset)value;
            return date.DateTime.ToShortDateString() + " " + date.DateTime.ToShortTimeString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value as string;
            DateTimeOffset resultDateTime;
            if (DateTimeOffset.TryParse(strValue, out resultDateTime))
            {
                return resultDateTime;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
