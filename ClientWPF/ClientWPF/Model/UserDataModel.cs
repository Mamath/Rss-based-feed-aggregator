using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using ClientWPF.ViewModel;
using ClientWPF.AccountService;
using System.IO;



namespace ClientWPF.Model
{
    class UserDataModel : BindableObject
    {
        static public void SaveConnectionString()
        {
            if (Instance == null)
                return;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("ConnectionString");
            config.AppSettings.Settings.Add("ConnectionString", Instance.ConnectionString);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        static private string LoadConnectionString()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["ConnectionString"] != null)
                return config.AppSettings.Settings["ConnectionString"].Value;
            return "";
        }

        private static UserDataModel _instance = new UserDataModel();
        public static UserDataModel Instance { get { return _instance; } }

        private string _connectionString;
        private string ConnectionString { 
            get
            {
                return _connectionString;
            } 
            set
            {
                _connectionString = value;
                if (_connectionString != null)
                    SaveConnectionString();
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
                        accountClient = new AccountClient("BasicHttpBinding_Account");
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                if (accountClient.State == CommunicationState.Closed)
                    accountClient.Open();
                return accountClient;
            }
        }

        public bool IsConnected { get; private set; }
        public bool IsUpdated { get; private set; }

        private UserDataModel()
        {
            try
            {
                AccountClient.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(OnEndLogin);
                AccountClient.RegisterCompleted += new EventHandler<RegisterCompletedEventArgs>(OnEndRegister);
                AccountClient.IsConnectedCompleted += new EventHandler<IsConnectedCompletedEventArgs>(OnIsConnected);
                AccountClient.UpdateDataCompleted += new EventHandler<UpdateDataCompletedEventArgs>(OnEndUpdateData);
                IsConnected = false;
                if (ConnectionString == null)
                {
                    _connectionString = LoadConnectionString();
                    AccountClient.IsConnectedAsync(ConnectionString);
                }
            }
            catch (Exception)
            {

                // HACK BIZARRE BUG DE BLEND
            }
        }

        public Resultat.ErrorCode Error;
        public string ErrorText;

        public void Login(string email, string password)
        {
            AccountClient.LoginAsync(email, password);
        }

        public void Register(string email, string username, string password)
        {
            AccountClient.RegisterAsync(email, username, password);
        }

        public void Logout()
        {
            ConnectionString = "";
            IsConnected = false;
            RaisePropertyChange("IsConnected");
        }

        public void Update(string username, string password)
        {
            if (username != null)
                User.Username = username;
            if (password != null)
                User.Password = password;
            AccountClient.UpdateDataAsync(ConnectionString, User);
        }

        public void OnEndUpdateData(object sender, UpdateDataCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                Error = Resultat.ErrorCode.INTERNAL_ERROR;
                ErrorText = args.Error.Message;
                RaisePropertyChange("Error");
                IsUpdated = false;
                return;
            }
            if (args.Result._error != Resultat.ErrorCode.SUCCESS)
            {
                Error = args.Result._error;
                ErrorText = "WebService Error";
                RaisePropertyChange("Error");
                IsUpdated = false;
            }
            else
            {
                IsUpdated = true;
            }
            RaisePropertyChange("IsUpdated");
        }

        public void OnEndLogin(object sender, LoginCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                Error = Resultat.ErrorCode.INTERNAL_ERROR;
                ErrorText = args.Error.Message;
                RaisePropertyChange("Error");
                return;
            }
            if (args.Result._error != Resultat.ErrorCode.SUCCESS)
            {
                Error = args.Result._error;
                ErrorText = "WebService Error";
                RaisePropertyChange("Error");
                IsConnected = false;
            }
            else
            {
                ConnectionString = args.Result._val1;
                User = args.Result._val2;
                IsConnected = true;
            }
            RaisePropertyChange("IsConnected");
        }

        public void OnEndRegister(object sender, RegisterCompletedEventArgs args)
        {
            if (!ErrorModel.Instance.EvalResponse(args)) return;
        }

        public void OnIsConnected(object sender, IsConnectedCompletedEventArgs args)
        {
            if (!ErrorModel.Instance.EvalResponse(args)) return;
            if (!ErrorModel.Instance.EvalWebResult(args.Result)) return;
            IsConnected = args.Result._val1;
            User = args.Result._val2;
            RaisePropertyChange("IsConnected");
        }

        public string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
