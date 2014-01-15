using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Client_WinPhone.Utils
{
    public class BindableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private Dictionary<int, PropertyChangedEventArgs> cacheEventArgs;
        protected bool EnableRaisePropertyChanged = true;

        public BindableObject()
        {
            this.cacheEventArgs = new Dictionary<int, PropertyChangedEventArgs>();
        }

        protected PropertyChangedEventArgs GetEventArgs(string propertyName)
        {
            PropertyChangedEventArgs result;

            this.cacheEventArgs.TryGetValue(propertyName.GetHashCode(), out result);
            if (result == null)
            {
                result = new PropertyChangedEventArgs(propertyName);
                this.cacheEventArgs[propertyName.GetHashCode()] = result;
            }
            return result;
        }

        protected void RaisePropertyChange(string propertyName)
        {
            if (!EnableRaisePropertyChanged) return;
            VerifyProperty(propertyName);
            PropertyChangedEventArgs args = GetEventArgs(propertyName);
            PropertyChanged(this, args);
        }

        [Conditional("DEBUG")]
        private void VerifyProperty(string propertyName)
        {
            Type type = this.GetType();

            type.GetHashCode();

            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new Exception(string.Format("{0} is not a public property of {1}", propertyName, type.FullName));
            }
        }
    }
}
