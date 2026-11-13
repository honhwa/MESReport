<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Burn_In_RecordQuery.aspx.cs" Inherits="Report.Reports.MFG.Burn_In_RecordQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
        <script language="javascript" src="../IEB/popcalendar.js"></script>
	<script language="JavaScript">dateFormat='yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt; vertical-align: middle;">
    <form id="form1" runat="server">
    <div style="font-size: 22pt">
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp;&nbsp;
 Burn In記錄查詢
        <table style="font-size: 14pt" >
            <tr>
                <td>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                </td>
                <td>
                    From Time:</td>
                <td><asp:TextBox ID="txtStartDT" runat="server" Width="115px"></asp:TextBox><input id="Button11"
                                class="CalendarBotton" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)"
                                style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                                background-image: url(../../Images/Button.gif); border-bottom-width: 0px;
                                padding-bottom: 0px; width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px"
                                type="button" />
                    </td>
                <td>
                To&nbsp; Time:</td>
                <td colspan="2">
                    &nbsp;<asp:TextBox ID="TxtEndDT" runat="server" Width="115px"></asp:TextBox><input id="Button22"
                                class="CalendarBotton" onclick="popUpCalendar(this, TxtEndDT, dateFormat,-1,-1,false)"
                                style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                                background-image: url(../../Images/Button.gif); border-bottom-width: 0px;
                                padding-bottom: 0px; width: 25px; padding-top: 0px; height: 17px; border-right-width: 0px"
                                type="button" />
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="height: 10px">
                </td>
                <td style="height: 10px">
                    Product_ID:</td>
                <td rowspan="2">
                    <asp:Panel ID="PnlProdid" runat="server" Height="150px" Width="150px" ScrollBars="Both">
                        <asp:CheckBoxList ID="chklProdid" runat="server" Font-Bold="True" Font-Size="9pt"
                            ForeColor="#0000C0">
                        </asp:CheckBoxList></asp:Panel>
                </td>
                <td colspan="1" rowspan="1" style="height: 10px">
                    Work Order:</td>
                <td colspan="2" rowspan="2" style="height: 120px">
                    <asp:Panel ID="PnlWonbr" runat="server" Height="150px" Width="150px" ScrollBars="Both">
                    <asp:CheckBoxList ID="chklWonbr" runat="server" Font-Bold="True" Font-Size="9pt"
                        ForeColor="#0000C0">
                    </asp:CheckBoxList></asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td colspan="1" rowspan="1">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    Lot/Panel ID:</td>
                <td>
                    <asp:TextBox ID="txtPanelID" runat="server" Height="18px" Width="141px"></asp:TextBox></td>
                <td colspan="1" rowspan="1">
                    Good Panel:</td>
                <td colspan="2" rowspan="1">
                    &nbsp; &nbsp;&nbsp;
                    <asp:CheckBox ID="chkY" runat="server" Text="Y" />
                    &nbsp; &nbsp;&nbsp; &nbsp;<asp:CheckBox ID="chkN" runat="server" Text="N" /></td>
            </tr>
            <tr>
                <td colspan="1" style="height: 21px">
                </td>
                <td colspan="5" style="height: 21px">
                    <asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" />
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" /></td>
            </tr>
            <tr>
                <td colspan="1" style="height: 21px">
                </td>
                <td colspan="5" style="height: 21px">
                    <asp:Label ID="lblERROR" runat="server" ForeColor="Red" Visible="False"></asp:Label></td>
            </tr>
        </table>
    
    </div>
        <table>
            <tr>
                <td colspan="3" rowspan="3" style="width: 70%; height: 50%;"><asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Style="width: 80%;" Width="125px" Height="300px">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="15">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:BoundField DataField="TRANINDATE" HeaderText="Burn-in Start Date">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TRANOUTDATE" HeaderText="Burn-in End Date">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PanelID" HeaderText="Panel ID">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Entityid" HeaderText="LOT No.">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="workctr" HeaderText="Station">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="qty" HeaderText="QTY">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="prodid" HeaderText="Product ID">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="prodgroup" HeaderText="Product Group">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="wonbr" HeaderText="Work Order">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StorageID" HeaderText="Storage">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SUBLINEID" HeaderText="SUBLINEID">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="defectcd" HeaderText="Defect CODE">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="userid" HeaderText="User ID">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
        </table>
    </form>
</body >
</html>
