<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateTypeControl.ascx.cs" Inherits="WebUserControl_MonthControl" %>
<script type="text/javascript" src=""></script>
<script type="text/javascript" language="javascript" src="../jsDateTime/popcalendar.js"></script>
<script type="text/javascript" language="JavaScript">    dateFormat = 'yyyy-ww'</script>

<asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Images/OptionsDateType.jpg" HorizontalAlign="Center" BorderStyle="None" Height="233px" Width="196px">
    <asp:Panel ID="Panel3" runat="server" Height="10px"></asp:Panel>
    <asp:TextBox ID="tbFormat" runat="server" Visible="False" Width="42px"></asp:TextBox>
    <asp:Label ID="lblDateType" runat="server" Text="Date" 
        Font-Names="Microsoft Sans Serif" Font-Size="10pt" Height="20px"></asp:Label>
    <asp:Panel ID="Panel7" runat="server" Height="15px"></asp:Panel>
    <asp:Panel ID="Panel8" runat="server" Height="20px" Font-Names="Microsoft Sans Serif" Font-Size="12px">
                <asp:RadioButtonList ID="rblDateType" runat="server" 
                    RepeatDirection="Horizontal" RepeatLayout="Flow" 
                    onselectedindexchanged="rblDateType_SelectedIndexChanged" 
                    AutoPostBack="True">
                    <asp:ListItem Value="Date" Selected="True">Date</asp:ListItem>
                    <asp:ListItem Value="Week">Week</asp:ListItem>
                    <asp:ListItem Value="Month">Month</asp:ListItem>
                </asp:RadioButtonList>
    </asp:Panel>
    <asp:Panel ID="Panel9" runat="server" Height="20px"></asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Height="20px" HorizontalAlign="Left">
        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="From:" 
            Font-Names="Microsoft Sans Serif" Font-Size="10pt" Width="120px"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="Panel" runat="server" Height="30px">
        <asp:DropDownList ID="ddlFrom" runat="server" Font-Names="Microsoft Sans Serif" 
            Font-Size="10pt" Width="160px">
        </asp:DropDownList>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Height="20px" HorizontalAlign="Left">
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" Text="To:" 
            Font-Names="Microsoft Sans Serif" Font-Size="10pt"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" Height="30px">
        <asp:DropDownList ID="ddlTo" runat="server" Font-Names="Microsoft Sans Serif" 
            Font-Size="10pt" Width="160px">
        </asp:DropDownList>
    </asp:Panel>
</asp:Panel>
