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
    public class ChannelData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Link { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public DateTime? PubDate { get; set; }
        [DataMember]
        public DateTime? LastBuildDate { get; set; }
        [DataMember]
        public string Image { get; set; }

        public ChannelData() { }
        public ChannelData(Channel channel)
        {
            Id = channel.id;
            Title = channel.title;
            Description = channel.description;
            Link = channel.link;
            Url = channel.url;
            PubDate = channel.pubDate;
            LastBuildDate = channel.lastBuildDate;
            Image = channel.image;
        }
    }

    [DataContract]
    public class ItemData
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Link { get; set; }
        [DataMember]
        public DateTimeOffset? PubDate { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public bool Read { get; set; }

        public ItemData() { }
        public ItemData(Item item)
        {
            Id = item.id;
            Title = item.title;
            Description = item.description;
            Link = item.link;
            Author = item.author;
            PubDate = item.pubDate;
            Category = item.category;
            Read = false;
        }
    }

    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Feed" à la fois dans le code, le fichier svc et le fichier de configuration.
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Feed : IFeed
    {
        SessionTools _session = new SessionTools();
        ServerDataContext _db = new ServerDataContext();
        ChannelManager _chan = new ChannelManager();

        [OperationContract]
        public Resultat AddFeed(string session_key, Uri uri)
        {
            User user = _session.GetUser(session_key);
            if (user == null)
                return (new Resultat(Resultat.ErrorCode.NOT_LOGUED));
            if (uri == null)
                return (new Resultat(Resultat.ErrorCode.CANNOT_CREATE_FEED));
            Channel NewChan = _chan.CreateChannel(uri, _db);
            if (NewChan == null)
                return (new Resultat(Resultat.ErrorCode.CANNOT_CREATE_FEED));
            ChannelXUser cu = new ChannelXUser();
            cu.id_user = user.id;
            cu.id_channel = NewChan.id;

            this._db.ChannelXUser.InsertOnSubmit(cu);
            this._db.SubmitChanges();
            return new Resultat();
        }

        [OperationContract]
        public Resultat<List<ChannelData>> GetFeeds(string session_key)
        {
            User user = _session.GetUser(session_key);
            if (user == null)
                return new Resultat<List<ChannelData>>(Resultat.ErrorCode.NOT_LOGUED);
            List<ChannelData> chans = new List<ChannelData>();
            try
            {
                var channels = (from c in this._db.ChannelXUser where user.id == c.id_user select c.Channel).OrderBy(c => c.title);
                foreach (var channel in channels)
                {
                    ChannelData chan = new ChannelData(channel);
                    chans.Add(chan);
                }
                return new Resultat<List<ChannelData>>(new List<ChannelData>(chans));
            }
            catch
            {
                return new Resultat<List<ChannelData>>(Resultat.ErrorCode.INTERNAL_ERROR);
            }
        }

        [OperationContract]
        public Resultat<List<ChannelData>> GetAllFeeds()
        {
            List<ChannelData> channels = new List<ChannelData>();

            try
            {
                var _channels = this._db.Channel.ToList().OrderBy(chan => chan.title);
                foreach (var _channel in _channels)
                {
                    ChannelData chan = new ChannelData(_channel);
                    channels.Add(chan);
                }
                return new Resultat<List<ChannelData>>(new List<ChannelData>(channels));
            }
            catch
            {
                return new Resultat<List<ChannelData>>(Resultat.ErrorCode.INTERNAL_ERROR);
            }
        }

        [OperationContract]
        public Resultat DeleteFeed(string session_key, ChannelData chan)
        {
            User user = _session.GetUser(session_key);
            if (user == null)
                return new Resultat(Resultat.ErrorCode.NOT_LOGUED);
            try
            {
                var cxu = from _user in this._db.ChannelXUser where _user.User == user && _user.id_channel == chan.Id select _user;
                if (cxu != null)
                {
                    this._db.ChannelXUser.DeleteAllOnSubmit(cxu);
                    this._db.SubmitChanges();
                }
                return new Resultat();
            }
            catch
            {
                return new Resultat(Resultat.ErrorCode.CANNOT_GET_FEED);
            }
        }

        [OperationContract]
        public Resultat<List<ItemData>> GetItem(ChannelData chan, string session_key)
        {
            User user = _session.GetUser(session_key);
            if (user == null)
                return new Resultat<List<ItemData>>(Resultat.ErrorCode.NOT_LOGUED);

            if (chan == null)
                return new Resultat<List<ItemData>>(Resultat.ErrorCode.INVALID_PARAMETER);

            List<ItemData> items = new List<ItemData>();
            try
            {
                var _items = (from _item in this._db.Item where _item.id_channel == chan.Id select _item).OrderByDescending(_item => _item.pubDate);
                foreach (var _i in _items)
                {
                    var read = from r in this._db.ItemRead where r.id_item == _i.id && user.id == r.id_user select r;
                    ItemData i = new ItemData(_i);
                    if (read.Count() == 1)
                        i.Read = true;
                    else
                        i.Read = false;
                    items.Add(i);
                }
                return new Resultat<List<ItemData>>(new List<ItemData>(items));
            }
            catch
            {
                return new Resultat<List<ItemData>>(Resultat.ErrorCode.CANNOT_GET_FEED);
            }
        }

        [OperationContract]
        public Resultat ReadItem(string session_key, ItemData item)
        {
            User user = _session.GetUser(session_key);
            if (user == null)
                return new Resultat(Resultat.ErrorCode.NOT_LOGUED);
            if (item == null)
                return new Resultat(Resultat.ErrorCode.INVALID_PARAMETER);
            try
            {
                var read = (from r in this._db.ItemRead where r.id_item == item.Id && r.User == user select r);
                if (read.Count() == 1)
                    return new Resultat();
                else if (read.Count() > 1)
                    return new Resultat(Resultat.ErrorCode.INTERNAL_ERROR);
                ItemRead i = new ItemRead()
                {
                    id_user = user.id,
                    id_item = item.Id
                };
                this._db.ItemRead.InsertOnSubmit(i);
                this._db.SubmitChanges();
                return new Resultat();
            }
            catch
            {
                return new Resultat(Resultat.ErrorCode.INTERNAL_ERROR);
            }
        }

        [OperationContract]
        public Resultat Update(ChannelData chan)
        {
            try
            {
                var ChanToUpdate = from c in this._db.Channel where c.id == chan.Id select c;
                if (ChanToUpdate.Count() > 1)
                    return new Resultat(Resultat.ErrorCode.INTERNAL_ERROR);
                if (ChanToUpdate.Count() == 0)
                    return new Resultat(Resultat.ErrorCode.CANNOT_GET_FEED);
                this._chan.UpdateChannel(ChanToUpdate.Single(), this._db);
                return new Resultat();
            }
            catch
            {
                return new Resultat(Resultat.ErrorCode.INTERNAL_ERROR);
            }
        }

    }
}
