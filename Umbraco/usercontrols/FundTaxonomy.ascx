<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FundTaxonomy.ascx.cs" Inherits="UmbracoWorkbench.usercontrols.FundTaxonomy" %>

    <asp:PlaceHolder runat="server" id="fundHolder">

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
            <asp:Label runat ="server" Text="Filter:" width="50"/>
            <asp:TextBox runat="server" id="FilterText" width="230" OnTextChanged="filtertext_TextChanged" AutoPostBack="true"/>
            <asp:Button runat="server" Text="Clear" id="clear" OnClick="clear_Click" />
            <asp:Button runat="server" Text="Show Selected Only" id="showSelected" OnClick="showSelected_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <hr />

    <asp:UpdatePanel runat="server" id="updatePanel" ChildrenAsTriggers="true" />

    </asp:PlaceHolder>