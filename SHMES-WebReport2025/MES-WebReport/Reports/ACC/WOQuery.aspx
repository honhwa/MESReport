<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WOQuery.aspx.cs" Inherits="Report.Reports.ACC.WOQuery" StylesheetTheme="UNICOMTheme" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>WOQuery</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />

</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="Panel1" runat="server" Height="141px" Width="809px">
            <asp:TextBox ID="TextBox1" runat="server" Height="130px" TextMode="MultiLine"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢" Width="50px" />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="匯出" Width="49px" /></asp:Panel>
        <br />
        &nbsp;</div>
        <asp:Panel ID="Panel2" runat="server" Height="288px" Width="808px">
            &nbsp;
            <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#3366CC"
                BorderStyle="None" BorderWidth="1px" CellPadding="4">
                <RowStyle BackColor="White" ForeColor="#003399" />
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            </asp:GridView>
        </asp:Panel>
    </form>
</body>
</html>
