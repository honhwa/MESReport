<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tptmpmgm1.aspx.cs" Inherits="Report.Reports.TPT.tptmpmgm11" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div>
        <table style="width: 1018px">
            <tr>
                <td colspan="3" rowspan="3" style="width: 1169px">
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="產品與群組對照"></asp:Label></td>
            </tr>
            <tr style="font-size: 12pt; color: #000000">
            </tr>
            <tr style="font-size: 12pt; color: #000000">
            </tr>
        </table>
        <br />
        <table style="font-size: 12pt; width: 1016px; color: #000000">
            <tr>
                <td colspan="3" rowspan="3" style="width: 1012px; height: 26px">
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Label ID="Label2" runat="server"
                        Text="群組"></asp:Label>
                    &nbsp; &nbsp; &nbsp;&nbsp;
                    <asp:DropDownList ID="ddlgroup" runat="server" BackColor="Info" Style="border-top-style: none;
                        border-right-style: none; border-left-style: none; border-bottom-style: none"
                        Width="150px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
        </table>
    
    </div>
        <table style="width: 1010px">
            <tr>
                <td colspan="3" rowspan="3" style="width: 1006px; height: 161px">
                    <asp:GridView ID="gvAll" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="None" OnRowCommand="gvAll_RowCommand1" Width="648px">
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:BoundField DataField="FABID" HeaderText="FABID" />
                            <asp:BoundField DataField="PRODID" HeaderText="Product ID" />
                            <asp:BoundField DataField="GroupName" HeaderText="Group" />
                            <asp:ButtonField Text="按鈕">
                                <ItemStyle BackColor="Info" />
                            </asp:ButtonField>
                        </Columns>
                        <RowStyle BackColor="#E3EAEB" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
        </table>
    </form>
</body>
</html>
