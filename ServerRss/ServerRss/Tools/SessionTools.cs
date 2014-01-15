using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerRss.Entity;

namespace ServerRss.Tools
{
    public class SessionTools
    {
         ServerDataContext db = new ServerDataContext();

        public Session GetSession(string key)
        {
            try
            {
                var sessions = from s in db.Session where s.session_key == key select s;
                if (sessions.Count() == 1)
                    return (sessions.Single());
            }
            catch
            {
                return null;
            }
            return null;
        }

        public Session CreateSession(User user)
        {
            Session session = new Session()
            {
                id_user = user.id,
                session_key = Guid.NewGuid().ToString(),
                expire = DateTime.Now
            };
            db.Session.InsertOnSubmit(session);
            db.SubmitChanges();
            return GetSession(session.session_key);
        }

        public void DeleteSession(string key)
        {
            try
            {
                var sessions = from s in db.Session where s.session_key == key select s;
                if (sessions.Count() == 1)
                {
                    db.Session.DeleteOnSubmit(sessions.Single());
                    db.SubmitChanges();
                }
            }
            catch { }
        }

        public User GetUser(string key)
        {
            Session s = GetSession(key);
            if (s == null)
                return null;
            var users = from u in db.User where u.id == s.id_user select u;
            if (users.Count() == 1)
                return users.Single();
            return null;
        }
    }
}