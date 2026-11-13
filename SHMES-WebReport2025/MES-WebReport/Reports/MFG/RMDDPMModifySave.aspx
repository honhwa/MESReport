<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMDDPMModifySave.aspx.cs" Inherits="Report.Reports.MFG.RMDDPMModifySave" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>生產日報人力工時修改功</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    
</head>
<body style="background-image: url(../../Images/right_bg.jpg)">
    <form id="form1" runat="server">
    <div>
        <table style="width: 912px ; text-align: center">
            <tr>
                <td style="height: 91px">
                    <div style="width: 897px; height: 42px">
                        生產日報人力工時修改功能</div>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <asp:Label ID="lblErrMsg" runat="server" ForeColor="#FF6633"></asp:Label><br />
        <table style="width: 946px; text-align: right">
            <tr>
                <td style="height: 20px; text-align: right; width: 139px;">
                        日期</td>
                <td style="height: 20px; text-align: right; width: 341px;">
                     <asp:TextBox ID="txtReportDate" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
                <td style="width: 261px; height: 20px; text-align: right">
                        機種</td>
                <td style="width: 359px; height: 20px; text-align: right">
                    <asp:TextBox ID="txtprodid" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="height: 25px; text-align: right; width: 139px;">
                        班別型態</td>
                <td style="width: 341px; height: 25px;">
                    <div id="prodIdDropDownList">
                        <asp:TextBox ID="txtShiftID" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox></div>
                </td>
                <td style="width: 261px; height: 25px;; text-align: right">
                    Team ID</td>
                <td style="width: 359px; height: 25px;">
                    <asp:TextBox ID="txtTeamID" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="height: 21px; width: 139px;">
                        User ID</td>
                <td style="height: 21px; width: 341px;">
                    <div id="partIdList">
                        <asp:TextBox ID="txtuserid" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox></div>
                </td>
                <td style="width: 261px; height: 21px">
                    Team Mode</td>
                <td style="width: 359px; height: 21px">
                    <asp:TextBox ID="txtTeamMode" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="height: 4px; width: 139px;">
                    產線</td>
                <td style="height: 4px; width: 341px;">
                    <div id="subLineList">
                        <asp:TextBox ID="txtSubLineID" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox></div>
                </td>
                <td style="width: 261px; height: 4px">
                        人力</td>
                <td style="width: 359px; height: 4px">
                    <asp:TextBox ID="txtMANPOWER" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="height: 24px; width: 139px;">
                        Lot Type</td>
                <td style="width: 341px; height: 24px">
                    <asp:TextBox ID="txtLotType" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox></td>
                <td style="width: 261px; height: 24px">
                            工時</td>
                <td style="width: 359px; height: 24px">
                    <asp:TextBox ID="txtWORKINGHOUR" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
            </tr>
        </table>
        <br />
    
    </div>
        <table style="width: 917px;text-align: center">
            <tr>
                <td style="height: 53px">
                    <asp:Button ID="btnSave" runat="server" Height="37px" Text="保存" Width="173px" OnClick="btnSave_Click" /></td>
                <td style="height: 53px">
                    <asp:Button ID="btnCancel" runat="server" Height="38px" Text="取消" Width="171px" OnClick="btnCancel_Click"   /></td>
                <td style="height: 53px">
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
