<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActualOutputMark.aspx.cs" Inherits="Report.Reports.PC.ActualOutputMark" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>備註說明</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 150px; height: 25px">
            <tr>
                <td style="width: 100px; height: 113px;">
                    <asp:TextBox ID="TextBox1" runat="server" Height="102px" MaxLength="80" Width="135px" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" style="width: 100px">
                    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" /></td>
            </tr>
        </table>
    
    </div>
        <asp:TextBox ID="txtFabID" runat="server" Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtMFGDate" runat="server" Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtProdLine" runat="server" Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtColID" runat="server" Visible="False"></asp:TextBox>
        <asp:TextBox ID="txtProductID" runat="server" Visible="False"></asp:TextBox>
    </form>
</body>
</html>
