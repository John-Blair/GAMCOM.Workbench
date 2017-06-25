<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisclaimerButtons.ascx.cs" Inherits="UmbracoWorkbench.usercontrols.DisclaimerButtons" %>

<form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span style="margin-right: 20px;" class="G_Highlight linkButtonContainer <umbraco:Item field='buttonStyle' runat='server' />">
                <asp:LinkButton ID="dsclmrBtn_Decline" runat="server" OnClick="dsclmrBtn_Decline_Click" Style="padding: 4px; width: 150px;" CssClass="linkButton"><umbraco:Item field="#I Decline" runat="server" /></asp:LinkButton>
            </span>
            <span style="margin-right: 20px;" class="G_Highlight linkButtonContainer <umbraco:Item field='buttonStyle' runat='server' />">
                <asp:LinkButton ID="dsclmrBtn_Accept" runat="server" OnClick="dsclmrBtn_Accept_Click" Style="padding: 4px; width: 150px;" CssClass="linkButton"><umbraco:Item field="#I Accept" runat="server" /></asp:LinkButton>
            </span>
        </ContentTemplate>
    </asp:UpdatePanel>
</form>
