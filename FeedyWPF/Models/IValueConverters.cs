using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FeedyWPF.Models
{
    public class EnumBooleanConverter : IValueConverter
    {
        //  #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, parameterString);
        }
    //    #endregion
    }

    public class BoolInverterConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if(value == null)
            {
                return null;
            }

            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        #endregion
    }
}

