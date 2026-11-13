<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnLineReport.aspx.cs" Inherits="WebReport.OnLineReport" %>

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
    <asp:Timer ID="TmrFinish" runat="server" Interval="110000" 
        ontick="TmrFinish_Tick">
    </asp:Timer>
    <table width="1024px">
        <tr>
            <td>
                <table width="70%">
                   <tr>
                       <td colspan="2" align="center" style="background-color: #99CCFF">
                           <asp:Label ID="lblTitle" runat="server" Text="一條龍生產資訊看板(機種)" Font-Bold="True" Font-Names="新細明體"
                               Width="600px" Font-Size="X-Large"></asp:Label>
                       </td>
                   </tr>   
                   <tr>
                     <td width="50%" >
                    <div id="left" align="left">
                        <asp:Chart ID="crtAccumulate" runat="server" Align="left" Width="512px" Height="400px">
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
                     <td width="50%" >
                      <div id="right">
                        <asp:Chart ID="crtPerHour" runat="server" Width="512px" Height="400px">
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
                    <tr>               
                        <td width="1024px" colspan="2">
                            <asp:Image ID="imgFinishYield" runat="server" ImageUrl="~/image/Finishyield.jpg"
                                Width="1024px" EnableViewState="False"></asp:Image>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" >
            <font style="font-family: Arial, Helvetica, sans-serif; font-weight: bold">Updated Time:<asp:Label
                    ID="lblupdateTime" runat="server" Text="Label"></asp:Label></font>
            <asp:Panel ID="pnlPOL" runat="server" GroupingText="Polar" Width="300px">
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
             <asp:Panel ID="pnlCog" runat="server" GroupingText="COG" Width="300px">
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
             <asp:Panel ID="pnlFOG" runat="server" GroupingText="FOG" Width="300px">
                    <asp:GridView ID="gvFOG" runat="server" AutoGenerateColumns="False">
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
             <asp:Panel ID="PnlLot2" runat="server" GroupingText="LOT2" Width="300px">
                    <asp:GridView ID="gvLot2" runat="server" AutoGenerateColumns="False">
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
        </tr>
    </table>
    
    
   
    </form>
</body>
</html>

