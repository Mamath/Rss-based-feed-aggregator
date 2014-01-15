using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Client_WinPhone.Utils;
using System.IO;
using System.Configuration;
using Client_WinPhone.ServAcc;

namespace Client_WinPhone.Model
{
    public class UserDataModel : BindableObject
    {
        private static UserDataModel _instance = new UserDataModel();
        public static UserDataModel Instance { get { return _instance; } }

        private string _connectionString;
        private string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }
        private AccountClient accountClient = null;
        static AccountData user { get; set; }

        public AccountData User
        {
            get { return user; }
            private set { user = value; }
        }

        private AccountClient AccountClient
        {
            get
            {
                if (accountClient == null)
                {
                    try
                    {
                        accountClient = new AccountClient();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                if (accountClient.State == CommunicationState.Closed)
                    accountClient.OpenAsync();
                return accountClient;
            }
        }

        private UserDataModel()
        {
            try
            {
                AccountClient.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(OnEndLogin);
                AccountClient.RegisterCompleted += new EventHandler<RegisterCompletedEventArgs>(OnEndRegister);
                AccountClient.IsConnectedCompleted += new EventHandler<IsConnectedCompletedEventArgs>(OnIsConnected);
                IsConnected = false;
                IsRegistered = false;
            }
            catch (Exception)
            {
                // HACK BIZARRE BUG DE BLEND
            }
        }

        public bool IsConnected { get; private set; }
        public bool IsRegistered { get; private set; }

        public void Login(string username, string password)
        {
            AccountClient.LoginAsync(username, password);
        }

        public void Register(string email, string username, string password)
        {
            AccountClient.RegisterAsync(email, username, password);
        }

        public void Logout(string username, string password)
        {
            //AccountClient.LogoutAsync(ConnectionString);
            ConnectionString = null;
        }

        public void Update()
        {
            AccountClient.UpdateDataAsync(ConnectionString, User);
        }

        public void Logout()
        {
            ConnectionString = "";
            IsConnected = false;
            RaisePropertyChange("IsConnected");
        }

        private void OnIsConnected(object sender, IsConnectedCompletedEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.Result._error == Resultat.ErrorCode.SUCCESS)
                {
                    IsConnected = args.Result._val1;
                    RaisePropertyChange("IsConnected");
                }
            }
        }

        private void OnEndLogin(object sender, LoginCompletedEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.Result._error == Resultat.ErrorCode.SUCCESS)
                {
                    ConnectionString = args.Result._val1;
                    IsConnected = true;
                    User = args.Result._val2;
                    //LoginView.Close();
                }
                else
                    IsConnected = false;
                RaisePropertyChange("IsConnected");
            }
        }

        private void OnEndRegister(object sender, RegisterCompletedEventArgs args)
        {
            if (args.Error == null)
            {
            }
        }

        public string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
