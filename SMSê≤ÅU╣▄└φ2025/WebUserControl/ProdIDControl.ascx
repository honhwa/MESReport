<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProdIDControl.ascx.cs" Inherits="WebUserControl_ProdIDControl" %>
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
    BackImageUrl="~/Images/TextBox1.jpg" Height="67px" Width="197px">
<table class="style23" border="0" cellpadding="0" cellspacing="0">
    <tr align="center">
        <td class="style17" 
            style="font-family: 'Microsoft Sans Serif'; font-size: 12px">
            <asp:Label ID="lblTitle" runat="server" Font-Names="Microsoft Sans Serif" 
                Font-Size="10pt">PRODID</asp:Label>
        </td>
    </tr>
    <tr  align="center">
        <td class="style29" 
            style="font-family: 'Microsoft Sans Serif'; font-size: 12px" 
            align="center">
            <asp:DropDownList ID="ddlValue" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" Width="170px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="font-family: 'Microsoft Sans Serif'; font-size: 12px" class="style1">
            </td>
    </tr>
</table>
</asp:Panel>