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

    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IAccount" à la fois dans le code et le fichier de configuration.
    public interface IAccount
    {
        Resultat<string, AccountData> Login(string email, string password);
        Resultat Register(string email, string username, string password);
        Resultat UpdateData(string session_key, AccountData updateData);
        Resultat DeleteAccount(string session_key);
        Resultat UpdatePassword(string session_key, string password);
        Resultat<bool, AccountData> IsConnected(string session_key);
    }
}
