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
    class FeedManagerViewModel : BindableObject
    {
        private FeedDataModel FeedsManager { get; set; }

        public ICommand RefreshFeed { get; private set; }
        public ICommand RefreshFeeds { get; private set; }
        public ICommand AddFeed { get; private set; }
        public ICommand RemoveFeed { get; private set; }
        public ICommand LoadFeedItems { get; private set; }
        public ICommand OpenFeedDetails { get; private set; }

        private List<ChannelData> channels = null;
        public List<ChannelData> Channels
        {
            get { return channels; }
            private set { channels = value; RaisePropertyChange("Channels"); }
        }

        private List<ChannelData> allChannels = null;
        public List<ChannelData> AllChannels
        {
            get { return allChannels; }
            private set { allChannels = value; RaisePropertyChange("AllChannels"); }
        }

        private List<ItemData> items = null;
        public List<ItemData> Items
        {
            get { return items; }
            private set { items = value; RaisePropertyChange("Items"); }
        }

        private string urlFeed = "";
        public string UrlFeed
        {
            get { return urlFeed; }
            set { urlFeed = value; RaisePropertyChange("UrlFeed"); }
        }

        private ChannelData currentChannel = null;
        public ChannelData CurrentChannel
        {
            get { return currentChannel; }
            set { currentChannel = value; RaisePropertyChange("CurrentChannel"); }
        }

        public FeedManagerViewModel()
        {
            FeedsManager = FeedDataModel.Instance;
            FeedsManager.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(FeedsManager_PropertyChanged);

            RefreshFeeds = new RelayCommand((param) => FeedsManager.GetAllRootFeeds());
            RefreshFeed = new RelayCommand((param) => FeedsManager.RefreshFeed(param as ChannelData));
            AddFeed = new RelayCommand((param) => AddFeedBody(param as string));
            RemoveFeed = new RelayCommand((param) => FeedsManager.RemoveFeed(param as ChannelData));
            LoadFeedItems = new RelayCommand((param) => FeedsManager.LoadFeedItems(param as ChannelData));

            OpenFeedDetails = new RelayCommand((param) => (new Channel()
            {
                DataContext = new FeedDetailsViewModel(param as ChannelData)
            }).Show());

            Channels = new List<ChannelData>();
        }

        void FeedsManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Channels")
            {
                Channels = (sender as FeedDataModel).Channels;
            }
            else if (e.PropertyName == "Items")
                Items = (sender as FeedDataModel).Items;
            else if (e.PropertyName == "AllChannels")
                AllChannels = (sender as FeedDataModel).AllChannels;
        }

        private void AddFeedBody(string url)
        {
            FeedsManager.AddNewFeed(url);
        }
    }
}
