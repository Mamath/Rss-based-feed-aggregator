<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ClientAsp.Models.AddFeedModel>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AddFeed
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>AddFeed</h2>

    <p>
        <%: ViewData["LogOnError"] %>
    </p>
    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Échec de la connexion. Corrigez les erreurs et réessayez.") %>
        <div>
            <fieldset>
                <legend>Informations de compte</legend>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.FeedLink) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.FeedLink) %>
                    <%: Html.ValidationMessageFor(m => m.FeedLink) %>
                </div>
                
                <p>
                    <input type="submit" value="Ajouter un flux" />
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
