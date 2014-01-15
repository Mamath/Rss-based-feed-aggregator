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
    class MainWindowViewModel : BindableObject
    {
        public ICommand CloseCommand { get; private set; }
        public ICommand Logout { get; private set; }
        public ICommand ShowConnectionDialog { get; private set; }
        public ICommand OpenUpdate { get; private set; }

        public LoginModal ConnectionModal { get; private set; }
       // public ErrorViewModel ErrorModel { get; private set; }

        private bool isAdmin;
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; RaisePropertyChange("IsAdmin"); }
        }


        private PropertyChangedEventHandler PropertyChangedHandler { get; set; }

        public MainWindowViewModel()
        {
            CloseCommand = new RelayCommand((param) => Application.Current.Shutdown());
            ShowConnectionDialog = new RelayCommand((param) =>
            {
                ShowConnectionModel_IFN();
            });
            Logout = new RelayCommand((param) => UserDataModel.Instance.Logout());
            PropertyChangedHandler = new System.ComponentModel.PropertyChangedEventHandler(UserData_PropertyChanged);
            OpenUpdate = new RelayCommand((param) => 
                {
                    new UpdateWindow().Show();
                });
            UserDataModel.Instance.PropertyChanged += PropertyChangedHandler;
            
            //search = SearchDataModel.Instance.Search;
            IsAdmin = UserDataModel.Instance.IsConnected;
            IsAdmin = false;

            //ErrorModel = new ErrorViewModel();
        }

        ~MainWindowViewModel()
        {
            UserDataModel.Instance.PropertyChanged -= PropertyChangedHandler;
        }

        void UserData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsConnected")
            {

                if (!(sender as UserDataModel).IsConnected)
                {
                    if (ConnectionModal == null)
                    {
                        ConnectionModal = new LoginModal();
                        ConnectionModal.ShowDialog();
                        ConnectionModal = null;
                    }
                    FeedDataModel.Instance.GetAllRootFeeds();
                    IsAdmin = false;
                }
                else
                {
                    if (ConnectionModal != null)
                        ConnectionModal.Close();
                    ConnectionModal = null;
                    if (UserDataModel.Instance.User != null)
                    {
                        IsAdmin = UserDataModel.Instance.User.Admin;
                        FeedDataModel.Instance.GetAllRootFeeds();
                    }
                }
            }
            IsAdmin = true;
        }

        void ShowConnectionModel_IFN()
        {
            if (!UserDataModel.Instance.IsConnected)
            {
                if (ConnectionModal == null)
                {
                    ConnectionModal = new LoginModal();
                    ConnectionModal.ShowDialog();
                    ConnectionModal = null;
                }
            }

        }

    }
}
