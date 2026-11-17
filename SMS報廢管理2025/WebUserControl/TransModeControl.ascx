<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TransModeControl.ascx.cs" Inherits="WebUserControl_TransModeControl" %>
<style type="text/css">
    .style1
    {
        height: 15px;
    }
    .style23
    {
        Width: 100%;
        height: 100%;
    }
</style>
<asp:Panel ID="Panel1" runat="server" 
    BackImageUrl="~/Images/OptionsBackImage.jpg" Height="71px" Width="197px">
<table class="style23" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="style17" 
            style="font-family: 'Microsoft Sans Serif'; font-size: 12px" 
            align="center">
            <asp:Label ID="lblTitle" runat="server" Font-Names="Microsoft Sans Serif" 
                Font-Size="10pt">TransMode</asp:Label>
        </td>
    </tr>
    <tr>
        <td class="style29" 
            style="font-family: 'Microsoft Sans Serif'; font-size: 12px" 
            align="center">
            <asp:RadioButtonList ID="rblTransMode" runat="server" 
                RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Value="OnePiece">OnePiece</asp:ListItem>
                <asp:ListItem Value="ByLot">ByLot</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td style="font-family: 'Microsoft Sans Serif'; font-size: 12px" class="style1">
            </td>
    </tr>
</table>
</asp:Panel>