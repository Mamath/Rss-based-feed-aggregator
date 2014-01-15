using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using ServerRss.Entity;
using ServerRss.Tools;

namespace ServerRss.Services
{
    [DataContract]
    public class AccountData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public bool Admin { get; set; }
        [DataMember]
        public string Email { get; set; }

        public AccountData() { }
        public AccountData(User user)
        {
            Id = user.id;
            Username = user.username;
            Password = user.password;
            Email = user.email;
            Admin = user.superuser;
        }
    }

    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Account" à la fois dans le code, le fichier svc et le fichier de configuration.
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Account : IAccount
    {

        ServerDataContext _db = new ServerDataContext();
        SessionTools _session = new SessionTools();

        private CompositionContainer _container;

        public Account()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Account).Assembly));

            _container = new CompositionContainer(catalog);
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException c)
            {
                Console.WriteLine(c.ToString());
            }
        }

        [OperationContract]
        public Resultat<string, AccountData> Login(string email, string password)
        {
            try
            {
                var users = from u in this._db.User where u.email == email && u.password == password select u;
                if (users.Count() > 1)
                    return new Resultat<string, AccountData>(Resultat.ErrorCode.INTERNAL_ERROR);
                if (users.Count() == 0)
                    return new Resultat<string, AccountData>(Resultat.ErrorCode.USER_NOT_FOUND);
                User user = (users).Single();
                Session s = this._session.CreateSession(user);
                return (new Resultat<String, AccountData>(s.session_key, new AccountData(user)));
            }
            catch
            {
                return new Resultat<string, AccountData>(Resultat.ErrorCode.USER_NOT_FOUND);
            }
        }

        [OperationContract]
        public Resultat Register(string email, string username, string password)
        {
            try
            {
                var users = from u in this._db.User where u.email == email select u;
                if (username == "" || password == "" || email == "")
                    return new Resultat(Resultat.ErrorCode.INFORMATION_REQUIRED);
                if (users.Count() > 0)
                    return new Resultat(Resultat.ErrorCode.USER_ALREADY_EXIST);
                User user = new User()
                {
                    username = username,
                    email = email,
                    password = password,
                };
                this._db.User.InsertOnSubmit(user);
                this._db.SubmitChanges();
                return new Resultat();
            }
            catch
            {
                return new Resultat(Resultat.ErrorCode.INTERNAL_ERROR);
            }
        }

        [OperationContract]
        public Resultat UpdateData(string session_key, AccountData updateData)
        {
            var users = from u in this._db.User where u.id == updateData.Id select u;
            if (users.Count() != 1)
                return new Resultat(Resultat.ErrorCode.INTERNAL_ERROR);
            var user = users.Single();
            user.username = updateData.Username;
            user.password = updateData.Password;
            user.email = updateData.Email;
            user.superuser = updateData.Admin;
            this._db.SubmitChanges();
            return new Resultat(Resultat.ErrorCode.SUCCESS);
        }

        [OperationContract]
        public Resultat DeleteAccount(string session_key)
        {
            Session session = this._session.GetSession(session_key);
            if (session == null)
                return new Resultat(Resultat.ErrorCode.NOT_LOGUED);

            try
            {
                User userToDelete = this._session.GetUser(session_key);
                var users = from u in this._db.User where u.id == userToDelete.id select u;
                if (users.Count() > 1)
                    return new Resultat(Resultat.ErrorCode.INTERNAL_ERROR);
                if (users.Count() == 0)
                    return new Resultat(Resultat.ErrorCode.USER_NOT_FOUND);
                var user = users.Single();
                this._db.Session.DeleteAllOnSubmit(this._db.Session.Where(s => s.User == user));
                this._db.ChannelXUser.DeleteAllOnSubmit(this._db.ChannelXUser.Where(c => c.User == user));
                this._db.User.DeleteOnSubmit(user);
                this._db.SubmitChanges();
                return new Resultat(Resultat.ErrorCode.SUCCESS);
            }
            catch
            {
                return new Resultat(Resultat.ErrorCode.USER_NOT_FOUND);
            }
        }

        [OperationContract]
        public Resultat UpdatePassword(string session_key, string password)
        {
            Session session = this._session.GetSession(session_key);
            if (session == null)
                return new Resultat(Resultat.ErrorCode.NOT_LOGUED);
            User user = this._session.GetUser(session_key);

            try
            {
                var users = from u in this._db.User where u.id == user.id select u;
                if (users.Count() > 1)
                    return new Resultat(Resultat.ErrorCode.INTERNAL_ERROR);
                if (users.Count() == 0)
                    return new Resultat(Resultat.ErrorCode.USER_NOT_FOUND);

                var userP = users.Single();
                userP.password = password;
                this._db.SubmitChanges();
                return new Resultat(Resultat.ErrorCode.SUCCESS);
            }
            catch
            {
                return new Resultat(Resultat.ErrorCode.INTERNAL_ERROR);
            }
        }

        [OperationContract]
        public Resultat<bool, AccountData> IsConnected(string session_key)
        {
            Session session = _session.GetSession(session_key);
            if (session == null)
                return new Resultat<bool, AccountData>(false, null);
            User user = session.User;
            return new Resultat<bool, AccountData>(true, new AccountData(user));
        }
    }
}
