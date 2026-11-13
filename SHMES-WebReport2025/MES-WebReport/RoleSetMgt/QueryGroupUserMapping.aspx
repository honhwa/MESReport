<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryGroupUserMapping.aspx.cs" Inherits="Report.RoleSetMgt.QueryGroupUserMapping" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbTitle" runat="server" ForeColor="Blue" Text="[查詢群組中帳號]"></asp:Label>
            <hr />
            群組：<asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="True" DataTextField="udepart_name" DataValueField="udepart" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
                <asp:ListItem>---請選擇---</asp:ListItem>
            </asp:DropDownList>
            &nbsp;<asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查詢" Visible="False" />
            <br />
            <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
            <asp:GridView ID="gvResult" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical">
                <AlternatingRowStyle BackColor="#DCDCDC" />
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
