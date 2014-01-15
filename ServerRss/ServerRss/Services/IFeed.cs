using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ServerRss.Tools;
using ServerRss.Entity;

namespace ServerRss.Services
{    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IFeed" à la fois dans le code et le fichier de configuration.
    public interface IFeed
    {
        Resultat AddFeed(string session_key, Uri uri);
        Resultat<List<ChannelData>> GetFeeds(string session_key);
        Resultat<List<ChannelData>> GetAllFeeds();
        Resultat DeleteFeed(string session_key, ChannelData chan);
        Resultat Update(ChannelData feed);
        Resultat<List<ItemData>> GetItem(ChannelData feed, string session_key);
        Resultat ReadItem(string session_key, ItemData item);
    }
}
