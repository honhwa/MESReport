<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScrapRateStation.aspx.cs" Inherits="Material_ScrapRateStation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>站點損耗率明細</title>
    <script language="javascript" type="text/javascript">
        function document.onkeydown() {
            if (event.keyCode == 27) {
                window.close();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="PRight" runat="server" Width="100%" Height="100%" ScrollBars="Auto" Font-Size="10pt" Font-Names="Microsoft Sans Serif" >
    <asp:Label ID="lblProjectName" runat="server" Text=""></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblPartType" runat="server" Text=""></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" 
            onrowdatabound="gvData_RowDataBound">
        <Columns>
            <asp:BoundField DataField="PRODID" HeaderText="產品號碼"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="TP_PART_NUM" HeaderText="13NC"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="COMP_PART" HeaderText="COMP_PART"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="WORKCTR" HeaderText="站點"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField> 
            <asp:BoundField DataField="Scrap1Qty" HeaderText="整體報廢" DataFormatString="{0:N0}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField> 
            <asp:BoundField DataField="Scrap2Qty" HeaderText="零散報廢" DataFormatString="{0:N0}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField> 
            <asp:BoundField DataField="ScrapQty" HeaderText="Scrap Qty" DataFormatString="{0:N0}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField> 
            <asp:BoundField DataField="OutputQty" HeaderText="Output Qty" DataFormatString="{0:N0}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField> 
            <asp:BoundField DataField="ScrapRate" HeaderText="損耗率" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField> 
        </Columns>
        <HeaderStyle BackColor="YellowGreen" />
    </asp:GridView>
    </asp:Panel>
    </form>
</body>
</html>
