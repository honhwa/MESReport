<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LockScrewDataQuery.aspx.cs" Inherits="Report.LockScrewDataQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;</div>
        <table style="font-size: medium" align="center" border="0" id="Table1">
            <tr>
                <td style="height: 13px" align="center" colspan="2">
                    <asp:Label ID="Label9" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="DodgerBlue"
                        Height="24px" Width="400px">Lock Screw Data Query</asp:Label></td>
            </tr>
            <tr>
                <td align="left" style="width: 80px">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        Height="16px" Width="80px">From Time:</asp:Label></td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="Button" /></td>
            </tr>
            <tr>
                <td align="left" style="width: 80px">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        Height="16px" Width="56px">To Time:</asp:Label></td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" style="width: 80px">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        Height="16px" Width="56px">ProdId:</asp:Label></td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 22px">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        Height="16px" Width="56px">MachineId:</asp:Label></td>
                <td style="height: 22px">
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 22px">
                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        Height="16px" Width="56px">LotId:</asp:Label></td>
                <td style="height: 22px">
                    <input id="Text1" type="text" /></td>
            </tr>
            <tr>
                <td align="left" style="width: 80px">
                    <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        Height="16px" Width="56px">PanelId:</asp:Label></td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 80px">
                    <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        Height="16px" Width="80px">Result:</asp:Label></td>
                <td>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" CellPadding="0" CellSpacing="0"
                        Font-Names="Arial" Font-Size="10pt" Height="16px" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" Width="136px">
                        <asp:ListItem Selected="True">ALL</asp:ListItem>
                        <asp:ListItem Value="OK">OK</asp:ListItem>
                        <asp:ListItem Value="NG">NG</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align="left" style="width: 80px">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 80px">
                </td>
                <td align="center">
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 80px">
                </td>
                <td>
                    <font face="新細明體"></font>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
