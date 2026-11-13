<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl1.ascx.cs" Inherits="OEE_REPORT.WebUserControl1" %>
<div id="div1" runat ="server" >
<div id="div2" runat ="server"  style="top:50">
<asp:TextBox ID="TextBox1" runat="server" Enabled="False" Width="83px" 
    ></asp:TextBox>
<asp:ImageButton ID="ibtndate" runat="server" ImageUrl="~/Images/Button.gif" OnClick="ibtndate_Click" />
</div>
<div id="div3" runat ="server" style="left:10;top:60">
<asp:Calendar ID="caldate" runat="server" BackColor="White" BorderColor="#999999"
    CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
    ForeColor="Black" Height="1px" Visible="False" Width="43px" OnSelectionChanged="caldate_SelectionChanged">
    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
    <SelectorStyle BackColor="#CCCCCC" />
    <WeekendDayStyle BackColor="#FFFFCC" />
    <OtherMonthDayStyle ForeColor="Gray" />
    <NextPrevStyle VerticalAlign="Bottom" />
    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
    <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
</asp:Calendar></div>
&nbsp;</div>