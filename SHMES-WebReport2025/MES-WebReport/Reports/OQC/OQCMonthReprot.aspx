<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OQCMonthReprot.aspx.cs" Inherits="Report.Reports.OQC.OQCMonthReprot" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div>
        <div>
            <div>
                <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                    margin: 0px; width: 103%; padding-top: 0px; height: 1064px">
                    <tr>
                        <td style="background-position-x: right; background-attachment: scroll; background-image: url(../Images/left_bg.jpg);
                            vertical-align: top; width: 176px; background-repeat: repeat-y">
                        </td>
                        <td style="background-position-x: left; background-image: url(../Images/right_bg.jpg);
                            width: 15px; background-repeat: repeat-y">
                        </td>
                        <td rowspan="2" style="background-position-x: center; background-image: url(../Images/right_bg.jpg);
                            vertical-align: top; width: 448px; background-repeat: repeat-y">
                            <div id="filter">
                                <table style="vertical-align: text-bottom; width: 749px; height: 68px; text-align: center">
                                    <tr>
                                        <td colspan="11" rowspan="1" style="font-size: 24px; color: black; height: 13px">
                                            OQC月報表</td>
                                    </tr>
                                    <tr>
                                        <td rowspan="2" style="width: 74px; height: 13px">
                                            群組</td>
                                        <td rowspan="2" style="width: 109px; height: 13px; text-align: left">
                                            <asp:DropDownList ID="DlGroup" runat="server" Width="102px">
                                            </asp:DropDownList></td>
                                        <td colspan="1" rowspan="2" style="width: 46px; height: 13px; text-align: center">
                                            年份</td>
                                        <td colspan="1" rowspan="2" style="width: 60px; height: 13px; text-align: center">
                                            <asp:DropDownList ID="dlYear" runat="server" Width="64px">
                                                <asp:ListItem>2008</asp:ListItem>
                                                <asp:ListItem>2009</asp:ListItem>
                                                <asp:ListItem>2010</asp:ListItem>
                                                <asp:ListItem>2011</asp:ListItem>
                                                <asp:ListItem>2012</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td colspan="1" rowspan="2" style="width: 51px; height: 13px; text-align: center">
                                            月份</td>
                                        <td colspan="1" rowspan="2" style="width: 69px; height: 13px; text-align: center">
                                            <asp:DropDownList ID="dlMonth" runat="server" Width="64px">
                                                <asp:ListItem>01</asp:ListItem>
                                                <asp:ListItem>02</asp:ListItem>
                                                <asp:ListItem>03</asp:ListItem>
                                                <asp:ListItem>04</asp:ListItem>
                                                <asp:ListItem>05</asp:ListItem>
                                                <asp:ListItem>06</asp:ListItem>
                                                <asp:ListItem>07</asp:ListItem>
                                                <asp:ListItem>08</asp:ListItem>
                                                <asp:ListItem>09</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td colspan="5" rowspan="2" style="height: 13px; text-align: left">
                                            <asp:Button ID="btnSerch" runat="server" OnClick="btnSerch_Click" Text="查詢" /></td>
                                    </tr>
                                    <tr>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 749px; height: 343px">
                                <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label></div>
                            <div style="width: 747px; height: 372px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-position-x: right; background-attachment: scroll; background-image: url(../Images/left_bg.jpg);
                            vertical-align: top; width: 176px; background-repeat: repeat-y; height: 965px">
                        </td>
                        <td rowspan="1" style="width: 15px;">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    
    </div>
    </form>
</body>
</html>
