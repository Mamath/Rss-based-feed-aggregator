<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Feed
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Feed</h2>

            <div class="row well">
                <div class="col-md-5">Item Feed</div>
                <div class="col-md-5">Publication Date</div>
                <div class="col-md-2">State</div>
            </div>
            <% foreach (var item in (ViewData["Items"] as ClientAsp.ServFeed.ItemData[]))
               { %>
                <div class="row well">
                    <div class="col-md-5"><%: Html.ActionLink(item.Title, "Item", "Feed", item, null) %></div>
                    <div class="col-md-5"><%: item.PubDate.DateTime %></div>
                    <div class="col-md-2"><%:(item.Read) ? "-" : "New" %></div>
                </div>
            <% } %>

</asp:Content>
