<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
    <div class="row">
    <ul class="nav navbar-nav">
    <%
        if (Request.IsAuthenticated) {
    %>
      <li><a><%: Page.User.Identity.Name %></a></li>
      <li><%: Html.ActionLink("Fermer la session", "LogOff", "Account") %></li>
      <li><%: Html.ActionLink("Supprimer ce compte", "Delete", "Account") %></li>
      <li><%: Html.ActionLink("changer de mot de passe", "ChangePassword", "Account") %></li>
<%
    }
    else {
%> 
      <li><%: Html.ActionLink("Ouvrir une session", "LogOn", "Account") %></li>
<%
    }
%>
    </ul>
</div>