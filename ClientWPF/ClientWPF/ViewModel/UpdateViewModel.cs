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
    class UpdateViewModel : BindableObject
    {
        public ICommand UpdateData { get; private set; }
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

        private bool _updtate;
        public bool Update
        {
            get { return _updtate; }
            set
            {
                _updtate = value;
                RaisePropertyChange("Update");
            }
        }

        private bool _updated;
        public bool Updated
        {
            get { return _updated; }
            set
            {
                _updated = value;
                RaisePropertyChange("Updated");
            }
        }

        private PropertyChangedEventHandler PropertyChangedHandler { get; set; }

        public UpdateViewModel()
        {
            UserData = UserDataModel.Instance;
            PropertyChangedHandler = new System.ComponentModel.PropertyChangedEventHandler(UserData_PropertyChanged);
            UserDataModel.Instance.PropertyChanged += PropertyChangedHandler;
            UpdateData = new RelayCommand((param) => UpdateBody(param as string[]));
            _updated = false;
        }

        void UserData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsUpdated")
            {
                Updated = (sender as UserDataModel).IsUpdated;
                RaisePropertyChange("Updated");
            }
        }

        public void UpdateBody(string[] param)
        {
            UserData.Update(Username, Password);
        }
    }
}
