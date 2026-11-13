<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EDC.aspx.cs" Inherits="YMSReport_EDC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EDC Report</title>
    <style type="text/css">
        body { margin:0;}
     </style>
    <script type="text/javascript" language="javascript" src="popcalendar.js"></script>
    <script type="text/javascript" language="JavaScript">dateFormat='yyyy-mm-dd'</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divFilter" runat="server">
        <table style="font-weight: bold; font-size: 15px; color: #0000ff; font-family: Arial; font-variant: normal; border-right: blue 2px solid; border-top: blue 2px solid; border-left: blue 2px solid; border-bottom: blue 2px solid;">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlBeol" runat="server" Enabled="False" Width="155px" Font-Bold="True">
                        <asp:ListItem>BEOL</asp:ListItem>
                    </asp:DropDownList></td>
                <td colspan="4">
                    From:<asp:TextBox ID="txtFDate" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="10pt" MaxLength="10" Width="80px"></asp:TextBox><img id="imgF" runat="server"
                            height="17" onclick="popUpCalendar(this, form1.txtFDate, dateFormat,-1,-1,false)"
                            src="../images/calendar.gif" type="button" width="30" /><asp:DropDownList ID="ddlFHour" runat="server" Font-Bold="True" Width="41px">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                            </asp:DropDownList>：<asp:DropDownList ID="ddlFMinute" runat="server" Font-Bold="True"
                        Width="40px">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>35</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                            </asp:DropDownList>
                    &nbsp; &nbsp; To:<asp:TextBox ID="txtTDate" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="10pt" MaxLength="10" Width="80px"></asp:TextBox><img id="imgT" runat="server" height="17" onclick="popUpCalendar(this, form1.txtTDate, dateFormat,-1,-1,false)"
                                    src="../images/calendar.gif" type="button" width="30" /><asp:DropDownList ID="ddlTHour" runat="server" Font-Bold="True" Width="41px">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                            </asp:DropDownList>：<asp:DropDownList ID="ddlTMinute" runat="server" Font-Bold="True"
                        Width="40px">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>35</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                            </asp:DropDownList></td>
                <td >
                </td>
                <td >
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlStation" runat="server" Width="155px" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" OnSelectedIndexChanged="ddlStation_SelectedIndexChanged">
                        <asp:ListItem Value="All">Station(Over All)</asp:ListItem>
                    </asp:DropDownList></td>
                <td >
                    <asp:DropDownList ID="ddlQueryBy" runat="server" Width="155px" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" OnSelectedIndexChanged="ddlQueryBy_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="All">QueryBy(All)</asp:ListItem>
                        <asp:ListItem Value="1">GLASSID</asp:ListItem>
                        <asp:ListItem Value="2">LOTID</asp:ListItem>
                    </asp:DropDownList></td>
                <td >
                    <asp:DropDownList ID="ddlShiftID" runat="server" Width="120px" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" OnSelectedIndexChanged="ddlShiftID_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="All">ShiftID(All)</asp:ListItem>
                        <asp:ListItem Value="1">UserDefine</asp:ListItem>
                    </asp:DropDownList></td>
                <td >
                    <asp:DropDownList ID="ddlFloor" runat="server" Width="120px" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" OnSelectedIndexChanged="ddlFloor_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="All">Floor(All)</asp:ListItem>
                        <asp:ListItem Value="1">UserDefine</asp:ListItem>
                    </asp:DropDownList></td>
                <td >
                    <asp:DropDownList ID="ddlProject" runat="server" Width="220px" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="All">Project(All)</asp:ListItem>
                        <asp:ListItem Value="1">UserDefine</asp:ListItem>
                    </asp:DropDownList></td>
                <td >
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pStation" runat="server" Height="120px" ScrollBars="Vertical" Width="155px" Visible="False">
                        <asp:CheckBoxList ID="chklStation" runat="server" Font-Names="Arial" Font-Size="10pt"
                            RepeatColumns="1" RepeatLayout="Flow">
                        </asp:CheckBoxList></asp:Panel>
                </td>
                <td style="vertical-align: text-top; text-align: left" >
                    <asp:TextBox ID="txtQueryBy" runat="server" Height="112px" TextMode="MultiLine" Visible="False"
                        Width="155px"></asp:TextBox></td>
                <td>
                    <asp:Panel ID="pShiftID" runat="server" Height="120px" ScrollBars="Vertical" Width="123px" Visible="False">
                        <asp:CheckBoxList ID="chklShiftID" runat="server" Font-Names="Arial" Font-Size="10pt"
                            RepeatColumns="1" RepeatLayout="Flow">
                        </asp:CheckBoxList></asp:Panel>
                </td>
                <td >
                    <asp:Panel ID="pFloor" runat="server" Height="120px" ScrollBars="Vertical" Width="123px" Visible="False">
                        <asp:CheckBoxList ID="chklFloor" runat="server" Font-Names="Arial" Font-Size="10pt"
                            RepeatColumns="1" RepeatLayout="Flow">
                        </asp:CheckBoxList></asp:Panel>
                </td>
                <td >
                    <asp:Panel ID="pProject" runat="server" Height="120px" ScrollBars="Vertical" Width="220px" Visible="False">
                        <asp:CheckBoxList ID="chklProject" runat="server" Font-Names="Arial" Font-Size="10pt"
                            RepeatColumns="1" RepeatLayout="Flow">
                        </asp:CheckBoxList></asp:Panel>
                </td>
                <td >
                </td>
                <td >
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:TextBox ID="tbFABID" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                        ID="tbFABName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                            ID="tbLoginNTName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                ID="tbLoginPCName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                    ID="tbLoginPCIP" runat="server" Visible="False" Width="50px"></asp:TextBox></td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
        <table >
            <tr>
                <td valign="top" style="width: 636px">
                    <asp:Button ID="btQuery" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        OnClick="btQuery_Click" Text="Query" />
                    <asp:Button ID="btExport" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        OnClick="ExportExcel_Click" Text="ExportExcel" />
                    <asp:Button ID="btClear" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        OnClick="btClear_Click" Text="Clear" />&nbsp; &nbsp;<asp:Label ID="lblFabID" runat="server"
                            Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Width="60px"></asp:Label>&nbsp;
                    &nbsp;<asp:Label ID="lblTimeSpan" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="10pt" ForeColor="Blue" Text="查詢耗時:" Visible="False"></asp:Label>&nbsp;
                    &nbsp;&nbsp;
                </td>
                <td align="left" valign="top" style="width: 123px">
                    <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="10pt" ToolTip="返回主頁面">Main Menu</asp:HyperLink></td>
                <td align="right" valign="top" style="width: 44px">
                    <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="10pt" NavigateUrl="~/Description/EDCReport.htm" Target="_blank"
                        ToolTip="報表說明">Help</asp:HyperLink></td>
            </tr>
            <tr>
                <td align="right" colspan="3" style="color: blue; text-align: left;" valign="top">
                    ---------------------------------------------------------------------------------------------------------------------------------------</td>
            </tr>
            <tr>
                <td align="left" colspan="3" style="font-weight: normal; color: red; font-family: Arial;
                    font-variant: normal" valign="top">
                    <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="10pt"
                        Text="ErrorMsg" Visible="False"></asp:Label></td>
            </tr>
        </table>
        <table width="810px" style="border-right: blue 2px solid; border-top: blue 2px solid; border-left: blue 2px solid; border-bottom: blue 2px solid" runat="server">
            <tr>
                <td colspan="3" style="vertical-align: text-top; text-align: left">
                    <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Both" Width="806px" Visible="False">
                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="PROJECT_NAME" HeaderText="PROJECT_NAME">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NWORKCTR" HeaderText="WORKCTR">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TRANSDATE" HeaderText="TRANSDATE">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOTID" HeaderText="LOTID">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GLASSID" HeaderText="SOLOTID/GlassID">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CARTON" HeaderText="CARTON_ID">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SHIFTID" HeaderText="SHIFTID">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="color: #0000ff">
                    -------------------------------------------------------------------------------------------------------------------------------------</td>
            </tr>
            <tr>
                <td colspan="3" >
                    <asp:Panel ID="Panel2" runat="server" Height="300px" ScrollBars="Both" Width="806px" Visible="False">
                        <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="PROJECT_NAME" HeaderText="PROJECT_NAME">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CWORKCTR" HeaderText="WORKCTR">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOTID" HeaderText="LOTID">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TRANSDATE" HeaderText="TRANSDATE">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="INPUTQTY" HeaderText="INPUTQTY">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OUTPUTQTY" HeaderText="OUTPUTQTY">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="YIELD" HeaderText="YIELD">
                                    <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="USERID" HeaderText="OPERATOR">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CSUBLINEID" HeaderText="MACHINE">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SHIFTID" HeaderText="SHIFTID">
                                    <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
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
