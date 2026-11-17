<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DayControl.ascx.cs" Inherits="WebUserControl_DayControl" %>
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
<script type="text/javascript" src=""></script>
<script type="text/javascript" language="javascript" src="../jsDateTime/popcalendar.js"></script>
<script type="text/javascript" language="JavaScript">    dateFormat = 'yyyy-mm-dd'</script>

<asp:Panel ID="Panel1" runat="server"
    BackImageUrl="~/Images/TextBox1.jpg" Height="67px" Width="197px">
    <table class="style23" border="0" cellpadding="0" cellspacing="0">
        <tr align="center">
            <td class="style17" 
                style="font-family: 'Microsoft Sans Serif'; font-size: 12px">
                <asp:Label ID="lblTitle" runat="server" Font-Names="Microsoft Sans Serif" 
                    Font-Size="10pt">Date</asp:Label>
            </td>
        </tr>
        <tr  align="center">
            <td class="style29" 
                style="font-family: 'Microsoft Sans Serif'; font-size: 12px" 
                align="center">
                <asp:TextBox ID="tbDate" runat="server" Height="20px" MaxLength="10" Width="160px" onclick="popUpCalendar(this,this, dateFormat,-1,-1,false)"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="font-family: 'Microsoft Sans Serif'; font-size: 12px" class="style1">
            </td>
        </tr>
</table>
</asp:Panel>