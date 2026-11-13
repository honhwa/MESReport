<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMASelect.aspx.cs" Inherits="WebReport.Reports.RMA.RMASelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>客退品復判系統信息查詢報表</title>
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">        dateFormat = 'yyyy-mm-dd'</script>
    <%--<style type="text/css">
        .style1
        {
            width: 80px;
            height: 36px;
        }
        .style2
        {
            height: 36px;
        }
    </style>--%>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div style="text-align: center">
        <table id="Table1" align="center" border="0" style="font-size: medium">
            <tr>
                <td align="center" colspan="2" style="height: 13px">
                    <asp:Label ID="Label9" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="DodgerBlue"
                        Height="36px" Width="280px" Font-Bold="True">GRMA Data Query</asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    &nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                        Height="16px" Width="88px">From Time:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtStartDT" runat="server" Width="150px" Height="20px"></asp:TextBox>
                    <input id="Button11" class="CalendarBotton" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)"
                        style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                        background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                        width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px" type="button" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    &nbsp;<asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                        Height="16px" Width="80px">To Time:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtEndDT" runat="server" Width="150px" Height="20px"></asp:TextBox>
                    <input id="Button22" class="CalendarBotton" onclick="popUpCalendar(this, TxtEndDT, dateFormat,-1,-1,false)"
                        style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                        background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                        width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px" type="button" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" 
                        Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                        Height="16px" Width="112px">SN or PanelId:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLot" runat="server" Width="177px" TextMode="MultiLine" Height="36px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    &nbsp;<asp:Label ID="Label5" runat="server" 
                        Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                        Height="16px" Width="66px">ProdID：</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtProd" runat="server" TextMode="MultiLine" Width="176px" Height="36px"></asp:TextBox>
                </td>
            </tr>
           </table>
                    <asp:Button ID="btnQuery" runat="server" CssClass="ff" Text="Query" OnClick="btnQuery_Click"
                        Width="55px" BackColor="#337AB7" Height="30px" BorderColor="#2E6DA4" BorderStyle="Solid"
                        ForeColor="White" />
                    <asp:Button ID="btnClear" runat="server" CssClass="gg" Text="Clear" Width="55px"
                        Height="30px" OnClick="btnClear_Click" BackColor="#D9534F" BorderColor="#D43F3A"
                        BorderStyle="Solid" ForeColor="White" />
                    <asp:Button ID="btnExport" runat="server" CssClass="hh" Text="Export" OnClick="btnExport_Click"
                        Width="55px" Height="30px" Enabled="False" BackColor="#5CB85C" BorderColor="#4CAE4C"
                        BorderStyle="Solid" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
                        ForeColor="White" />
            <table align="center" style="margin-top:5px">
            <tr>
                <td align="left" style="height: 21px;" colspan="2">
                    <font face="新細明體">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                    </font>
                </td>
            </tr>
        </table>
    </div>
    <div id="Div1" runat="server">
        <table align="center" border="0">
            <tr>
                <td style="width: 400px" align="center">
                    <asp:GridView ID="GridView1" OnPageIndexChanging="GridView1_PageIndexChanging" 
                        runat="server" CellPadding="3" Width="400px" 
                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                        Height="2px" HorizontalAlign="Center" Font-Size="X-Small" 
                        AllowPaging="True">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        <PagerSettings FirstPageText="首頁" LastPageText="尾頁" NextPageText="下一頁" 
                            PreviousPageText="上一頁" Mode="NextPreviousFirstLast" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
