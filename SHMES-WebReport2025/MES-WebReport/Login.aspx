<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebReport.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SH MES WEB REPORT -- LOGIN</title>
    <style type="text/css">
        .p1 {
            FONT-SIZE: 14px;
            LINE-HEIGHT: 24px;
        }

        .f12 {
            FONT-SIZE: 12px;
            LINE-HEIGHT: 20px
        }

        form {
            MARGIN: 0;
            PADDING: 0
        }

        body {
            margin: 6px 0 4px;
            background-color: #FFF
        }

        form {
            margin: 0
        }

        td {
            color: #333
        }

        td, div {
            font-size: 12px;
            line-height: 18px
        }

        a:link {
            color: #03C
        }

        a:visited {
            color: #800080
        }

        .lg {
            margin-bottom: 20px
        }

        #ft {
            clear: both;
            line-height: 20px;
            text-align: center
        }

            #ft, #ft * {
                color: #77C;
                font-size: 12px;
                font-family: Arial;
                white-space: nowrap
            }

        #main {
            width: 760px
        }

        #left {
            float: left;
            width: 476px;
        }

        #center {
            float: left;
            margin: -22px 0 10px 0;
            width: 262px
        }

        #right {
            float: right;
            width: 22px;
            height: 130px;
        }

        .b1, .b2 {
            padding: 10px;
            line-height: 22px;
            font-size: 14px
        }

        .b1 {
            color: #333333
        }

        .b2 {
            margin-bottom: 12px;
            border: 1px solid #AAA;
            background: #F9F9F9
        }

        .hl {
            clear: both;
            margin-bottom: 10px;
            color: #666
        }

        a.tlk {
            line-height: 24px;
            color: #FFF;
            font-size: 14px;
            font-weight: bold
        }

            a.tlk:visited {
                color: #FFF
            }

        .ip {
            width: 120px;
            height: 22px;
            font-size: 14px
        }

        .f12 {
            font-size: 12px
        }

        .f14 {
            font-size: 14px
        }

        .f14b {
            font-size: 14px;
            font-weight: bold
        }

        .gry {
            color: #666
        }

        .line {
            margin-bottom: 5px;
            line-height: 1px;
            border-top: 1px solid #ccc
        }

        .ip {
            width: 120px;
            height: 22px;
            font-size: 14px
        }

        img {
            border: 0
        }
        .auto-style1 {
            height: 23px;
        }
    </style>
</head>
<body style="margin-top: 0px; margin-left: 0px; background-position: 0px 0px; background-image: url(images/bg_Login.jpg); background-repeat: no-repeat">
    <form id="form1" runat="server">
        <div>
            <table style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px;">
                <tr>
                    <td style="width: 200px; height: 250px"></td>
                    <td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <table style="width: 300px; height: 170px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; padding-top: 0px;">
                            <tr>
                                <td style="height: 23px; text-align: right;">
                                    <asp:Label ID="lblFabId" runat="server" Text="FabID" Font-Bold="False" Font-Names="微软雅黑" Font-Size="Medium"></asp:Label>：</td>
                                <td>
                                    <asp:DropDownList ID="cmbFabId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbFabId_SelectedIndexChanged"
                                        Width="100px" Enabled="False">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="height: 23px; text-align: right;">
                                    <asp:Label ID="lblUserID" runat="server" Text="帳號：" Font-Bold="False" Font-Names="微软雅黑" Font-Size="Medium"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtUId" runat="server" MaxLength="10" Width="120px" OnTextChanged="txtUId_TextChanged" AutoPostBack="True"></asp:TextBox>
                                    <asp:Label ID="lblUserName" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="height: 23px; text-align: right;">
                                    <asp:Label ID="lblPaseword" runat="server" Text="密碼：" Font-Bold="False" Font-Names="微软雅黑" Font-Size="Medium"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtPwd" runat="server" MaxLength="14" TextMode="Password" Width="120px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="height: 30px; text-align: center;" class="auto-style1" colspan="2">
                                    <asp:Button ID="btnConfirm" runat="server" Text="登入" OnClick="btnConfirm_Click" Font-Bold="False" Font-Names="微软雅黑" Font-Size="Medium" Width="80px" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 25px; text-align: center">
                                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
