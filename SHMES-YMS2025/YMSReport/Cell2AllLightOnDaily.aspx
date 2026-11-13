<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cell2AllLightOnDaily.aspx.cs" Inherits="YMSReport_Cell2AllLightOnDaily" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.Common, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Dundas.Charting.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>BEOL All Light On Daily Yield</title>
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
        <asp:Panel ID="PMain" runat="server" Height="50px" Style="border-right: blue thin solid;
            border-top: blue thin solid; font-weight: bold; font-size: 10pt; border-left: blue thin solid;
            color: blue; line-height: 25pt; border-bottom: blue thin solid; font-family: Arial"
            Width="800px">
            Station:<asp:DropDownList ID="ddlStation" runat="server" Font-Bold="True" Width="180px" ForeColor="Red">
            </asp:DropDownList><br />
                    From:<asp:TextBox ID="txtFDate" runat="server" Width="80px" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" MaxLength="10"></asp:TextBox><img height="17"
                        onclick="popUpCalendar(this, form1.txtFDate, dateFormat,-1,-1,false)" src="../images/calendar.gif"
                        type="button" width="30" id="imgF" runat="server" />To:<asp:TextBox ID="txtTDate" runat="server" Width="80px" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" MaxLength="10"></asp:TextBox><img height="17"
                        onclick="popUpCalendar(this, form1.txtTDate, dateFormat,-1,-1,false)" src="../images/calendar.gif"
                        type="button" width="30" id="imgT" runat="server"  />
            Top:<asp:DropDownList ID="ddlDFCnt" runat="server" Font-Bold="True" Width="40px">
                <asp:ListItem Selected="True">3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
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
                    <asp:ListItem>24</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>26</asp:ListItem>
                    <asp:ListItem>27</asp:ListItem>
                    <asp:ListItem>28</asp:ListItem>
                    <asp:ListItem>29</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                </asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlPCS" runat="server" Font-Bold="True" Width="64px" AutoPostBack="True" OnSelectedIndexChanged="ddlPCS_SelectedIndexChanged">
                <asp:ListItem Selected="True" Value="PCS">PCS</asp:ListItem>
                <asp:ListItem>PCS></asp:ListItem>
                <asp:ListItem>PCS=</asp:ListItem>
                <asp:ListItem>PCS<</asp:ListItem>
            </asp:DropDownList>:<asp:TextBox ID="tbPCS" runat="server" Font-Bold="True" Font-Names="Arial" onchange="if(/\D/.test(this.value)) {alert('只能輸入數字');value='';}"
                Font-Size="10pt" Width="80px" Enabled="False"></asp:TextBox>
            <asp:DropDownList ID="ddlAYield" runat="server" Font-Bold="True" Width="71px" AutoPostBack="True" OnSelectedIndexChanged="ddlAYield_SelectedIndexChanged">
                <asp:ListItem Selected="True">A Yield</asp:ListItem>
                <asp:ListItem>A Yield></asp:ListItem>
                <asp:ListItem>A Yield=</asp:ListItem>
                <asp:ListItem>A Yield<</asp:ListItem>
            </asp:DropDownList>:<asp:TextBox ID="tbAYield" runat="server" Font-Bold="True" Font-Names="Arial" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')"
                Font-Size="10pt" Width="80px" Enabled="False"></asp:TextBox>
            <asp:CheckBox ID="cbCPFlag" runat="server" BackColor="#E0E0E0" Font-Bold="True"
                Font-Names="Arial" Font-Size="10pt" Style="border-top-width: thin; border-left-width: thin;
                border-left-color: blue; border-bottom-width: thin; border-bottom-color: blue;
                border-top-color: blue; border-right-width: thin; border-right-color: blue" Text="CC"
                Width="1px" Visible="False" />
            <asp:CheckBox ID="CB13NC" runat="server" BackColor="#E0E0E0" Font-Bold="True"
                Font-Names="Arial" Font-Size="10pt" Style="border-top-width: thin; border-left-width: thin;
                border-left-color: blue; border-bottom-width: thin; border-bottom-color: blue;
                border-top-color: blue; border-right-width: thin; border-right-color: blue" Text="Cell2 13NC"
                Width="100px" /><br />
            <table style="width: 561px;">
                <tr>
                    <td style="width: 100px;" align="left" valign="top">
            <asp:DropDownList ID="ddlSubLineID" runat="server" Font-Bold="True" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlSubLineID_SelectedIndexChanged">
                <asp:ListItem>SubLineID(ALL)</asp:ListItem>
                <asp:ListItem>UserDefine</asp:ListItem>
            </asp:DropDownList></td>
                    <td style="width: 100px;" align="left" valign="top">
            <asp:DropDownList ID="ddlLotType" runat="server" Font-Bold="True" Width="110px" AutoPostBack="True" OnSelectedIndexChanged="ddlLotType_SelectedIndexChanged">
                <asp:ListItem>LotType(ALL)</asp:ListItem>
                <asp:ListItem>UserDefine</asp:ListItem>
            </asp:DropDownList></td>
                    <td style="width: 100px;" align="left" valign="top">
            <asp:DropDownList ID="ddlProdID" runat="server" Font-Bold="True" Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddlProdID_SelectedIndexChanged">
                <asp:ListItem>ProdID(ALL)</asp:ListItem>
                <asp:ListItem>UserDefine</asp:ListItem>
            </asp:DropDownList></td>
                    <td align="left" valign="top">
            <asp:DropDownList ID="ddlWonbr" runat="server" Font-Bold="True" Width="140px" AutoPostBack="True" OnSelectedIndexChanged="ddlWonbr_SelectedIndexChanged">
                <asp:ListItem>Wonbr(ALL)</asp:ListItem>
                <asp:ListItem>UserDefine</asp:ListItem>
            </asp:DropDownList></td>
            <td align="left" valign="top" style="width: 3px">
                <asp:DropDownList ID="ddlGCraft" runat="server" AutoPostBack="True" Font-Bold="True"
                    Font-Names="Arial" Font-Size="10pt" OnSelectedIndexChanged="ddlGCraft_SelectedIndexChanged"
                    Width="140px">
                    <asp:ListItem Value="0">CRAFT(ALL)</asp:ListItem>
                    <asp:ListItem Value="1">TN</asp:ListItem>
                    <asp:ListItem Value="2">T3</asp:ListItem>
                </asp:DropDownList></td>
                    <td align="left" valign="top" style="width: 3px">
                        </td>
                    <td align="left" style="width: 3px;" valign="top">
                        <asp:Label ID="lblTTime" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            ForeColor="Green" Height="1px" Width="150px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 120px;">
                        <asp:Panel ID="PSubLineID" runat="server" Height="120px" Width="150px"  ScrollBars="Vertical" Visible="False">
                            <asp:CheckBoxList ID="cblSubLineID" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" RepeatLayout="Flow" style="line-height: normal" RepeatColumns="1">
                            </asp:CheckBoxList></asp:Panel>
                    </td>
                    <td style="width: 100px; height: 120px;">
                        <asp:Panel ID="PLotType" runat="server" Height="120px" Width="110px"  ScrollBars="Vertical" Visible="False">
                            <asp:CheckBoxList ID="cblLotType" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" RepeatLayout="Flow"  style="line-height: normal" RepeatColumns="1">
                            </asp:CheckBoxList></asp:Panel>
                    </td>
                    <td style="width: 100px; height: 120px;">
                        <asp:Panel ID="PProdID" runat="server" Height="120px" Width="170px"  ScrollBars="Vertical" Visible="False">
                            <asp:CheckBoxList ID="cblProdID" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" RepeatLayout="Flow" style="line-height: normal">
                        </asp:CheckBoxList></asp:Panel>
                    </td>
                    <td style="height: 120px">
                        <asp:Panel ID="PWonbr" runat="server" Height="120px" Width="140px"  ScrollBars="Vertical" Visible="False">
                            <asp:CheckBoxList ID="cblWonbr" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" RepeatLayout="Flow" style="line-height: normal">
                        </asp:CheckBoxList></asp:Panel>
                    </td>
                    <td style="height: 120px; width: 3px;">
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
                    <td style="height: 120px; width: 3px;">
                    </td>
                    <td style="height: 120px; width: 3px;">
                        </td>
                </tr>
            </table>
            <asp:TextBox ID="tbFABID" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                ID="tbFABName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                    ID="tbLoginNTName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                        ID="tbLoginPCName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                            ID="tbLoginPCIP" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                ID="tbDateType" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                    ID="tbCPFlag" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                        ID="tbSqlDate" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                            ID="tbSqlWhere" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                                ID="tbOPERATIONID" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                                    ID="tbType" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                                        ID="tbFDate" runat="server" Visible="False" Width="25px"></asp:TextBox><asp:TextBox
                                                            ID="tbTDate" runat="server" Visible="False" Width="25px"></asp:TextBox><asp:TextBox
                                                                ID="tbDiffDate" runat="server" Visible="False" Width="25px"></asp:TextBox>
            <asp:TextBox ID="tbAYPS" runat="server" Visible="False" Width="25px"></asp:TextBox></asp:Panel><div>
            <table style="width: 803px; height: 20px">
                <tr>
                    <td valign="top">
        <asp:Button ID="btQuery" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Text="Query" OnClick="btQuery_Click" />
        <asp:Button ID="btExport" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Text="ExportExcel" OnClick="ExportExcel_Click" />
            <asp:Button ID="btClear" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Text="Clear" OnClick="btClear_Click" />&nbsp; &nbsp;<asp:Label ID="lblFabID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            ForeColor="Red" Width="60px"></asp:Label>
            <asp:DropDownList ID="ddlByProdID" runat="server" Font-Bold="True" Width="170px" AutoPostBack="True" Visible="False">
            </asp:DropDownList></td>
                    <td align="right" valign="top">
                        <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ToolTip="返回主頁面">Main Menu</asp:HyperLink></td>
                        <td align="right" style="width: 5px" valign="top">
                            <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="10pt" NavigateUrl="~/Description/Cell2AllLightOnDaily.htm" Target="_blank" ToolTip="報表說明">Help</asp:HyperLink></td>
                </tr>
                <tr>
                    <td colspan="3">
                        -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------</td>
                </tr>
            </table>
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
            <asp:Panel ID="P1" runat="server" Height="300px" Width="800px" ScrollBars="Auto">
                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="#333333" Width="628px" AutoGenerateColumns="False">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" /><Columns>
                                <asp:BoundField DataField="Product_ID" HeaderText="產品號碼">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ReportDT" HeaderText="ReportDT" Visible="False">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OPERATION_ID" HeaderText="站點" Visible="False">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Chip_Qty" HeaderText="Chip_Qty">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="A_Rate" HeaderText="A_Rate">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="S_Rate" HeaderText="S_Rate">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Total_Qty" HeaderText="TotalQty" Visible="False">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOTGRADE" HeaderText="LOTGRADE" Visible="False">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DefectCode" HeaderText="DefectCode">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DF_Rate" HeaderText="DF_Rate">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="D1Day_Rate" HeaderText="D1Day_Rate">
                                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="D2Day_Rate" HeaderText="D2Day_Rate">
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
                &nbsp;</div>
    </form>
</body>
</html>
