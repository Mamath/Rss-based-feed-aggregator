using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Client_WinPhone.ServFeed;
using Client_WinPhone.ServAcc;
using Client_WinPhone.Model;
using Client_WinPhone.View;
using Client_WinPhone.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;

namespace Client_WinPhone.ViewModel
{
    public class FeedsManagerViewModel :BindableObject
    {
        private FeedDataModel FeedsManager { get; set; }

        public ICommand RefreshFeed { get; private set; }
        public ICommand RefreshFeeds { get; private set; }
        public ICommand AddFeed { get; private set; }
        public ICommand RemoveFeed { get; private set; }
        public ICommand LoadFeedItems { get; private set; }
        public ICommand OpenFeedDetails { get; private set; }
        public ICommand LogOut { get; private set; }

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

        public FeedsManagerViewModel()
        {
            FeedsManager = FeedDataModel.Instance;
            FeedsManager.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(FeedsManager_PropertyChanged);

            RefreshFeeds = new RelayCommand((param) => FeedsManager.GetAllRootFeeds());
            RefreshFeed = new RelayCommand((param) => FeedsManager.RefreshFeed(param as ChannelData));
            AddFeed = new RelayCommand((param) => AddFeedBody(param as string));
            RemoveFeed = new RelayCommand((param) => FeedsManager.RemoveFeed(param as ChannelData));
            LoadFeedItems = new RelayCommand((param) => FeedsManager.LoadFeedItems(param as ChannelData));
            OpenFeedDetails = new RelayCommand((param) => OpenDetails(param as ChannelData));

            Channels = new List<ChannelData>();
            FeedsManager.GetAllRootFeeds();
        }

        private void OpenDetails(ChannelData param)
        {
            /*new ChannelFeedsPage()
            {
                DataContext = new FeedDetailsViewModel(param)
            };*/
            //PhoneApplicationService.Current.State["chan"] = param;
            //NavigationService.Navigate(new Uri("/View/ChannelFeedsPage.xaml", UriKind.Relative));
            PhoneApplicationService.Current.State["Chan"] = param;
            FeedsManager.LoadFeedItems(param);
            //PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
            //bool success = frame.Navigate(new Uri("/View/ChannelFeedsPage.xaml", UriKind.Relative));

        }

        void FeedsManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Channels")
            {
                Channels = (sender as FeedDataModel).Channels;
            }
            else if (e.PropertyName == "Items")
            {
                Items = (sender as FeedDataModel).Items;
                PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
                PhoneApplicationService.Current.State["Items"] = Items;
                bool success = frame.Navigate(new Uri("/View/ChannelFeedsPage.xaml", UriKind.Relative));
            }
            else if (e.PropertyName == "AllChannels")
                AllChannels = (sender as FeedDataModel).AllChannels;
        }

        private void AddFeedBody(string url)
        {
            FeedsManager.AddNewFeed(url);
        }
    }
}
