<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WOCompress.aspx.cs" Inherits="Report.Reports.PC.WOCompress" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>壓縮工單</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Label  ID="lbltitle" runat ="server" Text ="月份："></asp:Label>
    <asp:DropDownList ID="ddlMonth" Width="200px" runat ="server" ></asp:DropDownList>
    <asp:Button ID="BtnQuery" runat="server" Text ="Query" OnClick="BtnQuery_Click" />
    <asp:Button ID="BtnExport" runat="server" Text ="Export" OnClick="BtnExport_Click" />
    <asp:Label ID="lblNote" runat="server" Width="50%" Text="" ForeColor ="red" ></asp:Label>
    <br />
    <asp:GridView ID="gvResult" RowStyle-VerticalAlign="middle" RowStyle-HorizontalAlign="center" 
     runat ="server" Width="98%" AutoGenerateColumns ="true" CellPadding="4" Font-Bold="True"  Font-Names="Arial" Font-Size="10pt"  GridLines ="Both" BorderColor="black" ForeColor="#333333" OnRowDataBound="gvResult_RowDataBound" >
        <RowStyle BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="YELLOW" Font-Bold="True"  CssClass ="Fix" ForeColor="BLACK" />
     </asp:GridView>
    </form>
</body>
</html>
