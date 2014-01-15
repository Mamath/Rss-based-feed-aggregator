using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using Client_WinPhone.ServFeed;
using Microsoft.Phone.Shell;

namespace Client_WinPhone.View
{
    public partial class ViewItem : PhoneApplicationPage
    {

        public ViewItem()
        {
            InitializeComponent();

            ItemData item = PhoneApplicationService.Current.State["Item"] as ItemData;

            browser.NavigateToString(item.Description);
        }

    }
}