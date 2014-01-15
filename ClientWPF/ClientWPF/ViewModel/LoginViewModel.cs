using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel;
using ClientWPF.Model;
using ClientWPF.Utils;
using ClientWPF.View;

namespace ClientWPF.ViewModel
{
    class LoginViewModel : BindableObject
    {
        public ICommand Login { get; private set; }
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

        private bool _register;
        public bool Register
        {
            get { return _register; }
            set
            {
                _register = value;
                RaisePropertyChange("Register");
            }
        }


        private bool _logued;
        public bool Logued
        {
            get { return _logued; }
            set
            {
                _logued = value;
                RaisePropertyChange("Logued");
            }
        }

        private PropertyChangedEventHandler PropertyChangedHandler { get; set; }

         public LoginViewModel()
        {
            UserData = UserDataModel.Instance;
            PropertyChangedHandler = new System.ComponentModel.PropertyChangedEventHandler(UserData_PropertyChanged);
            UserDataModel.Instance.PropertyChanged += PropertyChangedHandler;
            Login = new RelayCommand((param) => LoginBody(param as string[]));
            _logued = false;
        }

         ~LoginViewModel()
        {
            UserDataModel.Instance.PropertyChanged -= PropertyChangedHandler;
        }

         void UserData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
         {
             if (e.PropertyName == "IsConnected")
             {
                 Logued = (sender as UserDataModel).IsConnected;
                 RaisePropertyChange("Logued");
             }
         }

         private void LoginBody(string[] param)
         {
            UserData.Login(Username, Password);
         }
    }
}
