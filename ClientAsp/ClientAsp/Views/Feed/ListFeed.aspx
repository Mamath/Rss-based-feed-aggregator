<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Liste des Flux
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Liste des Flux</h2>


            <div class="row well">
                <div class="col-md-3">Feed</div>
                <div class="col-md-4">URL</div>
                <div class="col-md-4">Description</div>
                <div class="col-md-1"></div>
            </div>
            <% foreach (var channel in (ViewData["Feeds"] as ClientAsp.ServFeed.ChannelData[])){ %>
        <div class="row well">
                    <div class="col-md-3"> <%: Html.ActionLink(channel.Title, "Feed", "Feed", channel, null) %> </div>
                    <div class="col-md-4"> <a href="<%: channel.Link %>" target="blank"><%: channel.Link %></a> </div>
                    <div class="col-md-4"> <%: channel.Description %> </div>
                    <div class="col-md-1"> <%: Html.ActionLink("Unfollow", "Delete", "Feed", channel, null) %> </div>
                </div>
            <% } %>

</asp:Content>
