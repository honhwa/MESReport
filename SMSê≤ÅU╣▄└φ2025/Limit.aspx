<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Limit.aspx.cs" Inherits="Limit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>料件金額設定</title>
    <script language="javascript" type="text/javascript">
        function document.onkeydown() {
            if (event.keyCode == 27) {
                window.close();
            }
        }
    </script>
</head>
<body style="margin:0">
    <form id="form1" runat="server">
    <asp:Panel ID="Main" runat="server">
    <asp:TextBox ID="tbMaterial" runat="server"></asp:TextBox>
        <asp:Button ID="btQuery" runat="server" onclick="btQuery_Click" Text="Query" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btUPLoad" runat="server" onclick="btUPLoad_Click" 
        Text="UPLoad" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btExport" runat="server" onclick="btExport_Click" Text="Export" />
        <br />
    <%--<asp:FileUpload ID="FileUpload1" runat="server" Width="270px" />--%>

    <asp:GridView ID="gvMaterial" runat="server" AutoGenerateColumns="False" 
        Font-Size="10pt" Font-Names="Microsoft Sans Serif" Width="800px" 
        CellPadding="4" ForeColor="#333333" GridLines="None" 
        onrowdatabound="gvMaterial_RowDataBound">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="USERID" HeaderText="用戶名"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="USERGROUPCD" HeaderText="功能"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="USERGROUPDESC" HeaderText="功能描述"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView> 
    <asp:TextBox ID="tbNTName" runat="server" Visible="False"></asp:TextBox>
        <asp:TextBox ID="tbPCName" runat="server" Visible="False"></asp:TextBox>
        <asp:TextBox ID="tbUGC" runat="server" Visible="False"></asp:TextBox>
    </asp:Panel>
    </form>
</body>
</html>
