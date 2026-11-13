<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActualOutputQuery.aspx.cs" Inherits="WebReport.Reports.ActualOutputQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>南京及時產出報表</title>
</head>
<body>
    <form id="form1" runat="server">
        <table style="height: 1px; width: 100%;">
            <tr>
                <td align="center" colspan="8" style="height: 2px">
       <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="X-Large" Text="及時產出報表" BackColor="White" Width="312px"></asp:Label></td>
            </tr>
                        <tr>
                <td style="width: 85px; height: 2px">
                    <asp:DropDownList ID="DDLMFGDate" runat="server" AutoPostBack="True" Width="130px">
                    </asp:DropDownList></td>
                <td style="width: 100px; height: 2px">
                    <asp:DropDownList ID="DDLProdLine" runat="server" AutoPostBack="True" Width="130px">
                    </asp:DropDownList></td>
                <td style="width: 100px; height: 2px">
                    <asp:DropDownList ID="DDLApplication" runat="server" AutoPostBack="True" Width="130px">
                    </asp:DropDownList></td>
                <td style="width: 100px; height: 2px">
                    <asp:DropDownList ID="DDLCustomer" runat="server" AutoPostBack="True" Width="130px" >
                    </asp:DropDownList></td>
                <td style="width: 80px; height: 2px">
                                        <asp:Button ID="btnQuery" runat="server" Font-Size="X-Small" OnClick="btnQuery_Click"
                                            Text="查詢" Width="40px" />
                    &nbsp;
                    <asp:ImageButton ID="ImageButton1" runat="server" OnClick="ImageButton1_Click" Height="17px" ImageUrl="~/Images/xls.jpg" ToolTip="匯出EXCEL" Width="25px" /></td>
                            <td colspan="3" style="height: 2px">
                    <asp:Label ID="Label2" runat="server" BackColor="White" Font-Names="Arial" Font-Size="Smaller"
                        ForeColor="Red"></asp:Label></td>
            </tr>
        </table>
        <asp:TextBox ID="txtMUGMM" runat="server" Visible="False"></asp:TextBox><asp:GridView ID="GridView1" runat="server" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" style="border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="FABID" HeaderText="廠別" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /></asp:BoundField>
                <asp:BoundField DataField="PRODINFO" HeaderText="線別產品型號" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False"  BorderWidth="1px"/></asp:BoundField>
                <asp:BoundField DataField="Category" HeaderText="Category" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /></asp:BoundField>
                <asp:BoundField DataField="TTL" HeaderText="TTL" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="MTD" HeaderText="MTD" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="WTD" HeaderText="WTD" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="T1" HeaderText="10/03" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="T2" HeaderText="10/04" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="T3" HeaderText="10/05" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H8" HeaderText="H8" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H9" HeaderText="H9" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H10" HeaderText="H10" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H11" HeaderText="H11" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H12" HeaderText="H12" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H13" HeaderText="H13" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H14" HeaderText="H14" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H15" HeaderText="H15" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H16" HeaderText="H16" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H17" HeaderText="H17" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H18" HeaderText="H18" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H19" HeaderText="H19" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H20" HeaderText="H20" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H21" HeaderText="H21" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H22" HeaderText="H22" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H23" HeaderText="H23" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H0" HeaderText="H0" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H1" HeaderText="H1" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H2" HeaderText="H2" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H3" HeaderText="H3" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H4" HeaderText="H4" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H5" HeaderText="H5" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H6" HeaderText="H6" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px"  HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="H7" HeaderText="H7" ><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" HorizontalAlign="Center" /></asp:BoundField>
                <asp:BoundField DataField="ProductID" HeaderText="ProductID"><HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" /><ItemStyle Font-Names="Arial" Font-Size="Small" Wrap="False" BorderWidth="1px" HorizontalAlign="Center" /></asp:BoundField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
