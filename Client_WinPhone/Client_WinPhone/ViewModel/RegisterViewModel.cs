using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Client_WinPhone.Model;
using System.ComponentModel;
using Client_WinPhone.Utils;
using System.Windows;
using System.Windows.Navigation;
using System.Diagnostics;
using Microsoft.Phone.Controls;

namespace Client_WinPhone.ViewModel
{
    public class RegisterViewModel : BindableObject
    {
        public ICommand Register { get; private set; }
        private UserDataModel UserData { get; set; }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChange("Username");
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChange("Password");
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChange("Email");
            }
        }

        private bool _registered;
        public bool Registered
        {
            get { return _registered; }
            set
            {
                _registered = value;
                RaisePropertyChange("Registered");
            }
        }

        private PropertyChangedEventHandler PropertyChangedHandler { get; set; }

        public RegisterViewModel()
        {
            UserData = UserDataModel.Instance;

            PropertyChangedHandler = new System.ComponentModel.PropertyChangedEventHandler(UserData_PropertyChanged);
            UserDataModel.Instance.PropertyChanged += PropertyChangedHandler;
            Register = new RelayCommand((param) => RegisterBody(param as string[]));
        }

        ~RegisterViewModel()
        {
            UserDataModel.Instance.PropertyChanged -= PropertyChangedHandler;
        }

        void UserData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsConnected")
            {
                Registered = (sender as UserDataModel).IsRegistered;
                RaisePropertyChange("Registered");
                if (Registered)
                {
                    PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
                    bool success = frame.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }

            }
        }

        private void RegisterBody(string[] param)
        {
            UserData.Register(Email, Username, Password);
        }
    }
}
