<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TreeMenu.aspx.cs" Inherits="Report.TreeMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TreeMenu</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="position: absolute; left: 5px; width: 170px; bottom: 436px; top: 5px;">
            <%--<asp:Label ID="lbTitle" runat="server" Text="[選單]" Font-Bold="True" Font-Size="Small"></asp:Label>--%>
            <asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple" Width="40%" ShowLines="True" Target="main">
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                <LeafNodeStyle Font-Size="10pt" ForeColor="Black" />
                <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
                <ParentNodeStyle Font-Bold="False" />
                <RootNodeStyle Font-Bold="True" ForeColor="Black" Font-Size="10pt" />
                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
            </asp:TreeView>
            <asp:Label ID="lbMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
            <table>
                <tr style="height: 33px">
                    <td style="width: 35px; text-align: right;">
                        <img alt="" src="Images/user.png" style="background-image: url(Images/user.png)" />
                    </td>
                    <td style="width: 50px; vertical-align: middle;">
                        <table width="100%">
                            <tr>
                                <td >
                                    <img alt="" src="Images/left.png" />
                                </td>
                                <td style="background-image: url(Images/tb_bg.png); background-repeat: repeat-x">
                                    <asp:Label ID="lblUserName" runat="server" Width="90px" Font-Size="Smaller"></asp:Label>
                                </td>
                                <td >
                                    <img alt="" src="Images/right.png" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 35px; text-align: right;">
                        <img alt="" src="Images/ip.png" />
                    </td>
                    <td style="width: 50px; vertical-align: middle;">
                        <table width="100%">
                            <tr>
                                <td>
                                    <img alt="" src="Images/left.png" />
                                </td>
                                <td style="background-image: url(Images/tb_bg.png); background-repeat: repeat-x">
                                    <asp:Label ID="lblIp" runat="server" Width="90px" Font-Size="Smaller"></asp:Label>
                                </td>
                                <td>
                                    <img alt="" src="Images/right.png" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 41px; text-align: center;">
                        <asp:ImageButton ID="ibtnLogout" runat="server" ImageUrl="~/Images/Logout.png" OnClick="ibtnLogout_Click" />
                    </td>
                </tr>
            </table>

        </div>
    </form>

</body>
</html>
