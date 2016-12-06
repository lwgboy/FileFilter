using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace MMD5Ceshi
{
    public class GetCornerRadiusByHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return new CornerRadius(0, 0, 0, 0);
            var flatdouble = ((double) value)/2;
            return new CornerRadius(flatdouble, flatdouble, flatdouble, flatdouble);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
