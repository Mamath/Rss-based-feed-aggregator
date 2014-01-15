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
    class FeedDetailsDataModel : BindableObject
    {
        private FeedClient feedsClient = null;
        private FeedClient FeedsClient
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
        public ChannelData RootChannel { get; set; }

        private List<ItemData> items;

        public List<ItemData> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChange("Items"); }
        }

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

        void FeedsClient_ReadItemCompleted(object sender, ReadItemCompletedEventArgs e)
        {

        }


        void FeedsClient_GetFeedItemsCompleted(object sender, GetItemCompletedEventArgs e)
        {
            if (!ErrorModel.Instance.EvalResponse(e)) return;
            if (!ErrorModel.Instance.EvalWebResult(e.Result)) return;
            Items = e.Result._val.ToList();
        }

        public void ReadItem(ItemData item)
        {
            FeedsClient.ReadItemAsync(UserData.GetConnectionString(), item);
            item.Read = true;
        }

    }
}
