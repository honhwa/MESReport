<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModuleFPY.aspx.cs" Inherits="YMSReport_ModuleFPY" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.Common, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Dundas.Charting.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Module First Pass Yield Report</title>
        <style type="text/css">
        body { margin:0;}
     </style>
    <script type="text/javascript" language="javascript" src="popcalendar.js"></script>
    <script type="text/javascript" language="JavaScript">dateFormat='yyyy-mm-dd'</script>
<style type="text/css"> 
#winpop { width:200px; height:0px; position:absolute; right:0; bottom:0; border:1px solid #999999; margin:0; padding:1px; overflow:hidden;display:none; background:#FFFFFF} 
#winpop .title { width:100%; height:20px; line-height:20px; background:#FFCC00; font-weight:bold; text-align:center; font-size:12px;} 
#winpop .con { width:100%; height:80px; line-height:80px; font-weight:bold; font-size:12px; color:#FF0000; text-decoration:underline; text-align:center} 
#silu { font-size:13px; color:#999999; position:absolute; right:0;bottom:0px; text-align:right; text-decoration:underline; line-height:22px;} 
.close { position:absolute; right:4px; top:-1px; color:#FFFFFF; cursor:pointer} 
</style> 
<script type="text/javascript"> 
function show_pop()
{//显示窗口 
    document.getElementById("winpop").style.display="block"; 
    timer=setInterval("changeH(4)",2);//调用changeH(4),每0.002秒向上移动一次 
} 
function hid_pop()
{//隐藏窗口 
    timer=setInterval("changeH(-4)",2);//调用changeH(-4),每0.002秒向下移动一次 
} 
function changeH(addH) 
{ 
    var MsgPop=document.getElementById("winpop"); 
    var popH=parseInt(MsgPop.style.height||MsgPop.currentStyle.height);//用parseInt将对象的高度转化为数字,以方便下面比较（JS读<style>中的height要用"currentStyle.height"） 
    if (popH<=100&&addH>0||popH>=4&&addH<0)
    {//如果高度小于等于100(str>0)或高度大于等于4(str<0) 
        MsgPop.style.height=(popH+addH).toString()+"px";//高度增加或减少4个象素 
    } 
    else{//否则 
        clearInterval(timer);//取消调用,意思就是如果高度超过100象素了,就不再增长了，或高度等于0象素了，就不再减少了 
        MsgPop.style.display=addH>0?"block":"none"//向上移动时窗口显示,向下移动时窗口隐藏（因为窗口有边框,所以还是可以看见1~2象素没缩进去,这时候就把DIV隐藏掉） 
    } 
}
//window.onload=function()
//{//加载 
//    setTimeout("show_pop()",1000);//1秒后调用show_pop() 
//    setTimeout("hid_pop()",10000);//10秒后调用hid_pop() 
//} 

function RunFunc()
{//加载 
    setTimeout("show_pop()",1000);//1秒后调用show_pop() 
    setTimeout("hid_pop()",10000);//10秒后调用hid_pop() 
} 
</script>
</head>
<body style="font-weight: bold; font-size: 10pt; color: blue; font-family: Arial; background-image: url(images/CMI_ch1.jpg); background-color: transparent;">
    <form id="form1" runat="server">
<div id="winpop">
    <div class="title"><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label><span class="close" onclick="hid_pop()" style="right: 3px; width: 213px; top: 1px"></span></div> 
    <div class="con"><asp:TextBox ID="tbCon" runat="server" Height="80px" ReadOnly="True" TextMode="MultiLine" Width="198px"></asp:TextBox></div> 
</div>
        <asp:Panel ID="PMain" runat="server" Height="30px" Style="border-right: blue thin solid;
            border-top: blue thin solid; font-weight: bold; font-size: 10pt; border-left: blue thin solid;
            color: blue; line-height: 25pt; border-bottom: blue thin solid; font-family: Arial"
            Width="800px"><asp:DropDownList id="ddlDateType" runat="server" Width="60px" OnSelectedIndexChanged="ddlDateType_SelectedIndexChanged" AutoPostBack="True" Font-Bold="True">
                        <asp:ListItem Selected="True">Date</asp:ListItem>
                        <asp:ListItem>Week</asp:ListItem>
                        <asp:ListItem>Month</asp:ListItem>
                    </asp:DropDownList> From:<asp:TextBox id="txtFDate" runat="server" Width="80px" Font-Bold="True" MaxLength="10" Font-Size="10pt" Font-Names="Arial"></asp:TextBox><img alt="" id="imgF" onclick="popUpCalendar(this, form1.txtFDate, dateFormat,-1,-1,false)" height="17" src="../images/calendar.gif" width="30" runat="server" type="button" /><asp:DropDownList id="ddlFDate" runat="server" Width="115px" Font-Bold="True" Visible="False">
                    </asp:DropDownList>&nbsp; To:<asp:TextBox id="txtTDate" runat="server" Width="80px" Font-Bold="True" MaxLength="10" Font-Size="10pt" Font-Names="Arial"></asp:TextBox><img alt="" id="imgT" onclick="popUpCalendar(this, form1.txtTDate, dateFormat,-1,-1,false)" height="17" src="../images/calendar.gif" width="30" runat="server" type="button" /><asp:DropDownList id="ddlTDate" runat="server" Width="115px" Font-Bold="True" Visible="False">
                    </asp:DropDownList>&nbsp;<asp:DropDownList id="ddlYear" runat="server" Width="60px" Font-Bold="True" Enabled="False">
            </asp:DropDownList>&nbsp; <asp:CheckBox style="BORDER-TOP-WIDTH: thin; BORDER-LEFT-WIDTH: thin; BORDER-LEFT-COLOR: blue; BORDER-BOTTOM-WIDTH: thin; BORDER-BOTTOM-COLOR: blue; BORDER-TOP-COLOR: blue; BORDER-RIGHT-WIDTH: thin; BORDER-RIGHT-COLOR: blue" id="cbCPFlag" runat="server" Text="Cell2ProdID" Width="100px" Font-Bold="True" Font-Size="10pt" Font-Names="Arial" Visible="False" BackColor="#E0E0E0"></asp:CheckBox> Length:<asp:DropDownList id="ddlPdLen" runat="server" Width="40px" Font-Bold="True">
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem Selected="True">15</asp:ListItem>
                </asp:DropDownList> <asp:DropDownList id="ddlShiftID" runat="server" Width="75px" Font-Bold="True">
                <asp:ListItem>ShiftID</asp:ListItem>
                <asp:ListItem>A</asp:ListItem>
                <asp:ListItem>B</asp:ListItem>
            </asp:DropDownList> <asp:DropDownList id="ddlSelType" runat="server" Width="90px" Font-Bold="True" Visible="False">
                <asp:ListItem>MODULE</asp:ListItem>
                <asp:ListItem>MODULE+M1</asp:ListItem>
            </asp:DropDownList> <asp:CheckBox id="ChkMerge" runat="server" Text="Summary" AutoPostBack="True" OnCheckedChanged="ChkMerge_CheckedChanged"></asp:CheckBox><br /><table style="WIDTH: 561px"><tbody><tr><td style="WIDTH: 100px" valign="top" align="left"><asp:DropDownList id="ddlSubLineID" runat="server" Width="150px" OnSelectedIndexChanged="ddlSubLineID_SelectedIndexChanged" AutoPostBack="True" Font-Bold="True">
                <asp:ListItem>SubLineID(ALL)</asp:ListItem>
                <asp:ListItem>UserDefine</asp:ListItem>
            </asp:DropDownList></td><td style="WIDTH: 100px" valign="top" align="left"><asp:DropDownList id="ddlLotType" runat="server" Width="110px" OnSelectedIndexChanged="ddlLotType_SelectedIndexChanged" AutoPostBack="True" Font-Bold="True">
                <asp:ListItem>LotType(ALL)</asp:ListItem>
                <asp:ListItem>UserDefine</asp:ListItem>
            </asp:DropDownList></td><td style="WIDTH: 100px" valign="top" align="left"><asp:DropDownList id="ddlProdID" runat="server" Width="170px" OnSelectedIndexChanged="ddlProdID_SelectedIndexChanged" AutoPostBack="True" Font-Bold="True">
                <asp:ListItem>ProdID(ALL)</asp:ListItem>
                <asp:ListItem>UserDefine</asp:ListItem>
            </asp:DropDownList></td><td valign="top" align="left"><asp:DropDownList id="ddlWonbr" runat="server" Width="140px" OnSelectedIndexChanged="ddlWonbr_SelectedIndexChanged" AutoPostBack="True" Font-Bold="True">
                <asp:ListItem>Wonbr(ALL)</asp:ListItem>
                <asp:ListItem>UserDefine</asp:ListItem>
            </asp:DropDownList></td><td style="WIDTH: 3px" valign="top" align="left"><asp:DropDownList id="ddlGCraft" runat="server" Width="140px" OnSelectedIndexChanged="ddlGCraft_SelectedIndexChanged" AutoPostBack="True" Font-Bold="True" Font-Size="10pt" Font-Names="Arial"><asp:ListItem Value="0">CRAFT(ALL)</asp:ListItem>
<asp:ListItem Value="1">TN</asp:ListItem>
<asp:ListItem Value="2">T3</asp:ListItem>
<asp:ListItem Value="3">TN FAB3</asp:ListItem>
<asp:ListItem Value="4">TN FAB5</asp:ListItem>
</asp:DropDownList></td><td style="WIDTH: 3px" valign="top" align="left"></td><td style="WIDTH: 3px" valign="top" align="left"><asp:Label id="lblTTime" runat="server" Width="150px" Height="1px" Font-Bold="True" Font-Size="10pt" Font-Names="Arial" ForeColor="Green"></asp:Label></td></tr><tr><td style="WIDTH: 100px; HEIGHT: 120px"><asp:Panel id="PSubLineID" runat="server" Width="150px" Height="120px" Visible="False" ScrollBars="Vertical">
                            <asp:CheckBoxList ID="cblSubLineID" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" RepeatLayout="Flow" style="line-height: normal" RepeatColumns="1">
                            </asp:CheckBoxList></asp:Panel> </td><td style="WIDTH: 100px; HEIGHT: 120px"><asp:Panel id="PLotType" runat="server" Width="110px" Height="120px" Visible="False" ScrollBars="Vertical">
                            <asp:CheckBoxList ID="cblLotType" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" RepeatLayout="Flow"  style="line-height: normal" RepeatColumns="1">
                            </asp:CheckBoxList></asp:Panel> </td><td style="WIDTH: 100px; HEIGHT: 120px"><asp:Panel id="PProdID" runat="server" Width="170px" Height="120px" Visible="False" ScrollBars="Vertical">
                            <asp:CheckBoxList ID="cblProdID" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" RepeatLayout="Flow" style="line-height: normal">
                        </asp:CheckBoxList></asp:Panel> </td><td style="HEIGHT: 120px"><asp:Panel id="PWonbr" runat="server" Width="140px" Height="120px" Visible="False" ScrollBars="Vertical">
                            <asp:CheckBoxList ID="cblWonbr" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" RepeatLayout="Flow" style="line-height: normal">
                        </asp:CheckBoxList></asp:Panel> </td><td style="WIDTH: 3px; HEIGHT: 120px"><asp:Panel id="PGCraft" runat="server" Width="140px" Height="120px" Visible="False" ForeColor="Black" ScrollBars="Vertical">
                            <asp:CheckBoxList ID="cblSoFab" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Arial" Font-Size="10pt" ForeColor="Blue" OnSelectedIndexChanged="cblSoFab_SelectedIndexChanged"
                                RepeatLayout="Flow" Style="line-height: normal">
                            </asp:CheckBoxList>
                            <br />
                            ==LCD Craft==<br />
                            <asp:CheckBoxList ID="cblGCraft" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="10pt" ForeColor="Blue" RepeatLayout="Flow" Style="line-height: normal">
                            </asp:CheckBoxList></asp:Panel> </td><td style="WIDTH: 3px; HEIGHT: 120px"></td><td style="WIDTH: 3px; HEIGHT: 120px"></td></tr></tbody></table><asp:TextBox id="tbFABID" runat="server" Width="50px" Visible="False"></asp:TextBox><asp:TextBox id="tbFABName" runat="server" Width="50px" Visible="False"></asp:TextBox><asp:TextBox id="tbLoginNTName" runat="server" Width="50px" Visible="False"></asp:TextBox><asp:TextBox id="tbLoginPCName" runat="server" Width="50px" Visible="False"></asp:TextBox><asp:TextBox id="tbLoginPCIP" runat="server" Width="50px" Visible="False"></asp:TextBox><asp:TextBox id="tbDateType" runat="server" Width="50px" Visible="False"></asp:TextBox><asp:TextBox id="tbCPFlag" runat="server" Width="50px" Visible="False"></asp:TextBox><asp:TextBox id="tbSqlDate" runat="server" Width="50px" Visible="False"></asp:TextBox><asp:TextBox id="tbSqlWhere" runat="server" Width="50px" Visible="False"></asp:TextBox><asp:TextBox id="tbFDate" runat="server" Width="25px" Visible="False"></asp:TextBox><asp:TextBox id="tbTDate" runat="server" Width="25px" Visible="False"></asp:TextBox><asp:TextBox id="tbDiffDate" runat="server" Width="25px" Visible="False"></asp:TextBox></asp:Panel><div>
            <table style="width: 803px; height: 20px">
                <tr>
                    <td valign="top">
        <asp:Button ID="btQuery" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Text="Query" OnClick="btQuery_Click" />
        <asp:Button ID="btExport" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Text="ExportExcel" OnClick="ExportExcel_Click" />
            <asp:Button ID="btClear" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Text="Clear" OnClick="btClear_Click" />&nbsp;
                        <asp:Label ID="lblFabID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            ForeColor="Red" Width="60px"></asp:Label></td>
                    <td align="right" valign="top">
                        <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ToolTip="返回主頁面">Main Menu</asp:HyperLink></td>
                        <td align="right" style="width: 5px" valign="top">
                            <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="10pt" NavigateUrl="~/Description/ModuleFPY.htm" Target="_blank" ToolTip="報表說明">Help</asp:HyperLink></td>
                </tr>
                <tr>
                    <td colspan="3">
                        -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------</td>
                </tr>
                <tr>
                    <td colspan="3">
                <asp:CheckBoxList ID="cblLevel" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" RepeatLayout="Flow" style="line-height: normal" AutoPostBack="True" ForeColor="Blue" OnSelectedIndexChanged="cblLevel_SelectedIndexChanged" RepeatDirection="Horizontal" BackColor="#E0E0E0" Enabled="False">
                    <asp:ListItem Selected="True">A</asp:ListItem>
                    <asp:ListItem>S</asp:ListItem>
                </asp:CheckBoxList></td>
                </tr>
            </table>
            --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        <asp:Panel ID="Panel1" runat="server" Style="border-right: blue thin solid;
            border-top: blue thin solid; border-left: blue thin solid; border-bottom: blue thin solid; font-weight: bold; font-size: 10pt; color: black; font-family: Arial;"
            Width="800px">
            Group By ProdID:<asp:Label ID="lblDataRunTime" runat="server" Font-Bold="True" Font-Names="Arial"
                Font-Size="8pt" ForeColor="Blue" Style="text-align: right" Width="670px"></asp:Label><br />
            <asp:Panel ID="Panel4" runat="server" Width="800px">
                <DCWC:Chart ID="ChartAll" runat="server" Palette="Pastel" Width="800px" Visible="False" BackColor="" Height="400px" OnClick="ChartAll_Click">
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
                        <DCWC:ChartArea Name="Default">
                        </DCWC:ChartArea>
                    </ChartAreas>
                </DCWC:Chart>
            </asp:Panel>
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
            Group By Station:<asp:DropDownList ID="ddlByProdID" runat="server" Font-Bold="True" Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddlByProdID_SelectedIndexChanged">
            </asp:DropDownList><asp:Label ID="lblTime01" runat="server" Font-Bold="True" Font-Names="Arial"
                Font-Size="10pt" ForeColor="Green" Width="150px"></asp:Label><asp:Panel ID="Panel2" runat="server" Width="800px"><DCWC:Chart ID="ChartProdID" runat="server" Palette="Pastel" Width="800px" Visible="False" BackColor="" Height="400px" OnClick="ChartProdID_Click">
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
                    <DCWC:ChartArea Name="Default">
                    </DCWC:ChartArea>
                </ChartAreas>
            </DCWC:Chart>
            </asp:Panel>
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
            Group By Time:<br />
            <asp:Panel ID="Panel3" runat="server" Width="800px">
                <DCWC:Chart ID="ChartDT" runat="server" Palette="Pastel" Width="800px" Visible="False" BackColor="" Height="400px" OnClick="ChartDT_Click">
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
                        <DCWC:ChartArea Name="Default">
                        </DCWC:ChartArea>
                    </ChartAreas>
                </DCWC:Chart>
            </asp:Panel>
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
            DefectCode By:<asp:RadioButtonList AutoPostBack="True" Enabled="False" ID="RadioButtonList1" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" >
                <asp:ListItem Selected="True">DefectCode</asp:ListItem>
            </asp:RadioButtonList><br />
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
            S Grade DefectCode Summary:<br />
            <asp:Panel ID="Panel5" runat="server" Width="800px">
                <DCWC:Chart ID="ChartSDefectCode" runat="server" Palette="Pastel" Width="800px" Visible="False" BackColor="" Height="400px" OnClick="ChartSDefectCode_Click">
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
                        <DCWC:ChartArea Name="Default">
                        </DCWC:ChartArea>
                    </ChartAreas>
                </DCWC:Chart>
            </asp:Panel>
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
            S Grade DefectCode Distribution:<asp:DropDownList ID="ddlSDefList" runat="server" Font-Bold="True" Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddlSDefList_SelectedIndexChanged">
            </asp:DropDownList><asp:Label ID="lblTime02" runat="server" Font-Bold="True" Font-Names="Arial"
                Font-Size="10pt" ForeColor="Green" Width="150px"></asp:Label><br />
            <asp:Panel ID="Panel8" runat="server" Width="800px">
                <DCWC:Chart ID="ChartSDefectDistr" runat="server" Palette="Pastel" Width="800px" Visible="False" BackColor="" Height="400px" OnClick="ChartSDefectDistr_Click">
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
                        <DCWC:ChartArea Name="Default">
                        </DCWC:ChartArea>
                    </ChartAreas>
                </DCWC:Chart>
            </asp:Panel>
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
                <asp:ImageButton ID="IBGBYData" runat="server" Height="9px" ImageUrl="~/images/menu_icon.gif"
                    Width="9px" OnClick="IBGBYData_Click" ToolTip="點擊展開數據表" Enabled="False" /><br />
            <asp:Panel ID="PTotal" runat="server" Height="150px" Visible="False" Width="800px">
            <asp:Panel ID="P1" runat="server" Height="150px" Width="800px" ScrollBars="Auto">
                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" Font-Bold="False" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="#333333" GridLines="None" Width="790px" AutoGenerateColumns="False" ShowFooter="True">
                            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="White" Height="40px" /><Columns>
                                <asp:BoundField DataField="Product_ID" HeaderText="產品號碼">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OPERATION_ID" HeaderText="站點">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Chip_Qty" HeaderText="Chip_Qty">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="A_Yield" HeaderText="A_Rate">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="S_Yield" HeaderText="S_Rate">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
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
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
            <asp:Panel ID="P2" runat="server" Height="150px" Width="800px" ScrollBars="Auto">
                <asp:GridView ID="GridView2" runat="server" CellPadding="4" Font-Bold="False" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="#333333" GridLines="None" Width="790px" AutoGenerateColumns="False" ShowFooter="True">
                    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="White" Height="40px" />
                    <Columns>
                        <asp:BoundField DataField="Product_ID" HeaderText="產品號碼">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OPERATION_ID" HeaderText="站點">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Chip_Qty" HeaderText="Chip_Qty">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="A_Yield" HeaderText="A_Rate">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S_Yield" HeaderText="S_Rate">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
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
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
            <asp:Panel ID="P3" runat="server" Height="150px" Width="800px" ScrollBars="Auto">
                &nbsp;<asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    Font-Bold="False" Font-Names="Arial" Font-Size="10pt" ForeColor="#333333" GridLines="None"
                    Width="790px" ShowFooter="True">
                    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="White" Height="40px" />
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="ReportDT" HeaderText="日期">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_ID" HeaderText="產品號碼">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OPERATION_ID" HeaderText="站點">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Chip_Qty" HeaderText="Chip_Qty">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="A_Yield" HeaderText="A_Rate">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S_Yield" HeaderText="S_Rate">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
            <asp:Panel ID="P4" runat="server" Height="150px" Width="800px" ScrollBars="Auto">
                <asp:GridView ID="GridView4" runat="server" CellPadding="4" Font-Bold="False" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="#333333" GridLines="None" Width="790px" AutoGenerateColumns="False" ShowFooter="True">
                    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="White" Height="40px" /><Columns>
                        <asp:BoundField DataField="ReportDT" HeaderText="日期" Visible="False">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_ID" HeaderText="產品號碼">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LOTGRADE" HeaderText="等級">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                            <asp:BoundField DataField="DEFCODE" HeaderText="不良代碼">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DEFECTDESC" HeaderText="不良代碼描述">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField DataField="DefectRate" HeaderText="DefectRate">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TTL_CNT" HeaderText="TTL_CNT">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8180" HeaderText="M1">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField DataField="S8210" HeaderText="M2-COG">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8220" HeaderText="M2-COF">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8230" HeaderText="M2-FPC">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8234" HeaderText="FOG">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8240" HeaderText="TOD">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8250" HeaderText="M2-FOB">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8260" HeaderText="OCA Atta">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8270" HeaderText="HalfModuleTest(8270)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8280" HeaderText="HalfModuleTest(8280)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8290" HeaderText="Lamination(8290)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>

                        <asp:BoundField DataField="S8285" HeaderText="Lamination(8285)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8292" HeaderText="Lamination(8292)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8294" HeaderText="Lamination(8294)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8296" HeaderText="Lamination(8296)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>

                        <asp:BoundField DataField="S829X" HeaderText="M2.5(829X)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8297" HeaderText="M2.5(8297)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8298" HeaderText="M2.5(8298)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8299" HeaderText="M2.5(8299)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>

                        <asp:BoundField DataField="S8300" HeaderText="AssemblyReceived">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8305" HeaderText="VIA">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8350" HeaderText="Assembly">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                         <asp:BoundField DataField="S8351" HeaderText="Burn-In">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                         <asp:BoundField DataField="S8352" HeaderText="Baking">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                         <asp:BoundField DataField="S8353" HeaderText="Module Test">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8355" HeaderText="AssemblyResorting">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8360" HeaderText="OQC">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8380" HeaderText="Packing">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
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
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />
            <asp:Panel ID="P5" runat="server" Height="150px" Width="800px" ScrollBars="Auto">
                <asp:GridView ID="GridView5" runat="server" CellPadding="4" Font-Bold="False" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="#333333" GridLines="None" Width="790px" AutoGenerateColumns="False" ShowFooter="True">
                    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="White" Height="40px" />
                    <Columns>
                        <asp:BoundField DataField="ReportDT" HeaderText="日期">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_ID" HeaderText="產品號碼">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LOTGRADE" HeaderText="等級">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                            <asp:BoundField DataField="DEFCODE" HeaderText="不良代碼">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DEFECTDESC" HeaderText="不良代碼描述">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField DataField="DefectRate" HeaderText="DefectRate">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TTL_CNT" HeaderText="TTL_CNT">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                                                <asp:BoundField DataField="S8180" HeaderText="M1" >
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                        <asp:BoundField DataField="S8210" HeaderText="M2-COG">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8220" HeaderText="M2-COF">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8230" HeaderText="M2-FPC">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8250" HeaderText="M2-FOB">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8260" HeaderText="OCA Atta">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8270" HeaderText="HalfModuleTest(8270)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8280" HeaderText="HalfModuleTest(8280)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8290" HeaderText="Lamination(8290)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>

                        <asp:BoundField DataField="S8285" HeaderText="Lamination(8285)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8292" HeaderText="Lamination(8292)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8294" HeaderText="Lamination(8294)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8296" HeaderText="Lamination(8296)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>

                        <asp:BoundField DataField="S829X" HeaderText="M2.5(829X)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8297" HeaderText="M2.5(8297)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8298" HeaderText="M2.5(8298)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8299" HeaderText="M2.5(8299)">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>

                        <asp:BoundField DataField="S8300" HeaderText="AssemblyReceived">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8350" HeaderText="Assembly">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                         <asp:BoundField DataField="S8351" HeaderText="Burn-In">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                         <asp:BoundField DataField="S8352" HeaderText="Baking">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                         <asp:BoundField DataField="S8353" HeaderText="Module Test">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8355" HeaderText="AssemblyResorting">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8360" HeaderText="OQC">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="S8380" HeaderText="Packing">
                            <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
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
            </asp:Panel>
            -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------</asp:Panel></div>
    </form>
</body>
</html>
