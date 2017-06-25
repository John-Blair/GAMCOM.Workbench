<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListOfValuesDDL.ascx.cs" Inherits="DataEditorControls.ListOfValuesDDL" %>
  <asp:UpdatePanel ID="LOVUpdatePanel" runat="server">
    <ContentTemplate>

        <asp:DropDownList ID="LOVDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="LOVDropDownList_SelectedIndexChanged">
        </asp:DropDownList> 
        <asp:TextBox ID="rootNodeTextBox" runat="server" style="display:none"></asp:TextBox>

    </ContentTemplate>
</asp:UpdatePanel>

