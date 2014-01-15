<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" validateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Item
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Item</h2>
    <h3><%: ViewData["ItemTitle"] %></h3>

    <p>
        <%: ViewData["ItemDescription"] %>
    </p>
</asp:Content>
