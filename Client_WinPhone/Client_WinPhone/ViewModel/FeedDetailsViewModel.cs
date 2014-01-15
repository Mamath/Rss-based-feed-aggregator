using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Client_WinPhone.Model;
using System.ComponentModel;
using Client_WinPhone.Utils;
using Client_WinPhone.View;
using System.Windows;
using System.Windows.Navigation;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using Client_WinPhone.ServFeed;
using Microsoft.Phone.Shell;

namespace Client_WinPhone.ViewModel
{
    public class FeedDetailsViewModel : BindableObject
    {
        #region Fields
        private List<ItemData> _items;
        private FeedDetailsDataModel feedDetailsDataModel;
        public ICommand LogOut { get; private set; }
        #endregion

        #region Properties
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
        #endregion

        public FeedDetailsViewModel()
        {
            ChannelData chan = PhoneApplicationService.Current.State["Chan"] as ChannelData;
            RootChannel = chan;
            feedDetailsDataModel = new FeedDetailsDataModel(chan);
            feedDetailsDataModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(feedDetailsDataModel_PropertyChanged);
            ReadItem = new RelayCommand((param) => ReadItemBody(param as ItemData));

            Items = PhoneApplicationService.Current.State["Items"] as List<ItemData>;

            LogOut = new RelayCommand((param) => Logout(param as string[]));
            /*RootChannel = new ChannelData();
            RootChannel.Title = "Titre du channel";
            RootChannel.Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam sit amet eleifend ante." +
                                        "In vel mauris metus, ac viverra lectus. Aenean dui sapien, pretium eu gravida ut, sollicitudin" +
                                        "et nisi. In hac habitasse platea dictumst. Quisque egestas ligula in lorem sodales sed" +
                                        " congue turpis varius. Maecenas vel quam at tortor viverra tristique vitae at lorem. Maecenas" +
                                        " augue augue, convallis tristique congue ut, porta sed felis. Nam nisi libero, vehicula" +
                                        " ac ultricies quis, imperdiet lobortis tellus.";
            RootChannel.Link = "http://www.google.com";

            Items = new List<ItemData>();
            for (int i = 0; i < 20; ++i)
            {
                ItemData item = new ItemData();
                item.Title = "Titre de l'item " + i.ToString();
                item.Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam sit amet eleifend ante." +
                                    "In vel mauris metus, ac viverra lectus. Aenean dui sapien, pretium eu gravida ut, sollicitudin" +
                                    "et nisi. In hac habitasse platea dictumst. Quisque egestas ligula in lorem sodales sed" +
                                    " congue turpis varius. Maecenas vel quam at tortor viverra tristique vitae at lorem. Maecenas" +
                                    " augue augue, convallis tristique congue ut, porta sed felis. Nam nisi libero, vehicula" +
                                    " ac ultricies quis, imperdiet lobortis tellus.";
                item.PubDate = DateTime.Now;
                Items.Add(item);

            }

            ReadItem = new RelayCommand((param) => ReadItemBody(param as ItemData));
            LogOut = new RelayCommand((param) => Logout(param as string[]));
            //feedDetailsDataModel = new FeedDetailsDataModel();*/

        }

        public FeedDetailsViewModel(ChannelData channel)
        {
            RootChannel = channel;
            feedDetailsDataModel = new FeedDetailsDataModel(channel);
            feedDetailsDataModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(feedDetailsDataModel_PropertyChanged);
            Items = feedDetailsDataModel.Items;

            ReadItem = new RelayCommand((param) => ReadItemBody(param as ItemData));
            LogOut = new RelayCommand((param) => Logout(param as string[]));
        }

        private void ReadItemBody(ItemData item)
        {

            feedDetailsDataModel.ReadItem(item);
            PhoneApplicationService.Current.State["Item"] = item;
            PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
            bool success = frame.Navigate(new Uri("/View/ViewItem.xaml", UriKind.Relative));
        }

        void feedDetailsDataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Items")
            {
                Items = (sender as FeedDetailsDataModel).Items;
            }
        }

        private void Logout(string[] param)
        {
            UserDataModel.Instance.Logout();
            PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
            bool success = frame.Navigate(new Uri("/View/LogInPage.xaml", UriKind.Relative));
        }
    }
}
