<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FactoryControl.ascx.cs" Inherits="WebUserControl_FactoryControl" %>
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
    .style24
    {
        height: 27px;
    }
</style>
<asp:Panel ID="Panel1" runat="server"
    BackImageUrl="~/Images/OptionsBackImage.jpg" Height="71px" Width="197px">
<table class="style23" border="0" cellpadding="0" cellspacing="0">
    <tr align="center">
        <td
            style="font-family: 'Microsoft Sans Serif'; font-size: 12px" 
            class="style24">
            <asp:Label ID="lblTitle" runat="server" Font-Names="Microsoft Sans Serif" 
                Font-Size="10pt">FactoryName</asp:Label>
        </td>
    </tr>
    <tr  align="center">
        <td class="style29" 
            style="font-family: 'Microsoft Sans Serif'; font-size: 12px" 
            align="center">
            <asp:RadioButtonList ID="rblFactoryName" runat="server" 
                RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Value="A" Selected="True">WD01</asp:ListItem>
                <asp:ListItem Value="B">WD02</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td style="font-family: 'Microsoft Sans Serif'; font-size: 12px" class="style1">
            </td>
    </tr>
</table>
</asp:Panel>
