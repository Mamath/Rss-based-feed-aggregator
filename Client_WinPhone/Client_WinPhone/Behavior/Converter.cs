using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;


namespace Client_WinPhone.Behavior
{
    public class Converter
    {
        public class StringToHtmlPageConverter : IValueConverter
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

        public class DateConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DateTimeOffset date = (DateTimeOffset)value;
                return date.DateTime.ToShortDateString() + " " + date.DateTime.ToShortTimeString();
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
}
