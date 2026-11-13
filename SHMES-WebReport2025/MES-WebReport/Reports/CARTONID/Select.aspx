<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Select.aspx.cs" Inherits="Report.Reports.CARTONID.Select" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CARTONID查詢</title>
    <script src="popcalendar.js" type="text/javascript"></script>
    <link href="Pager.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .aa
        {
            border-radius: 5px;
        }
        .bb
        {
            border-radius: 5px;
        }
        .cc
        {
            border-radius: 4px;
        }
        .dd
        {
            border-radius: 4px;
        }
        .ee
        {
            border-radius: 5px;
        }
        .ff
        {
            font-weight: 700;//加粗
            border-radius: 4px;
        }
        .gg
        {
            font-weight: 700;//加粗
            border-radius: 4px;
        }
        .hh
        {
            font-weight: 700;//加粗
            border-radius: 4px;
        }
        table.ii
        {
            border-collapse: collapse;
            border: solid 3px #507CD1;
        }
    </style>
</head>
<body background="../../Images/right_bg.jpg" onload="remove_loading();">
    <form id="form1" runat="server">
    <div style="text-align: center">
        <table id="Table1" align="center" border="0" width="270px" style="margin-bottom:8px">
            <tr>
                <td align="center" colspan="2" style="height: 28px">
                    <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="DodgerBlue"
                        Height="25px" Width="270px" Font-Bold="True">CARTONID SELECT</asp:Label>
                </td>
            </tr>
            <tr style="width: 270px;">
                <td width="120px">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"
                        Height="16px" Width="80px">查詢條件:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                        Width="180px" CssClass="aa" Height="28px">
                        <asp:ListItem>PANELID查箱號</asp:ListItem>
                        <asp:ListItem>箱號查PANELID</asp:ListItem>
                        <asp:ListItem>PANELID查生產記錄</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="width: 200px;">
                <td>
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"
                        Height="16px" Width="60px">Customer:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList2" runat="server" CssClass="bb" Width="180px" 
                        Height="28px" AutoPostBack="True" 
                        onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                        <asp:ListItem>BMW</asp:ListItem>
                        <asp:ListItem>Audi</asp:ListItem>
                        <asp:ListItem>Panasonic</asp:ListItem>
                        <asp:ListItem>Automotine</asp:ListItem>
                        <asp:ListItem>ALL</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"
                        Height="16px" Width="80px">From Time:</asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="txtStartDT" CssClass="cc" runat="server" Width="135px" Height="20px"></asp:TextBox>
                    <input id="Button11" class="CalendarBotton" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)"
                        style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                        background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                        width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px" type="button" />
                </td>
            </tr>
            <tr>
                <td style="width: 80px">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"
                        Height="16px" Width="100px">To Time:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEndDT" CssClass="dd" runat="server" Width="135px" Height="20px"></asp:TextBox>
                    <input id="Button22" class="CalendarBotton" onclick="popUpCalendar(this, txtEndDT, dateFormat,-1,-1,false)"
                        style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                        background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                        width: 25px; padding-top: 0px; height: 17px; border-right-width: 0px" type="button" />
                </td>
            </tr>
            <tr>
                <td style="height: 22px">
                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"
                        Height="16px" Width="130px">CartId or PanelId:</asp:Label>
                </td>
                <td style="height: 12px">
                    <asp:TextBox ID="TxtPanelId" CssClass="ee" runat="server" TextMode="MultiLine" Width="180px"
                        Height="40px"></asp:TextBox>
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
    <div id="Div1" align="center" runat="server">
        <table>
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server" Height="232px" Width="548px" CellPadding="0"
                        ForeColor="#333333" HorizontalAlign="Center" UseAccessibleHeader="False" CssClass="ii">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="red" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table>
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" CssClass="anpager" CurrentPageButtonClass="cpb"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页" SubmitButtonText="Go"
                TextAfterPageIndexBox="页" TextBeforePageIndexBox="转到" OnPageChanged="AspNetPager1_PageChanged"
                PageSize="30">
            </webdiyer:AspNetPager>
        </table>
    </div>
    </form>
</body>
</html>
