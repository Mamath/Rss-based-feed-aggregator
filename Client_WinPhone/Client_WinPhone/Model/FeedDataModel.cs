using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client_WinPhone.ServFeed;
using System.ServiceModel;
using Client_WinPhone.Utils;

namespace Client_WinPhone.Model
{
    public class FeedDataModel : BindableObject
    {
        #region Common
        static private FeedClient feedsClient = null;
        static private FeedClient FeedsClient
        {
            get
            {
                if (feedsClient == null)
                    feedsClient = new FeedClient();
                if (feedsClient.State == CommunicationState.Closed)
                    feedsClient.OpenAsync();
                return feedsClient;
            }
        }
        #endregion

        #region PPties
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
        #endregion

        #region CTor
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
                    FeedsClient.GetAllFeedsAsync();
            }
            catch (Exception)
            {

                // HACK BIZARRE BUG DE BLEND
            }

        }
        #endregion

        #region OnEndAction
        void FeedsClient_RefreshFeedCompleted(object sender, UpdateCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result._error == Resultat.ErrorCode.SUCCESS)
                {
                    GetAllRootFeeds();
                }
            }
        }

        void feedsClient_GetAllFeedsCompleted(object sender, GetAllFeedsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result._error == Resultat.ErrorCode.SUCCESS)
                {
                    AllChannels = e.Result._val.ToList();
                }
            }
        }

        void FeedsClient_AddNewFeedCompleted(object sender, AddFeedCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result._error == Resultat.ErrorCode.SUCCESS)
                {
                    GetAllRootFeeds();
                }
            }
        }

        void FeedsClient_UnfollowFeedCompleted(object sender, DeleteFeedCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result._error == Resultat.ErrorCode.SUCCESS)
                {
                    GetAllRootFeeds();
                }
            }
        }

        void FeedsClient_GetFeedsCompleted(object sender, GetFeedsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result._error == Resultat.ErrorCode.SUCCESS)
                {
                    Channels = e.Result._val.ToList();
                }
            }
        }

        void FeedsClient_GetFeedItemsCompleted(object sender, GetItemCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result._error == Resultat.ErrorCode.SUCCESS)
                {
                    Items = e.Result._val.ToList();
                }
            }
        }
        #endregion

        #region Action
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
        #endregion


        public void LoadFeedItems(ChannelData rssFeed)
        {
            FeedsClient.GetItemAsync(rssFeed, UserData.GetConnectionString());
        }
    }

    public class FeedDetailsDataModel : BindableObject
    {

        #region Common
        private FeedClient feedsClient = null;
        private FeedClient FeedsClient
        {
            get
            {
                if (feedsClient == null)
                    feedsClient = new FeedClient();
                if (feedsClient.State == CommunicationState.Closed)
                    feedsClient.OpenAsync();
                return feedsClient;
            }
        }
        #endregion

        #region PPties
        private UserDataModel UserData { get; set; }
        public ChannelData RootChannel { get; set; }

        private List<ItemData> items;

        public List<ItemData> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChange("Items"); }
        }
        #endregion

        public FeedDetailsDataModel()
        {
            UserData = UserDataModel.Instance;

            FeedsClient.ReadItemCompleted += new EventHandler<ReadItemCompletedEventArgs>(FeedsClient_ReadItemCompleted);
        }

        public FeedDetailsDataModel(ChannelData rootChan)
        {
            if (rootChan == null)
                throw new NullReferenceException();

            UserData = UserDataModel.Instance;

            RootChannel = rootChan;
            FeedsClient.GetItemCompleted += new EventHandler<GetItemCompletedEventArgs>(FeedsClient_GetFeedItemsCompleted);
            FeedsClient.GetItemAsync(rootChan, UserData.GetConnectionString());
            FeedsClient.ReadItemCompleted += new EventHandler<ReadItemCompletedEventArgs>(FeedsClient_ReadItemCompleted);
        }

        #region db result
        void FeedsClient_ReadItemCompleted(object sender, ReadItemCompletedEventArgs e)
        {

        }

        void FeedsClient_GetFeedItemsCompleted(object sender, GetItemCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result._error == Resultat.ErrorCode.SUCCESS)
                {
                    Items = e.Result._val.ToList();
                }
            }
        }
        #endregion

        #region action

        public void LoadItem(ChannelData channel)
        {
            FeedsClient.GetItemAsync(channel, UserData.GetConnectionString());
        }

        public void ReadItem(ItemData item)
        {
            FeedsClient.ReadItemAsync(UserData.GetConnectionString(), item);
            item.Read = true;
        }
        #endregion
    }
}