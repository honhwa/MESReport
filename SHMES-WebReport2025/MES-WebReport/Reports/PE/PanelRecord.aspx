<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PanelRecord.aspx.cs" Inherits="Report.Reports.PE.PanelRecord" StylesheetTheme="UNICOMTheme" Theme="UNICOMTheme" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../../Images/right_bg.jpg); text-align: center">
<form id="form1" runat="server">
    <div>
        <br />
        <table style="width: 90%;">
            <tr>
                <td colspan="1" style="font-size: 20pt">
                    &nbsp; &nbsp; &nbsp;</td>
                <td colspan="3" style="font-size: 20pt;">
                    Panel Record 查詢</td>
            </tr>
            <tr>
                <td style="width: 20px; text-align: left">
                </td>
                <td colspan="2" style="vertical-align: top; text-align: left">
                    LOTID ：<asp:TextBox ID="txtLotID" runat="server"></asp:TextBox><br />
                    <br />
                    PanelID：<asp:TextBox ID="txtPanelID" runat="server"></asp:TextBox></td>
                <td style="text-align: left; vertical-align: top; width: 50%;">
                    Product_ID： &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<asp:Panel ID="Panel2" runat="server"
                        Height="150px" HorizontalAlign="Left" ScrollBars="Both" Width="180px">
                        <asp:CheckBox ID="chkAll" runat="server" Text="ALL" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged" /><asp:CheckBoxList ID="chklProdid"
                            runat="server">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="1" style="text-align: left">
                </td>
                <td colspan="3" style="text-align: left; width: 90%;">
                    <asp:Button ID="btnQuery" runat="server" Font-Bold="False" Font-Size="12pt" Height="30px"
                        Text="查詢" Width="90px" OnClick="btnQuery_Click" />&nbsp;
                    <asp:Button ID="btnClear" runat="server" Font-Bold="False" Font-Size="12pt" Height="30px"
                        Text="CLEAR" Width="90px" OnClick="btnClear_Click" />
                    <asp:Button ID="btnDownLoad" runat="server" Font-Bold="False" Font-Size="12pt" Height="30px"
                        Text="匯出" Width="90px" OnClick="btnDownLoad_Click" />&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1" style="vertical-align: top; text-align: left">
                </td>
                <td colspan="3" style="vertical-align: top; text-align: left">
                    &nbsp;<asp:Label ID="lblError" runat="server" Font-Size="12pt" ForeColor="Red" Visible="False"></asp:Label></td>
            </tr>
        </table>
    
    </div>
    <table style="width: 90%; height: 60%">
        <tr>
            <td colspan="1">
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            </td>
            <td colspan="3" style="vertical-align: top; text-align: left; width: 70%;">
                    <asp:Panel ID="Panel1" runat="server" Height="300px" HorizontalAlign="Left" ScrollBars="Both"
                        Style="width: 65%;" Width="100%" Visible="False">
                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="ENTITYID" HeaderText="LOTID">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PANELID" HeaderText="PANELID">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PWORKCTR" HeaderText="PreWorkctr">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CWORKCTR" HeaderText="CWORKCTR">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOTGRADE" HeaderText="LOTGRADE">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOTLEVEL" HeaderText="LOTLEVEL">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DEFECTCD" HeaderText="DEFECTCD">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SOPRODID" HeaderText="SOPRODID">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PRODID" HeaderText="PRODUCTID">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PSUBLINEID" HeaderText="PSUBLINEID">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SUBLINEID" HeaderText="SUBLINEID">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="WONBR" HeaderText="WONBR">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOTTYPE" HeaderText="LOTTYPE">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PSHOPCD" HeaderText="PSHOPCD">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CSHOPCD" HeaderText="CSHOPCD">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="USERID" HeaderText="USERID">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="USERNAME" HeaderText="USERNAME">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TRANSID" HeaderText="TRANSID">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
