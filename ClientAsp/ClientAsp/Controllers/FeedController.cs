using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Configuration;
using ClientAsp.Models;

namespace ClientAsp.Controllers
{
    public class FeedController : Controller
    {
        //
        // GET: /Feed/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddFeed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFeed(AddFeedModel model)
        {
            if (ModelState.IsValid)
            {
                FormsIdentity id = User.Identity as FormsIdentity;
                FormsAuthenticationTicket ticket = id.Ticket;
                string session = ticket.UserData;
                ServFeed.Feed sf = new ServFeed.Feed();
                ServFeed.Resultat feed = sf.AddFeed(session, model.FeedLink);
                if (feed._error == ServFeed.ResultatErrorCode.SUCCESS)
                {
                    return RedirectToAction("ListFeed", "Feed");
                }
            }
            return View();
        }

        public ActionResult ListFeed()
        {
            FormsIdentity id = User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string session = ticket.UserData;
            ServFeed.Feed sf = new ServFeed.Feed();
            ServFeed. ResultatOfArrayOfChannelDataxs063O9k feeds = sf.GetFeeds(session);
            if (feeds._error != ServFeed.ResultatErrorCode.SUCCESS)
                ViewData["FeedsError"] = feeds._error.ToString();
            else
                ViewData["Feeds"] = feeds._val;
            return View();
        }

        public ActionResult AllFeed()
        {
            FormsIdentity id = User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string session = ticket.UserData;
            ServFeed.Feed sf = new ServFeed.Feed();
            ServFeed.ResultatOfArrayOfChannelDataxs063O9k feeds = sf.GetAllFeeds();
            if (feeds._error != ServFeed.ResultatErrorCode.SUCCESS)
                ViewData["FeedsError"] = feeds._error.ToString();
            else
                ViewData["AllFeeds"] = feeds._val;
            return View();

        }

        public ActionResult Feed(ServFeed.ChannelData channel)
        {
            Refresh(channel);
            FormsIdentity id = User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string session = ticket.UserData;
            ServFeed.Feed sf = new ServFeed.Feed();
            ServFeed.ResultatOfArrayOfItemDataxs063O9k items = sf.GetItem(channel, session);

            if (items._error == ServFeed.ResultatErrorCode.SUCCESS)
            {
               // ServFeed.ItemData[] _items = items._val;
                ViewData["Items"] = items._val;
            }
            return View();
        }

        
        [ValidateInput(false)]
        public ActionResult Item(ServFeed.ItemData item)
        {
            FormsIdentity id = User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string session = ticket.UserData;
            ServFeed.Feed sf = new ServFeed.Feed();
            ServFeed.Resultat read = sf.ReadItem(session, item);
            ViewData["ItemTitle"] = item.Title;
            ViewData["ItemDescription"] = new HtmlString(item.Description.ToString());
            return View();

        }

        public ActionResult Delete(ServFeed.ChannelData channel)
        {
            FormsIdentity id = User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string session = ticket.UserData;
            ServFeed.Feed sf = new ServFeed.Feed();
            ServFeed.Resultat delete = sf.DeleteFeed(session, channel);

            return RedirectToAction("ListFeed", "Feed");
        }

        public void Refresh(ServFeed.ChannelData channel)
        {
            FormsIdentity id = User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string session = ticket.UserData;
            ServFeed.Feed sf = new ServFeed.Feed();
            ServFeed.Resultat delete = sf.Update(channel);

            //return RedirectToAction("ListFeed", "Feed");
        }
    }
}
