using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.ServiceModel.Syndication;
using ServerRss.Entity;

namespace ServerRss.Tools
{
    public class ChannelManager
    {
        public Channel CreateChannel(Uri uri, ServerDataContext db)
        {
            XmlReader xml = XmlReader.Create(uri.ToString());
            try
            {
                SyndicationFeed feed = SyndicationFeed.Load(xml);

                Channel chan = null;
                var TestChan = from c in db.Channel where c.url == uri.ToString() select c;
                if (TestChan.Count() > 1)
                    return null;
                else if (TestChan.Count() == 1)
                {
                    chan = TestChan.Single();
                    UpdateChannel(chan, db);
                }
                else
                {

                    chan = new Channel()
                    {
                        title = feed.Title.Text,
                        link = feed.Links[0].Uri.AbsoluteUri,
                        description = feed.Description.Text,
                        url = uri.AbsoluteUri,
                        lastBuildDate = feed.LastUpdatedTime.DateTime,
                        image = feed.ImageUrl != null ? feed.ImageUrl.ToString() : null
                    };

                    db.Channel.InsertOnSubmit(chan);
                    db.SubmitChanges();
                    foreach (SyndicationItem item in feed.Items)
                        CreateItem(item, chan, db);
                }
                return chan;
            }
            catch
            {
                return null;
            }
            /*if (feed == null)
                return null;

            Channel chan = null;
            var TestChan = from c in db.Channel where c.url == uri.ToString() select c;
            if (TestChan.Count() > 1)
                return null;
            else if (TestChan.Count() == 1)
            {
                chan = TestChan.Single();
                UpdateChannel(chan, db);
            }
            else
            {

                chan = new Channel()
                {
                    title = feed.Title.Text,
                    link = feed.Links[0].Uri.AbsoluteUri,
                    description = feed.Description.Text,
                    url = uri.AbsoluteUri,
                    lastBuildDate = feed.LastUpdatedTime.DateTime,
                    image = feed.ImageUrl != null ? feed.ImageUrl.ToString() : null
                };

                db.Channel.InsertOnSubmit(chan);
                db.SubmitChanges();
                foreach (SyndicationItem item in feed.Items)
                    CreateItem(item, chan, db);
            }
            return chan;*/
        }

        private void CreateItem(SyndicationItem item, Channel chan, ServerDataContext db)
        {
            Item _item = null;
            string _itemId = item.Id;
            bool NewItem = false;

            if (_itemId != null)
            {
                var items = from i in db.Item where i.Channel.id == chan.id && i.guid == _itemId select i;
                if (items.Count() > 1)
                    return;
                if (items.Count() == 1)
                    _item = items.Single();
            }

            if (_item == null)
            {
                _item = new Item();
                NewItem = true;
            }

            _item.id_channel = chan.id;
            _item.title = item.Title != null ? item.Title.Text : "";
            _item.link = item.Links.Count > 0 ? item.Links[0].Uri.AbsoluteUri : "";
            _item.pubDate = item.LastUpdatedTime > item.PublishDate ? item.LastUpdatedTime : item.PublishDate;
            _item.description = item.Summary != null ? item.Summary.Text : "";
            _item.guid = item.Id != null ? item.Id : "";
            _item.author = item.Authors.Count > 0 ? item.Authors[0].Email : "";
            _item.category = item.Categories.Count > 0 ? item.Categories[0].Name : "";
            _item.comments = "";

            if (NewItem == true)
                db.Item.InsertOnSubmit(_item);
            db.SubmitChanges();
        }

        public void UpdateChannel(Channel chan, ServerDataContext db)
        {
            XmlReader xml = XmlReader.Create(chan.url);
            SyndicationFeed feed = SyndicationFeed.Load(xml);

            if (feed == null)
                return;

            var ChanToUpdate = (from c in db.Channel where chan.id == c.id select c).SingleOrDefault();
            ChanToUpdate.title = feed.Title.Text;
            ChanToUpdate.link = feed.Links[0].Uri.AbsoluteUri;
            ChanToUpdate.description = feed.Description.Text;
            ChanToUpdate.lastBuildDate = feed.LastUpdatedTime.DateTime;
            ChanToUpdate.image = feed.ImageUrl != null ? feed.ImageUrl.ToString() : null;
            db.SubmitChanges();

            foreach (SyndicationItem item in feed.Items)
                CreateItem(item, chan, db);
        }
    }
}