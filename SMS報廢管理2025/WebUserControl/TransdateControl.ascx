<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TransdateControl.ascx.cs" Inherits="WebUserControl_TransdateControl" %>
<style type="text/css">
    .style3
    {
        height: 27px;
    }
</style>
<script type="text/javascript" src=""></script>
<script type="text/javascript" language="javascript" src="../jsDateTime/popcalendar.js"></script>
<script type="text/javascript" language="JavaScript">dateFormat = 'yyyy-mm-dd'</script>

<asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Images/OptionsDate.jpg" HorizontalAlign="Center"
    BorderStyle="None" Height="155px" Width="198px">
    <asp:Panel ID="Panel3" runat="server" Height="10px"></asp:Panel>
    <asp:Label ID="lblDate" runat="server" Text="TransDate:" Font-Names="Microsoft Sans Serif" Font-Size="10pt"></asp:Label>
    <asp:Panel ID="Panel2" runat="server" Height="20px" HorizontalAlign="Left">
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server" Text="FromTime:" 
            Font-Names="Microsoft Sans Serif" Font-Size="10pt" Width="120px"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="Panel6" runat="server" Height="30px">
        <asp:TextBox ID="tbFromTime" runat="server" Height="20px" MaxLength="10" Width="160px" onclick="popUpCalendar(this,this, dateFormat,-1,-1,true)"></asp:TextBox>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Height="20px" HorizontalAlign="Left">
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" Text="ToTime:" Font-Names="Microsoft Sans Serif" Font-Size="10pt"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" Height="30px">
        <asp:TextBox ID="tbToTime" runat="server" Height="20px" MaxLength="10" Width="160px" onclick="popUpCalendar(this,this, dateFormat,-1,-1,true)"></asp:TextBox>
    </asp:Panel>
</asp:Panel>
