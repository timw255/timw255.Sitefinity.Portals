<%@ Control Language="C#" %>
<%@ Register TagPrefix="sitefinity" Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI" %>
<%@ Register TagPrefix="Portals" Assembly="Portals" Namespace="timw255.Sitefinity.Portals.Web.UI.UserDashboardDatas" %>
<%@ Import Namespace="timw255.Sitefinity.Portals" %>

<h1 class="sfBreadCrumb">
    <asp:Literal ID="sampleModule2" runat="server" Text='Portals'></asp:Literal>
</h1>
<div class="sfMain sfClearfix">
    <div class="sfContent sfWorkArea">
        <h2><asp:Literal ID="message" runat="server" Text='<%$Resources:PortalsResources, UserDashboardDataEmptyPageMessage %>'></asp:Literal></h2>
    </div>
</div>