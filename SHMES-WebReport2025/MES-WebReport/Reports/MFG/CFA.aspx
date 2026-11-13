<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CFA.aspx.cs" Inherits="Report.Reports.MFG.CFA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
    <div>
        <div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" Font-Size="Smaller">
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                <Columns>
                    <asp:BoundField DataField="WONBR" HeaderText="WONBR" SortExpression="WONBR" />
                    <asp:BoundField DataField="PROCESSID" HeaderText="PROCESSID" SortExpression="PROCESSID" />
                    <asp:BoundField DataField="PRODID" HeaderText="PRODID" SortExpression="PRODID" />
                    <asp:BoundField DataField="INPUTQTY" HeaderText="INPUTQTY" SortExpression="INPUTQTY" />
                    <asp:BoundField DataField="OUTPUTQTY" HeaderText="OUTPUTQTY" SortExpression="OUTPUTQTY" />
                </Columns>
            </asp:GridView>
            &nbsp;
            <asp:GridView ID="GridView2" runat="server" BackColor="Moccasin" 
                BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="WONBR,PROCESSID,PRODID"
                Font-Size="Smaller">
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            </asp:GridView>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Export Excel" /></div>
    
    </div>
    </form>
</body>
</html>
