<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListOfValues.ascx.cs" Inherits="DataEditorControls.ListOfValues" %>
  <asp:UpdatePanel ID="LOVUpdatePanel" runat="server">
    <ContentTemplate>

        <asp:TextBox ID="LOVValueTextBox" runat="server" style="width:500px"></asp:TextBox><br />
        <span>Choose a Tile Theme:</span>
        <asp:DropDownList ID="LOVDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="LOVDropDownList_SelectedIndexChanged">
        </asp:DropDownList> 
        <asp:TextBox ID="rootNodeTextBox" runat="server" style="display:none"></asp:TextBox>

    </ContentTemplate>
</asp:UpdatePanel>

