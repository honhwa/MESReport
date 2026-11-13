<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MainReport.aspx.cs" Inherits="WebReport.MainReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MainReport</title>
    <link href="Stylesheet.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
// <!CDATA[

function IMG1_onclick() {

}

// ]]>
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="width: 100%; height: 57px; background-position: right center; background-image: url(Images/head.jpg);">
            </div>
            <table style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                padding-top: 0px; width: 100%" cellspacing="0">
                <tr>
                    <td style="width: 176px; background-position-x: right; background-attachment: scroll;
                        background-image: url(Images/left_bg.jpg); background-repeat: repeat-y">
                        <img src="Images/logo.jpg" /></td>
                    <td style="width: 12px; background-position-x: left; background-image: url(Images/right_bg.jpg);
                        background-repeat: repeat-y;">
                    </td>
                    <td style="background-position-x: center; background-image: url(Images/right_bg.jpg);
                        background-repeat: repeat-y;">
                    </td>
                </tr>
                <tr>
                    <td style="background-position-x: right; background-attachment: scroll; background-image: url(Images/left_bg.jpg);
                        background-repeat: repeat-y; height: 450px; vertical-align: top;">
                        <table style="text-align: center">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="btnRpeort1" runat="server" Height="55px" ImageUrl="~/Images/button_1.jpg"
                                        Width="161px" PostBackUrl="~/Reports/ModuleMaterielScrapQuery.aspx" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/button2.jpg" OnClick="ImageButton1_Click" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <img src="Images/button_bg.jpg" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <img src="Images/button_bg.jpg" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <img src="Images/button_bg.jpg" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <img src="Images/button_bg.jpg" /></td>
                            </tr>
                            <tr>
                                <td style="height: 20px">
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td rowspan="2" style="background-color: #3c2679">
                    </td>
                    <td style="background-position-x: center; background-image: url(Images/right_bg.jpg);
                        background-repeat: repeat-y; vertical-align: top;">
                    </td>
                </tr>
                <tr>
                    <td style="background-position-x: right; background-attachment: scroll; background-image: url(Images/left_bg.jpg);
                        background-repeat: repeat-y; height: 21px; text-align: center;">
                        <asp:ImageButton ID="btnLogout2" runat="server" ImageUrl="~/Images/Logout.png" OnClick="btnLogout_Click" /></td>
                    <td style="background-position-x: center; background-image: url(Images/right_bg.jpg);
                        background-repeat: repeat-y; text-align: right;">
                        &nbsp;<table cellpadding="0" style="text-align: left; vertical-align: middle;">
                            <tr>
                                <td style="background-position: left center; background-image: url(Images/userinfo_bg.png);
                                    width: 403px; height: 33px">
                                    <table>
                                        <tr>
                                            <td style="width: 50px">
                                            </td>
                                            <td style="width: 110px">
                                                <asp:Label ID="lblIp" runat="server" Width="110px"></asp:Label></td>
                                            <td style="width: 90px">
                                            </td>
                                            <td style="width: 70px">
                                                <asp:Label ID="lblUserName" runat="server" Width="70px"></asp:Label></td>
                                            <td style="width: 20px">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 30px;">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
