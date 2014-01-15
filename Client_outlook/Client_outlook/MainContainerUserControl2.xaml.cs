using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace Client_outlook
{
    /// <summary>
    /// Logique d'interaction pour MainContainerUserControl.xaml
    /// </summary>
    public partial class MainContainerUserControl : System.Windows.Controls.UserControl
    {
        public MainContainerUserControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ServFeed.Feed sf = new ServFeed.Feed();
            ServFeed.ResultatOfArrayOfChannelDataxs063O9k feeds = sf.GetFeeds(Glob.res);
            button3.Visibility = Visibility.Hidden;
            button4.Visibility = Visibility.Hidden;
            dataGrid2.Visibility = Visibility.Hidden;
            dataGrid1.Visibility = Visibility.Visible;
            if (feeds._error == ServFeed.ResultatErrorCode.SUCCESS)
                dataGrid1.DataContext = feeds._val;
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button3.Visibility = Visibility.Visible;
            button4.Visibility = Visibility.Visible;
            ServFeed.Feed sf = new ServFeed.Feed();
            Client_outlook.ServFeed.ChannelData chanel = (Client_outlook.ServFeed.ChannelData)dataGrid1.SelectedItem;
            ServFeed.ResultatOfArrayOfItemDataxs063O9k items = sf.GetItem(chanel, Glob.res);
            if (items._error == ServFeed.ResultatErrorCode.SUCCESS)
            {
                dataGrid1.Visibility = Visibility.Hidden;
                dataGrid2.Visibility = Visibility.Visible;
                dataGrid2.DataContext = items._val;
            }
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button3.Visibility = Visibility.Visible;
            button4.Visibility = Visibility.Visible;
            Client_outlook.ServFeed.ItemData data = (Client_outlook.ServFeed.ItemData)dataGrid2.SelectedItem;
            ServFeed.Feed sf = new ServFeed.Feed(); 
            ServFeed.Resultat read = sf.ReadItem(Glob.res, data);
            if (read._error == ServFeed.ResultatErrorCode.SUCCESS)
                webBrowser1.NavigateToString(data.Description + data.Link);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            button3.Visibility = Visibility.Hidden;
            button4.Visibility = Visibility.Hidden;
            dataGrid2.Visibility = Visibility.Hidden;
            dataGrid1.Visibility = Visibility.Visible;
            ServFeed.Feed sf = new ServFeed.Feed();
            ServFeed.ResultatOfArrayOfChannelDataxs063O9k feeds = sf.GetAllFeeds();
            if (feeds._error != ServFeed.ResultatErrorCode.SUCCESS)
                System.Windows.MessageBox.Show("Cannot load feeds!");
            else
                dataGrid1.DataContext = feeds._val;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            ServFeed.Feed sf = new ServFeed.Feed();
            Client_outlook.ServFeed.ChannelData chanel = (Client_outlook.ServFeed.ChannelData)dataGrid1.SelectedItem;
            ServFeed.Resultat refresh = sf.Update(chanel);
            ServFeed.ResultatOfArrayOfItemDataxs063O9k items = sf.GetItem(chanel, Glob.res);
            if (items._error == ServFeed.ResultatErrorCode.SUCCESS)
            {
                dataGrid1.Visibility = Visibility.Hidden;
                dataGrid2.Visibility = Visibility.Visible;
                dataGrid2.DataContext = items._val;
            }
            button3.Visibility = Visibility.Hidden;
            button4.Visibility = Visibility.Hidden;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            ServFeed.Feed sf = new ServFeed.Feed();
            Client_outlook.ServFeed.ChannelData chanel = (Client_outlook.ServFeed.ChannelData)dataGrid1.SelectedItem;
            ServFeed.Resultat delete = sf.DeleteFeed(Glob.res, chanel);
            ServFeed.ResultatOfArrayOfChannelDataxs063O9k feeds = sf.GetFeeds(Glob.res);
            if (feeds._error != ServFeed.ResultatErrorCode.SUCCESS)
                System.Windows.MessageBox.Show("Cannot load feeds!");
            else
            {
                dataGrid2.Visibility = Visibility.Hidden;
                dataGrid1.Visibility = Visibility.Visible;
                dataGrid1.DataContext = feeds._val;
            }
            button4.Visibility = Visibility.Hidden;
            button3.Visibility = Visibility.Hidden;
        }
    }
}
