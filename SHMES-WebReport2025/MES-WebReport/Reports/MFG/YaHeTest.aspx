<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YaHeTest.aspx.cs" Inherits="Report.Reports.MFG.YaHeTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>压合机报表查询</title>
    <script type="text/javascript">        dateFormat = 'yyyy-mm-dd'</script>
    <script src="popcalendar.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 80px;
            height: 36px;
        }
        .style2
        {
            height: 36px;
        }
    </style>
</head>
<body>
    <body background="../../Images/right_bg.jpg">
        <form id="form1" runat="server">
        <div>
            <table id="Table1" align="center" border="0" style="font-size: medium">
                <tr>
                    <td align="center" colspan="2" style="height: 13px">
                        <asp:Label ID="Label9" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="DodgerBlue"
                            Height="24px" Width="400px" Font-Bold="True">YaHeTest Query</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style1">
                        &nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="80px">From Time:</asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="txtStartDT" runat="server" Width="115px"></asp:TextBox>
                        <input id="Button11" class="CalendarBotton" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)"
                            style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                            background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                            width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px" type="button" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 80px">
                        &nbsp;<asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="56px">To Time:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtEndDT" runat="server" Width="115px"></asp:TextBox>
                        <input id="Button22" class="CalendarBotton" onclick="popUpCalendar(this, TxtEndDT, dateFormat,-1,-1,false)"
                            style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                            background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                            width: 25px; padding-top: 0px; height: 17px; border-right-width: 0px" type="button" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 80px">
                        &nbsp;<asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="56px">ProdId:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProdid" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 22px">
                        &nbsp;<asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="56px">OperationID:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOperationID" runat="server" Enabled="False">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 22px">
                        &nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="56px">PanelId:</asp:Label>
                    </td>
                    <td style="height: 22px">
                        <asp:TextBox ID="txtLot" runat="server" TextMode="MultiLine" Width="160px"></asp:TextBox><asp:Label
                            ID="Label6" runat="server" Font-Bold="True" Font-Size="XX-Small" ForeColor="Red">請以回車符隔開多個PanelId</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 80px">
                        &nbsp;<asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="50px">Result:</asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblResult" runat="server" CellPadding="0" CellSpacing="0"
                            Font-Names="Arial" Font-Size="10pt" Height="16px" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" Width="136px" Font-Bold="True">
                            <asp:ListItem Selected="True">ALL</asp:ListItem>
                            <asp:ListItem Value="OK">OK</asp:ListItem>
                            <asp:ListItem Value="NG">NG</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 80px">
                    </td>
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" Width="55px" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="55px" OnClick="btnClear_Click" />
                        <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click"
                            Width="55px" Enabled="False" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 21px;" colspan="2">
                        <font face="新細明體">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                        </font>
                    </td>
                </tr>
            </table>
        </div>
        <table align="center" border="0">
            <tr>
                <td style="width: 700px" align="center">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="GridView1_PageIndexChanging"
                        PageSize="15" Font-Size="Small">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="PANELID" HeaderText="PANELID">
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PRODID" HeaderText="Product ID">
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OperationID" HeaderText="OperationID">
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DATATIME" HeaderText="DATATIME">
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RESULT" HeaderText="Result">
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FLG" HeaderText="FLG">
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Pressure" HeaderText="Pressure">
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PressurePro" HeaderText="PressurePro">
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PressureTime" HeaderText="PressureTime" />
                            <asp:BoundField DataField="PressureTimePro" HeaderText="PressureTimePro" />
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        </form>
    </body>
</body>
</html>
