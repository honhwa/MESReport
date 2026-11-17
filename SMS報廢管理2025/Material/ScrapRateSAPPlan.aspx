<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScrapRateSAPPlan.aspx.cs" Inherits="Material_ScrapRateSAPPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SAP Plan目標值設定</title>
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
    <asp:DropDownList ID="ddlYear" runat="server" >
    </asp:DropDownList>
    <asp:DropDownList ID="ddlMonth" runat="server" >
    </asp:DropDownList>
    <asp:TextBox ID="tbProjectName" runat="server"></asp:TextBox>
        <asp:RadioButtonList ID="rblPlan" runat="server" 
            Font-Names="Microsoft Sans Serif" Font-Size="12pt" RepeatDirection="Horizontal" 
            RepeatLayout="Flow">
            <asp:ListItem Selected="True" Value="Plan">目標值查詢</asp:ListItem>
            <asp:ListItem Value="TP">無目標值TP查詢</asp:ListItem>
        </asp:RadioButtonList>
    <asp:Button ID="btQuery" runat="server" onclick="btQuery_Click" Text="Query" />
    <asp:Button ID="btExport" runat="server" onclick="btExport_Click" Text="Export" />
        <br />
    <asp:FileUpload ID="FileUpload1" runat="server" Width="270px" />
    <asp:Button ID="btUPLoad" runat="server" onclick="btUPLoad_Click" 
        Text="UPLoad" />
    <a ID="ExportExcel" runat="server" target="_blank" style="font-size: 12px">下載Excel模板</a>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" 
        Font-Size="10pt" Font-Names="Microsoft Sans Serif" Width="800px" 
        CellPadding="4" ForeColor="#333333" GridLines="None" 
        onrowdatabound="gvData_RowDataBound">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Year" HeaderText="年"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="Month" HeaderText="月"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="TP_PART_NUM" HeaderText="TP"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="Shop" HeaderText="Shop"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="PlanRate" HeaderText="目標值"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="Time_Stamp" HeaderText="更新時間"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="Userid" HeaderText="更新人員"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="PCName" HeaderText="更新電腦名稱"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
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
        <asp:GridView ID="TP_PART_NUM" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" Font-Names="Microsoft Sans Serif" Font-Size="10pt" 
            ForeColor="#333333" GridLines="None" onrowdatabound="gvData_RowDataBound" 
            Width="800px">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
            <asp:BoundField DataField="ReportMonth" HeaderText="月"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="TP_PART_NUM" HeaderText="TP"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="Shop" HeaderText="Shop"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="PlanRate" HeaderText="目標值"><HeaderStyle Wrap="False" HorizontalAlign="Left" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
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
    </asp:Panel>
    </form>
</body>
</html>
