<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InputOutpuDefect.aspx.cs" Inherits="YMSReport_InputOutpuDefect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Microsoft.ReportViewer.Common, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Dundas.Charting.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    <meta http-equiv="Content-Type" content="text/html; "/>
</head>
<body>
    <form id="form1" runat="server">
    <table id="tblcontrol" runat ="server" width="100%">
    <tr >
    <td colspan ="6" align="center"   style =" font-size:medium; font-family:Arial ;  font-weight :bolder; background-color:Silver"  >Yield &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;Report</td>
    
    </tr>
    <tr >
    <td colspan ="6" align="center"  > &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;</td>
    
    </tr>
    <tr>
    <td style="width:10%" align="right" >
                     <asp:Label  ID='lblyear' runat ="server" >Year：</asp:Label>
                     </td>
                     <td style="width:10%">
                     <asp:DropDownList id="ddlYear" runat="server" Width="95%" Font-Bold="True" Visible="true" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                    </asp:DropDownList>
                    </td>
                   
                    <td style="height: 24px;width:10%"  align="right">
                    <asp:Label ID='lbltype' runat ="server" >Type：</asp:Label>
                    </td>
                     <td style="height: 24px;width:10%">                   
                   
                   <%-- <asp:DropDownList ID="ddlDateType" runat="server" Font-Bold="True" Width="95%" OnSelectedIndexChanged="ddlDateType_SelectedIndexChanged1" >
                        <asp:ListItem Text="Week" Value="W"></asp:ListItem>
                        <asp:ListItem Text="Month" Value="W"></asp:ListItem>
                    </asp:DropDownList>--%>
                    <asp:DropDownList ID="ddltype" runat ="server" OnSelectedIndexChanged="ddltype_SelectedIndexChanged"  Width="95%" AutoPostBack="True">
                         <asp:ListItem Text="Week" Value="W"></asp:ListItem>
                          <asp:ListItem Text="Month" Value="M"></asp:ListItem>
                    </asp:DropDownList>
                     </td>
                     <td style="height: 24px;width:10%" align="right">
                    <asp:Label ID='lbl' runat ="server" >Week/Month：</asp:Label>
                    </td>
                    <td style="height: 24px;width:20%">
                    <asp:DropDownList id="ddlWD" runat="server" Width="95%" Font-Bold="True" Visible="true">
                    
                    </asp:DropDownList>
                    </td>
                      </tr>
                      <tr>
                      <td style ="width:10%" align="right">
                     <asp:Label ID='lblprod' runat ="server" >Product：</asp:Label>
                     </td>
                     <td  colspan ="1" style ="width:200px; height :20px">
                     
                           <asp:DropDownList ID="ddlprodtype" runat ="server" Width="146px" AutoPostBack="True" OnSelectedIndexChanged="ddlprodtype_SelectedIndexChanged" >
                           <asp:ListItem Text="ALL" Value ="ALL" ></asp:ListItem>
                           <asp:ListItem Text="Product" Value ="Product" ></asp:ListItem>
                           </asp:DropDownList><br />
                           <asp:Panel ID="pnl" runat ="server" Height="100px" Width="200px" ScrollBars ="Vertical" Visible="False" >
                           <asp:CheckBoxList ID="cblprod" runat="server" Height="85px" RepeatColumns="1" RepeatLayout="Flow">
                            <asp:ListItem Text="ALL" Value ="ALL" ></asp:ListItem>
                           </asp:CheckBoxList>
                     </asp:Panel>
                    </td>
                          <td style="width: 101px" >
                              <asp:DropDownList ID="ddlGCraft" runat="server" AutoPostBack="True" Font-Bold="True"
                                  Font-Names="Arial" Font-Size="10pt" OnSelectedIndexChanged="ddlGCraft_SelectedIndexChanged"
                                  Width="140px">
                                  <asp:ListItem Value="0">CRAFT(ALL)</asp:ListItem>
                                  <asp:ListItem Value="1">TN</asp:ListItem>
                                  <asp:ListItem Value="2">T3</asp:ListItem>
                              </asp:DropDownList>
                              <asp:Panel ID="PGCraft" runat="server" ForeColor="Black" Height="120px" ScrollBars="Vertical"
                                  Visible="False" Width="140px">
                                  <asp:CheckBoxList ID="cblSoFab" runat="server" AutoPostBack="True" Font-Bold="True"
                                      Font-Names="Arial" Font-Size="10pt" ForeColor="Blue" OnSelectedIndexChanged="cblSoFab_SelectedIndexChanged"
                                      RepeatLayout="Flow" Style="line-height: normal">
                                  </asp:CheckBoxList>
                                  <br />
                                  ==LCD Craft==<br />
                                  <asp:CheckBoxList ID="cblGCraft" runat="server" Font-Bold="True" Font-Names="Arial"
                                      Font-Size="10pt" ForeColor="Blue" RepeatLayout="Flow" Style="line-height: normal">
                                  </asp:CheckBoxList></asp:Panel>
                          </td>
                    <td colspan ="1"> <asp:Button ID="btnquery" Text="Query"  runat ="server" OnClick="btnquery_Click" /></td>
                    <td colspan ="1"> <asp:Button ID="btnfresh" Text="Fresh"  runat ="server" OnClick="btnfresh_Click"  /></td>
                    <td colspan ="1"> <asp:Button ID="btnexport" Text="Export Excel"  runat ="server" OnClick="btnexport_Click"   /></td>
                     <td valign="bottom" ><asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ToolTip="返回主頁面">Main Menu</asp:HyperLink>
                     <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="10pt" NavigateUrl="~/Description/InputOutputDefect.htm" Target="_blank" ToolTip="報表說明">Help</asp:HyperLink>
                     </td>
                    </tr>
                    
                    </table>
                     <asp:TextBox ID="tbFABID" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                ID="tbFABName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                    ID="tbLoginNTName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                        ID="tbLoginPCName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                            ID="tbLoginPCIP" runat="server" Visible="False" Width="50px"></asp:TextBox>
                      <asp:Label ID="lblerror" Width="100%" runat ="server"  ForeColor ="red"  Text=""></asp:Label> 
                    <asp:Panel ID="PNLMAIN" runat ="server" Width ="100%">
                    
                 
        <asp:Panel ID="Panel4" runat="server" Width="100%">        
                <DCWC:Chart ID="ChartAll" runat="server"  Palette="Pastel" Width="1000px" Visible="False" BackColor="Cornsilk" Height="400px">
                    <Legends>
                        <DCWC:Legend Name="Default" Alignment="Center" Docking="Bottom" LegendStyle="Row">
                        </DCWC:Legend>
                    </Legends>
                    <Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series1" ShadowOffset="1">
                        </DCWC:Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series2" ShadowOffset="1">
                        </DCWC:Series>
                       
                    </Series>
                    <ChartAreas>
                        <DCWC:ChartArea Name="Default" BorderColor="White">
                            <AxisX2 Enabled="False">
                            </AxisX2>
                            <AxisY>
                                <MajorGrid Enabled="False" />
                            </AxisY>
                            <AxisX>
                                <MajorGrid Enabled="False" />
                            </AxisX>
                        </DCWC:ChartArea>
                    </ChartAreas>
                    <Titles>
                    <DCWC:Title Name="Default Title">
                    </DCWC:Title>
                    </Titles>
                    <UI>
                        <Toolbar Enabled="True">
                        </Toolbar>
                    </UI>
                </DCWC:Chart>
            </asp:Panel>
    <div id="divmain" runat ="server" style ="width:100%">
        <asp:GridView id="gvmain" runat ='server' Width="100%" OnRowDataBound="gvmain_RowDataBound">
        </asp:GridView>
    </div>
    <asp:Label  ID="lbl40" Width ="100%" runat ="server" BackColor ="Goldenrod" Visible="False" >1.Cell2 </asp:Label>
    <div id="div4" runat ="server" style ="width:100%">
    <asp:Panel ID="Panel5" runat="server" Width="100%">        
                <DCWC:Chart ID="Chart1" runat="server" Palette="Pastel" Width="1000px" Visible="False" BackColor="Cornsilk" Height="400px">
                    <Legends>
                        <DCWC:Legend Name="Default" Alignment="Center" Docking="Bottom">
                        </DCWC:Legend>
                    </Legends>
                    <Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series1" ShadowOffset="1">
                        </DCWC:Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series2" ShadowOffset="1">
                        </DCWC:Series>
                    </Series>
                    <ChartAreas>
                        <DCWC:ChartArea Name="Default" BorderColor="White">
                            <AxisX2 Enabled="False">
                            </AxisX2>
                            <AxisY>
                                <MajorGrid Enabled="False" />
                            </AxisY>
                            <AxisX>
                                <MajorGrid Enabled="False" />
                            </AxisX>
                        </DCWC:ChartArea>
                    </ChartAreas>
                    <Titles>
                    <DCWC:Title Name="Default Title">
                    </DCWC:Title>
                    </Titles>
                    <UI>
                        <Toolbar Enabled="True">
                        </Toolbar>
                    </UI>
                </DCWC:Chart>
            </asp:Panel>
        <asp:GridView id="gv_d_cell2" runat ='server' Width="100%" OnRowDataBound="gv_d_cell2_RowDataBound" OnSelectedIndexChanged="gv_d_cell2_SelectedIndexChanged"  >
        </asp:GridView>
    </div>
    <asp:Label  ID="LBL50" Width ="100%" runat ="server" BackColor ="Goldenrod" Visible="False" >2.Finishing </asp:Label>
    <div id="div5" runat ="server" style ="width:100%">
    <asp:Panel ID="Panel6" runat="server" Width="100%">        
                <DCWC:Chart ID="Chart2" runat="server" Palette="Pastel" Width="1000px" Visible="False" BackColor="Cornsilk" Height="400px">
                    <Legends>
                        <DCWC:Legend Name="Default" Alignment="Center" Docking="Bottom">
                        </DCWC:Legend>
                    </Legends>
                    <Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series1" ShadowOffset="1">
                        </DCWC:Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series2" ShadowOffset="1">
                        </DCWC:Series>
                    </Series>
                    <ChartAreas>
                        <DCWC:ChartArea Name="Default" BorderColor="White">
                            <AxisX2 Enabled="False">
                            </AxisX2>
                            <AxisY>
                                <MajorGrid Enabled="False" />
                            </AxisY>
                            <AxisX>
                                <MajorGrid Enabled="False" />
                            </AxisX>
                        </DCWC:ChartArea>
                    </ChartAreas>
                    <Titles>
                    <DCWC:Title Name="Default Title">
                    </DCWC:Title>
                    </Titles>
                    <UI>
                        <Toolbar Enabled="True">
                        </Toolbar>
                    </UI>
                </DCWC:Chart>
            </asp:Panel>
        <asp:GridView id="gv_d_m2" runat ='server' Width="100%" OnRowDataBound="gv_d_m2_RowDataBound"   >
        </asp:GridView>
    </div>
    <asp:Label  ID="lbl60" Width ="100%" runat ="server" BackColor ="Goldenrod" Visible="False" >3.Assembly </asp:Label>
    <div id="div6" runat ="server" style ="width:100%">
    <asp:Panel ID="Panel7" runat="server" Width="100%">        
                <DCWC:Chart ID="Chart3" runat="server" Palette="Pastel" Width="1000px" Visible="False" BackColor="Cornsilk" Height="400px">
                    <Legends>
                        <DCWC:Legend Name="Default" Alignment="Center" Docking="Bottom">
                        </DCWC:Legend>
                    </Legends>
                    <Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series1" ShadowOffset="1">
                        </DCWC:Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series2" ShadowOffset="1">
                        </DCWC:Series>
                    </Series>
                    <ChartAreas>
                        <DCWC:ChartArea Name="Default" BorderColor="White">
                            <AxisX2 Enabled="False">
                            </AxisX2>
                            <AxisY>
                                <MajorGrid Enabled="False" />
                            </AxisY>
                            <AxisX>
                                <MajorGrid Enabled="False" />
                            </AxisX>
                        </DCWC:ChartArea>
                    </ChartAreas>
                    <Titles>
                    <DCWC:Title Name="Default Title">
                    </DCWC:Title>
                    </Titles>
                    <UI>
                        <Toolbar Enabled="True">
                        </Toolbar>
                    </UI>
                </DCWC:Chart>
            </asp:Panel>
        <asp:GridView id="gv_d_m3" runat ='server' Width="100%" OnRowDataBound="gv_d_m3_RowDataBound"   >
        </asp:GridView>
    </div>
    <asp:Label  ID="lbl10" Width ="100%" runat ="server" BackColor ="Goldenrod" Visible="False" >Cell2 Yield Loss Trend Chart By Defect</asp:Label>
     <asp:Panel ID="Panel1" runat="server" Width="100%">        
                <DCWC:Chart ID="Chartcell2_D" runat="server" Palette="Pastel" Width="1000px" Visible="False" BackColor="Cornsilk" Height="400px">
                    <Legends>
                        <DCWC:Legend Name="Default" Alignment="Center" Docking="Bottom">
                        </DCWC:Legend>
                    </Legends>
                    <Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series1" ShadowOffset="1">
                        </DCWC:Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series2" ShadowOffset="1">
                        </DCWC:Series>
                    </Series>
                    <ChartAreas>
                        <DCWC:ChartArea Name="Default" BorderColor="White">
                            <AxisX2 Enabled="False">
                            </AxisX2>
                            <AxisY>
                                <MajorGrid Enabled="False" />
                            </AxisY>
                            <AxisX>
                                <MajorGrid Enabled="False" />
                            </AxisX>
                        </DCWC:ChartArea>
                    </ChartAreas>
                    <Titles>
                    <DCWC:Title Name="Default Title">
                    </DCWC:Title>
                    </Titles>
                    <UI>
                        <Toolbar Enabled="True">
                        </Toolbar>
                    </UI>
                </DCWC:Chart>
            </asp:Panel>
    <div id="div1" runat ="server" style ="width:100%">
        <asp:GridView id="gvcell2_d" runat ='server' Width="100%" OnRowDataBound="gvcell2_d_RowDataBound">
        </asp:GridView>
    </div>
    <asp:Label ID="lbl20"  BackColor ="Goldenrod" Width ="100%"  Visible="false"  runat='server'>Module(finishing) Yield Loss Trend Chart By Defect</asp:Label>
    <asp:Panel ID="Panel2" runat="server" Width="100%">        
                <DCWC:Chart ID="chartfini_d" runat="server" Palette="Pastel" Width="1000px" Visible="False" BackColor="Cornsilk" Height="400px">
                    <Legends>
                        <DCWC:Legend Name="Default" Alignment="Center" Docking="Bottom">
                        </DCWC:Legend>
                    </Legends>
                    <Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series1" ShadowOffset="1">
                        </DCWC:Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series2" ShadowOffset="1">
                        </DCWC:Series>
                    </Series>
                    <ChartAreas>
                        <DCWC:ChartArea Name="Default" BorderColor="White">
                            <AxisX2 Enabled="False">
                            </AxisX2>
                            <AxisY>
                                <MajorGrid Enabled="False" />
                            </AxisY>
                            <AxisX>
                                <MajorGrid Enabled="False" />
                            </AxisX>
                        </DCWC:ChartArea>
                    </ChartAreas>
                    <Titles>
                    <DCWC:Title Name="Default Title">
                    </DCWC:Title>
                    </Titles>
                    <UI>
                        <Toolbar Enabled="True">
                        </Toolbar>
                    </UI>
                </DCWC:Chart>
            </asp:Panel>
    <div id="div2" runat ="server" style ="width:100%">
        <asp:GridView id="gvfini_d" runat ='server' Width="100%" OnRowDataBound="gvfini_d_RowDataBound" OnSelectedIndexChanged="gvfini_d_SelectedIndexChanged" >
        </asp:GridView>
    </div>
    <asp:Label ID="lbl30"  BackColor ="Goldenrod"  Width ="100%" Visible="false"  runat='server'>Module(assembly) Yield Loss Trend Chart By Defect</asp:Label>
    <asp:Panel ID="Panel3" runat="server" Width="100%">        
                <DCWC:Chart ID="chartassy_d" runat="server" Palette="Pastel" Width="1000px" Visible="False" BackColor="Cornsilk" Height="400px">
                    <Legends>
                        <DCWC:Legend Name="Default" Alignment="Center" Docking="Bottom">
                        </DCWC:Legend>
                    </Legends>
                    <Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series1" ShadowOffset="1">
                        </DCWC:Series>
                        <DCWC:Series BorderColor="64, 64, 64" Name="Series2" ShadowOffset="1">
                        </DCWC:Series>
                    </Series>
                    <ChartAreas>
                        <DCWC:ChartArea Name="Default" BorderColor="White">
                            <AxisX2 Enabled="False">
                            </AxisX2>
                            <AxisY>
                                <MajorGrid Enabled="False" />
                            </AxisY>
                            <AxisX>
                                <MajorGrid Enabled="False" />
                            </AxisX>
                        </DCWC:ChartArea>
                    </ChartAreas>
                    <Titles>
                    <DCWC:Title Name="Default Title">
                    </DCWC:Title>
                    </Titles>
                    <UI>
                        <Toolbar Enabled="True">
                        </Toolbar>
                    </UI>
                </DCWC:Chart>
            </asp:Panel>
     <div id="div3" runat ="server" style ="width:100%">
        <asp:GridView id="gvassy_d" runat ='server' Width="100%" OnRowDataBound="gvassy_d_RowDataBound"  >
        </asp:GridView>
    </div>
    </asp:Panel>
    </form>
</body>
</html>
