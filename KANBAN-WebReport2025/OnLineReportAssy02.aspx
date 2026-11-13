<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnLineReportAssy02.aspx.cs" Inherits="WebReport.OnLineReportAssy02" %>

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
    <asp:Timer ID="TimerPage" runat="server" Interval="110000" 
        ontick="TimerPage_Tick">
    </asp:Timer>
    <table cellpadding="0" cellspacing="0">
        <tr >
            <td> 
                <table border=1 width="940px">
                    <tr>
                        <td  align="center"  colspan="2" style="background-color: #99CCFF">
                            <asp:Label ID="lblTitle" runat="server" Text="" Font-Bold="True" Font-Names="新細明體"
                                Width="690px" Font-Size="X-Large"></asp:Label>
                            
                            <asp:Label ID="lblupdateTime" runat="server" Text="Label" Font-Bold="True"></asp:Label>
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
                            <asp:Panel ID="pnlAssy" runat="server" GroupingText="Assy" Width="300px" Height="140px">
                                <asp:GridView ID="gvPOL" runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="Yield" HeaderText="Yield" DataFormatString="{0:N2}">
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
                            <asp:Panel ID="pnlModuleTest" runat="server" GroupingText="Module Test" Width="300px">
                                <asp:GridView ID="gvCog" runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="Yield" HeaderText="Yield" DataFormatString="{0:N2}">
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
                            <asp:Panel ID="pnlOQC" runat="server" GroupingText="OQC" Width="300px">
                                <asp:GridView ID="gvFOG" runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="Yield" HeaderText="良率" DataFormatString="{0:N2}">
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
                        <td align="center" style="background-color: #CCFFCC;" height="200px">Report under construction</td>
                        
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>
    
    
   
    </form>
</body>
</html>
