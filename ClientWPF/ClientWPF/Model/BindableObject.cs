using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace ClientWPF.Model
{
    class BindableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private Dictionary<int, PropertyChangedEventArgs> Cache;
        protected bool EnableRaise = true;

        public BindableObject()
        {
            this.Cache = new Dictionary<int, PropertyChangedEventArgs>();
        }

        protected PropertyChangedEventArgs GetEventArgs(string propertyName)
        {
            PropertyChangedEventArgs result;

            this.Cache.TryGetValue(propertyName.GetHashCode(), out result);
            if (result == null)
            {
                result = new PropertyChangedEventArgs(propertyName);
                this.Cache[propertyName.GetHashCode()] = result;
            }
            return result;
        }

        protected void RaisePropertyChange(string propertyName)
        {
            if (!EnableRaise) return;
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
