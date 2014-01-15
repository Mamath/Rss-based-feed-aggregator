using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Net;
using System.ServiceModel.Channels;
using ClientWPF.FeedService;
using System.ServiceModel;
using ClientWPF.Model;
using ClientWPF.Utils;
using ClientWPF.View;

namespace ClientWPF.ViewModel
{
    class FeedDetailsViewModel : BindableObject
    {
        private List<ItemData> _items;
        private FeedDetailsDataModel feedDetailsDataModel;

        public ICommand ReadItem { get; private set; }

        public ChannelData RootChannel { get; private set; }
        public List<ItemData> Items
        {
            get { return _items; }
            private set { _items = value; RaisePropertyChange("Items"); }
        }

        public string WindowTitle
        {
            get { return RootChannel.Title + " (" + RootChannel.Link + ")"; }
        }

        public FeedDetailsViewModel()
        {
            RootChannel = new ChannelData();
            RootChannel.Title = "Titre du channel";
            RootChannel.Description = "";
            RootChannel.Link = "http://www.google.com";

            Items = new List<ItemData>();
            for (int i = 0; i < 20; ++i)
            {
                ItemData item = new ItemData();
                item.Title = "Titre de l'item " + i.ToString();
                item.Description = "";
                item.PubDate = DateTime.Now;
                Items.Add(item);

            }

            ReadItem = new RelayCommand((param) => ReadItemBody(param as ItemData));
        }

        public FeedDetailsViewModel(ChannelData channel)
        {
            RootChannel = channel;
            feedDetailsDataModel = new FeedDetailsDataModel(channel);
            feedDetailsDataModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(feedDetailsDataModel_PropertyChanged);
            Items = feedDetailsDataModel.Items;

            ReadItem = new RelayCommand((param) => ReadItemBody(param as ItemData));
        }

        private void ReadItemBody(ItemData item)
        {
            feedDetailsDataModel.ReadItem(item);
        }

        void feedDetailsDataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Items")
            {
                Items = (sender as FeedDetailsDataModel).Items;
            }
        }

    }
}
