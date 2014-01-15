using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientWPF.FeedService;
using System.ServiceModel;
using ClientWPF.Model;
using System.ComponentModel;

namespace ClientWPF.Model
{
    class FeedDataModel : BindableObject
    {
        static private FeedClient feedsClient = null;
        static private FeedClient FeedsClient
        {
            get
            {
                if (feedsClient == null)
                    feedsClient = new FeedClient();
                if (feedsClient.State == CommunicationState.Closed)
                    feedsClient.Open();
                return feedsClient;
            }
        }

        private UserDataModel UserData { get; set; }

        private List<ChannelData> channels = new List<ChannelData>();
        public List<ChannelData> Channels
        {
            get { return channels; }
            private set { channels = value; RaisePropertyChange("Channels"); }
        }

        private List<ChannelData> allChannels = new List<ChannelData>();
        public List<ChannelData> AllChannels
        {
            get { return allChannels; }
            private set { allChannels = value; RaisePropertyChange("AllChannels"); }
        }

        private List<ItemData> item = new List<ItemData>();
        public List<ItemData> Items
        {
            get { return item; }
            private set { item = value; RaisePropertyChange("Items"); }
        }

        private static FeedDataModel instance = new FeedDataModel();
        public static FeedDataModel Instance { get { return instance; } }

        private FeedDataModel()
        {
            try
            {
                UserData = UserDataModel.Instance;

                FeedsClient.GetFeedsCompleted += new EventHandler<GetFeedsCompletedEventArgs>(FeedsClient_GetFeedsCompleted);
                FeedsClient.AddFeedCompleted += new EventHandler<AddFeedCompletedEventArgs>(FeedsClient_AddNewFeedCompleted);
                FeedsClient.DeleteFeedCompleted += new EventHandler<DeleteFeedCompletedEventArgs>(FeedsClient_UnfollowFeedCompleted);
                FeedsClient.GetItemCompleted += new EventHandler<GetItemCompletedEventArgs>(FeedsClient_GetFeedItemsCompleted);
                feedsClient.GetAllFeedsCompleted += new EventHandler<GetAllFeedsCompletedEventArgs>(feedsClient_GetAllFeedsCompleted);
                FeedsClient.UpdateCompleted += new EventHandler<UpdateCompletedEventArgs>(FeedsClient_RefreshFeedCompleted);

                if (UserData.IsConnected)
                {
                    GetAllRootFeeds();
                }
            }
            catch (Exception)
            {
                
                // HACK BIZARRE BUG DE BLEND
            }
            
        }

        void FeedsClient_RefreshFeedCompleted(object sender, UpdateCompletedEventArgs e)
        {
            if (!ErrorModel.Instance.EvalResponse(e)) return;
            if (!ErrorModel.Instance.EvalWebResult(e.Result)) return;
            GetAllRootFeeds();
        }

        void feedsClient_GetAllFeedsCompleted(object sender, GetAllFeedsCompletedEventArgs e)
        {
            if (!ErrorModel.Instance.EvalResponse(e)) return;
            if (!ErrorModel.Instance.EvalWebResult(e.Result)) return;
            AllChannels = e.Result._val.ToList();
        }

        void FeedsClient_AddNewFeedCompleted(object sender, AddFeedCompletedEventArgs e)
        {
            if (!ErrorModel.Instance.EvalResponse(e)) return;
            if (!ErrorModel.Instance.EvalWebResult(e.Result)) return;
            GetAllRootFeeds();
        }

        void FeedsClient_UnfollowFeedCompleted(object sender, DeleteFeedCompletedEventArgs e)
        {
            if (!ErrorModel.Instance.EvalResponse(e)) return;
            if (!ErrorModel.Instance.EvalWebResult(e.Result)) return;
            GetAllRootFeeds();
        }

        void FeedsClient_GetFeedsCompleted(object sender, GetFeedsCompletedEventArgs e)
        {
            if (!ErrorModel.Instance.EvalResponse(e)) return;
            if (!ErrorModel.Instance.EvalWebResult(e.Result)) return;
            Channels = e.Result._val.ToList();
        }

        void FeedsClient_GetFeedItemsCompleted(object sender, GetItemCompletedEventArgs e)
        {
            if (!ErrorModel.Instance.EvalResponse(e)) return;
            if (!ErrorModel.Instance.EvalWebResult(e.Result)) return;
            Items = e.Result._val.ToList();
        }

        public void GetAllRootFeeds()
        {
            FeedsClient.GetFeedsAsync(UserData.GetConnectionString());
            FeedsClient.GetAllFeedsAsync();
        }

        public void AddNewFeed(string url)
        {
            try
            {
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    FeedsClient.AddFeedAsync(UserData.GetConnectionString(), new Uri(url));
            }
            catch (Exception)
            {
                
            }
            
        }

        public void RemoveFeed(ChannelData feed)
        {
            FeedsClient.DeleteFeedAsync(UserData.GetConnectionString(), feed);
        }

        public void RefreshFeed(ChannelData feed)
        {
            FeedsClient.UpdateAsync(feed);
        }


        public void LoadFeedItems(ChannelData rssFeed)
        {
            FeedsClient.GetItemAsync(rssFeed, UserData.GetConnectionString());
        }
    }
}
