<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnLineReportAssyVer3.aspx.cs" Inherits="WebReport.OnLineReportAssyVer3" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" width="100%">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Timer ID="TimerPage" runat="server" 
        ontick="TimerPage_Tick">
    </asp:Timer>
    <table cellpadding="0" cellspacing="0">
        <tr >
            <td> 
                <table border=1 width="940px">
                    <tr >
                        <td align="center"  colspan="2" style="background-color: #99CCFF">
                            <table cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left" height="100%">
                                        <asp:Image ID="imgLogo" runat="server" ImageUrl="getLogo.ashx?Customer=''" Width="200px"
                                            Height="160px" />
                                    </td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" width="734px">
                                            <tr>
                                                <td height="8px" colspan=2>
                                                    <br />
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td align="right" style="font-size: large; font-weight: bold;" width="100px">
                                                <td align="left"  >
                                                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Names="新細明體"
                                                        Width="530px" Font-Size="XX-Large"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="16px" colspan=2>
                                                    <br />
                                                </td>
                                                
                                            </tr>
                                            <tr >
                                                <td align="right" style="font-size: large; font-weight: bold;" width="100px"> &nbsp;</td>
                                                <td align="left" style="font-size: large; font-weight: bold;">生產型號Product:&nbsp;
                                                    <asp:Label ID="lblProduct" runat="server" Font-Bold="True" Font-Names="新細明體"
                                                        Width="326px" Font-Size="Large" ForeColor="#000099"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td align="left" style="font-size: large; font-weight: bold;">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblMoudleTest" runat="server" Font-Bold="True" Font-Names="新細明體"
                                                        Width="416px" Font-Size="Large" ForeColor="#000099"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td align="left" style="font-size: large; font-weight: bold;">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblOQC" runat="server" Font-Bold="True" Font-Names="新細明體"
                                                        Width="416px" Font-Size="Large" ForeColor="#000099"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="8px" colspan=2>
                                                    
                                                </td>
                                                
                                            </tr>
                                            <tr valign="bottom">
                                                <td></td>
                                                <td align="right">
                                                    <asp:Label ID="lblupdateTime" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                    <asp:Label ID="lblUpdateDescr" runat="server" Text="" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                           
                           
                        </td>
                    </tr>
                    <tr>
                        <td width="50%">
                            <div id="left" align="left">
                                <asp:Chart ID="crtAccumulate" runat="server" Align="left" Width="460px" Height="310px">
                                    <Series>
                                        <asp:Series Name="Series1">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                        </td>
                        <td width="50%">
                            <div id="right">
                                <asp:Chart ID="crtPerHour" runat="server" Width="460px" Height="310px">
                                    <Series>
                                        <asp:Series Name="Series1" YValuesPerPoint="2">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                        </td>
                    </tr>
                </table>
                <table border="1" width="940px">
                    <tr>
                        <td valign="top">
                            <asp:Panel ID="pnlAssy" runat="server" GroupingText="Assy YieldTarget 99.5%" 
                                Width="300px" Height="140px">
                                <asp:GridView ID="gvPOL" runat="server" AutoGenerateColumns="False" 
                                    onrowdatabound="gvPOL_RowDataBound">
                                    <Columns>                                        
                                        <asp:BoundField DataField="Yield" HeaderText="ACT. Yield" DataFormatString="{0:N2}">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DefectCD" HeaderText="Code">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DefectDesc" HeaderText="Desc.">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td valign="top">
                            <asp:Panel ID="pnlModuleTest" runat="server" 
                                GroupingText="Module Test  Yield Target 99%" Width="300px">
                                <asp:GridView ID="gvCog" runat="server" AutoGenerateColumns="False" 
                                    onrowdatabound="gvCog_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="Yield" HeaderText="ACT. Yield" DataFormatString="{0:N2}">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DefectCD" HeaderText="Code">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DefectDesc" HeaderText="Desc.">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td valign="top">
                            <asp:Panel ID="pnlOQC" runat="server" GroupingText="OQC Yield Target 100%" 
                                Width="300px">
                                <asp:GridView ID="gvFOG" runat="server" AutoGenerateColumns="False" 
                                    onrowdatabound="gvFOG_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="Yield" HeaderText="ACT. Yield" DataFormatString="{0:N2}">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DefectCD" HeaderText="Code">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DefectDesc" HeaderText="Desc.">
                                            <HeaderStyle BackColor="#99FF99" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <table border="1" width="940px">
                    <tr>
                        <td align="Left" style="background-color: #CCFFCC;" height="100%">
                            <asp:GridView ID="gvActAndTarget" runat="server" BackColor="White" 
                                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                ForeColor="Black" GridLines="Vertical" 
                                onrowdatabound="gvActAndTarget_RowDataBound" style="margin-right: 0px">
                                <AlternatingRowStyle BackColor="White" />
                                <FooterStyle BackColor="#CCCC99" />
                                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                <RowStyle BackColor="#F7F7DE" />
                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                <SortedAscendingHeaderStyle BackColor="#848384" />
                                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                <SortedDescendingHeaderStyle BackColor="#575357" />
                            </asp:GridView>
                        </td>
                        
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>
    
    
   
    </form>
</body>
</html>
