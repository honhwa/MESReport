<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cell2DailyYiedReport.aspx.cs"
    Inherits="YMSReport_ModuleDailyYield" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.Common, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Dundas.Charting.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ModuleDailyYiedReport</title>
    <style type="text/css">
         .fixedheader 
            {     
                position:relative ; 
                table-layout:fixed;
                top:expression(this.offsetParent.scrollTop -1);   
                z-index: 10;
            } 

.fixedheader th{text-overflow:ellipsis;overflow:hidden;white-space: nowrap;}
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
<body style="font-weight: bold; font-size: 10pt; color: blue; font-family: Arial;
    background-image: url(images/CMI_ch1.jpg); background-color: transparent;">
    <form id="form1" runat="server">
        <div id="winpop">
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label><span class="close" onclick="hid_pop()"
                    style="right: 3px; width: 213px; top: 1px"></span></div>
            <div class="con">
                <asp:TextBox ID="tbCon" runat="server" Height="80px" ReadOnly="True" TextMode="MultiLine"
                    Width="198px"></asp:TextBox></div>
        </div>
        <asp:Panel ID="PMain" runat="server" Height="30px" Style="border-right: blue thin solid;
            border-top: blue thin solid; font-weight: bold; font-size: 10pt; border-left: blue thin solid;
            color: blue; line-height: 25pt; border-bottom: blue thin solid; font-family: Arial"
            Width="1200px">
            <asp:DropDownList ID="ddlDateType" runat="server" Font-Bold="True" Width="60px" AutoPostBack="True">
                <asp:ListItem Selected="True">Date</asp:ListItem>
                <asp:ListItem Enabled="False">Week</asp:ListItem>
                <asp:ListItem Enabled="False">Month</asp:ListItem>
            </asp:DropDownList>
            From:<asp:TextBox ID="txtFDate" runat="server" Width="80px" Font-Bold="True" Font-Names="Arial"
                Font-Size="10pt" MaxLength="10"></asp:TextBox><img alt="" height="17" onclick="popUpCalendar(this, form1.txtFDate, dateFormat,-1,-1,false)"
                    src="../images/calendar.gif" type="button" width="30" id="imgF" runat="server" /><asp:DropDownList
                        ID="ddlFDate" runat="server" Font-Bold="True" Width="115px" Visible="False">
                    </asp:DropDownList>&nbsp; To:<asp:TextBox ID="txtTDate" runat="server" Width="80px"
                        Font-Bold="True" Font-Names="Arial" Font-Size="10pt" MaxLength="10"></asp:TextBox><img alt=""
                            height="17" onclick="popUpCalendar(this, form1.txtTDate, dateFormat,-1,-1,false)"
                            src="../images/calendar.gif" type="button" width="30" id="imgT" runat="server" /><asp:DropDownList
                                ID="ddlTDate" runat="server" Font-Bold="True" Width="115px" Visible="False">
                            </asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlYear" runat="server" Font-Bold="True"
                                Width="60px" Enabled="False">
                            </asp:DropDownList>&nbsp;
            <asp:CheckBox ID="cbCPFlag" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                Text="Cell2ProdID" Style="border-top-width: thin; border-left-width: thin; border-left-color: blue;
                border-bottom-width: thin; border-bottom-color: blue; border-top-color: blue;
                border-right-width: thin; border-right-color: blue;" Width="100px" BackColor="#E0E0E0"
                Visible="False" />
            <asp:CheckBox ID="CB13NC" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                Text="Cell2 13NC" Style="border-top-width: thin; border-left-width: thin; border-left-color: blue;
                border-bottom-width: thin; border-bottom-color: blue; border-top-color: blue;
                border-right-width: thin; border-right-color: blue;" Width="100px" BackColor="#E0E0E0" />
            <asp:DropDownList ID="ddlPdLen" runat="server" Font-Bold="True" Width="40px" Visible="False">
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
                <asp:ListItem Selected="True">16</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlShiftID" runat="server" Font-Bold="True" Width="75px" Visible="False">
                <asp:ListItem>ShiftID</asp:ListItem>
                <asp:ListItem>A</asp:ListItem>
                <asp:ListItem>B</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlSelType" runat="server" Font-Bold="True" Width="85px" Visible="False">
                <asp:ListItem>BEOL</asp:ListItem>
                <asp:ListItem>BEOL-M1</asp:ListItem>
            </asp:DropDownList><br />
            <table style="width: 561px;">
                <tr>
                    <td style="width: 100px; height: 10px;" align="left" valign="top">
                        <asp:DropDownList ID="ddlSubLineID" runat="server" Font-Bold="True" Width="150px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlSubLineID_SelectedIndexChanged"
                            Visible="False">
                            <asp:ListItem>SubLineID(ALL)</asp:ListItem>
                            <asp:ListItem>UserDefine</asp:ListItem>
                        </asp:DropDownList></td>
                    <td style="width: 100px; height: 10px;" align="left" valign="top">
                        <asp:DropDownList ID="ddlLotType" runat="server" Font-Bold="True" Width="110px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlLotType_SelectedIndexChanged">
                            <asp:ListItem>LotType(ALL)</asp:ListItem>
                            <asp:ListItem>UserDefine</asp:ListItem>
                        </asp:DropDownList></td>
                    <td style="width: 100px; height: 10px;" align="left" valign="top">
                        <asp:DropDownList ID="ddlProdID" runat="server" Font-Bold="True" Width="170px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlProdID_SelectedIndexChanged" Visible="False">
                            <asp:ListItem>ProdID(ALL)</asp:ListItem>
                            <asp:ListItem>UserDefine</asp:ListItem>
                        </asp:DropDownList></td>
                    <td align="left" valign="top" style="width: 143px; height: 10px;">
                        <asp:DropDownList ID="ddlWTLTM" runat="server" Font-Bold="True" Width="140px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlWonbr_SelectedIndexChanged" Visible="False">
                            <asp:ListItem>WTLTM(ALL)</asp:ListItem>
                            <asp:ListItem>UserDefine</asp:ListItem>
                        </asp:DropDownList></td>
                    <td align="left" valign="top" style="width: 3px; height: 10px;">
                    </td>
                    <td align="left" style="width: 3px; height: 10px;" valign="top">
                        <asp:Label ID="lblTTime" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            ForeColor="Green" Height="1px" Width="150px" Visible="False"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 10px;">
                        <asp:Panel ID="PSubLineID" runat="server" Height="10px" Width="150px" ScrollBars="Vertical"
                            Visible="False">
                            <asp:CheckBoxList ID="cblSubLineID" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="10pt" RepeatLayout="Flow" Style="line-height: normal" RepeatColumns="1"
                                Visible="False">
                            </asp:CheckBoxList></asp:Panel>
                    </td>
                    <td style="width: 100px; height: 10px;">
                        <asp:Panel ID="PLotType" runat="server" Height="120px" Width="110px" ScrollBars="Vertical"
                            Visible="False">
                            <asp:CheckBoxList ID="cblLotType" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="10pt" RepeatLayout="Flow" Style="line-height: normal" RepeatColumns="1">
                            </asp:CheckBoxList></asp:Panel>
                    </td>
                    <td style="width: 100px; height: 10px;">
                        <asp:Panel ID="PProdID" runat="server" Height="10px" Width="170px" ScrollBars="Vertical"
                            Visible="False">
                            <asp:CheckBoxList ID="cblProdID" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="10pt" RepeatLayout="Flow" Style="line-height: normal" Visible="False">
                            </asp:CheckBoxList></asp:Panel>
                    </td>
                    <td style="height: 10px; width: 14px;">
                        <asp:Panel ID="PWTLTM" runat="server" Height="10px" Width="140px" ScrollBars="Vertical"
                            Visible="False">
                            <asp:CheckBoxList ID="cblWTLTM" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="10pt" RepeatLayout="Flow" Style="line-height: normal" Visible="False">
                            </asp:CheckBoxList></asp:Panel>
                    </td>
                    <td style="height: 10px; width: 3px;">
                    </td>
                    <td style="height: 10px; width: 3px;">
                    </td>
                </tr>
            </table>
            <asp:DropDownList ID="ddlTYPE" runat="server" AppendDataBoundItems="True" Font-Bold="True"
                Width="160px">
                <asp:ListItem>13碼料號</asp:ListItem>
                <asp:ListItem>PanelSize</asp:ListItem>
                <asp:ListItem>13碼料號+PanelSize</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:TextBox ID="tbFABID" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                ID="tbFABName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                    ID="tbLoginNTName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                        ID="tbLoginPCName" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                            ID="tbLoginPCIP" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                ID="tbDateType" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                    ID="tbCPFlag" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                        ID="tbSqlDate" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                            ID="tbSqlWhere" runat="server" Visible="False" Width="50px"></asp:TextBox><asp:TextBox
                                                ID="tbFDate" runat="server" Visible="False" Width="25px"></asp:TextBox><asp:TextBox
                                                    ID="tbTDate" runat="server" Visible="False" Width="25px"></asp:TextBox><asp:TextBox
                                                        ID="tbDiffDate" runat="server" Visible="False" Width="25px"></asp:TextBox>
            <asp:DropDownList ID="ddlByProdID" runat="server" AutoPostBack="True" Font-Bold="True"
                Visible="False" Width="170px">
            </asp:DropDownList>
            <asp:DropDownList ID="ddlByWtltm" runat="server" AutoPostBack="True" Font-Bold="True"
                Visible="False" Width="111px">
            </asp:DropDownList></asp:Panel>
        <div>
            <table style="width: 1200px; height: 20px">
                <tr>
                    <td valign="top" style="height: 26px">
                        <asp:Button ID="btQuery" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Text="Query" OnClick="btQuery_Click" />
                        <asp:Button ID="btExport" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Text="ExportExcel" OnClick="ExportExcel_Click" />
                        <asp:Button ID="btClear" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Text="Clear" OnClick="btClear_Click" />&nbsp;
                        <asp:Label ID="lblFabID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            ForeColor="Red" Width="60px"></asp:Label></td>
                    <td align="right" valign="top" style="height: 26px; width: 167px;">
                        <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" ToolTip="返回主頁面">Main Menu</asp:HyperLink></td>
                    <td align="right" style="width: 5px; height: 26px;" valign="top">
                        <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" NavigateUrl="~/Description/Cell2DailyYield.htm" Target="_blank"
                            ToolTip="報表說明">Help</asp:HyperLink></td>
                </tr>
            </table>
            <asp:Panel ID="Panel1" runat="server" Style="border-right: blue thin solid; border-top: blue thin solid;
                border-left: blue thin solid; border-bottom: blue thin solid; font-weight: bold;
                font-size: 10pt; color: black; font-family: Arial;" Width="900px" Height="500px">
                <asp:Label ID="lblDataRunTime" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="8pt" ForeColor="Blue" Style="text-align: right" Width="670px"></asp:Label><br />
                <asp:Panel ID="Panel4" runat="server" Width="1200px" ScrollBars="Auto" Height="500px"
                    HorizontalAlign="Left">
                    <asp:GridView ID="GridView1" AutoGenerateColumns="False" CellPadding="4" Font-Bold="True"
                        Font-Names="Arial" Font-Size="10pt" ForeColor="#333333" runat="server" Width="900px"
                        OnRowDataBound="GridView1_RowDataBound">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="40px" />
                        <Columns>
                            <asp:BoundField DataField="OPERATION_ID" HeaderText="Operation">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PRODUCT_ID" HeaderText="Prodid">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Panel_Size" HeaderText="Panel Size">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Chip_Qty" HeaderText="Input">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="A_Yield" HeaderText="Yield">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AA_Yield" HeaderText="AA_Rate">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top1_Code" HeaderText="Top1_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate01" HeaderText="Rate01">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top2_Code" HeaderText="Top2_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate02" HeaderText="Rate02">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top3_Code" HeaderText="Top3_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate03" HeaderText="Rate03">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top4_Code" HeaderText="Top4_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate04" HeaderText="Rate04">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top5_Code" HeaderText="Top5_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate05" HeaderText="Rate05">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="A_Qty" HeaderText="A_Qty">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top6_Code" HeaderText="Top6_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate06" HeaderText="Rate06">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top7_Code" HeaderText="Top7_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate07" HeaderText="Rate07">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top8_Code" HeaderText="Top8_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate08" HeaderText="Rate08">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top9_Code" HeaderText="Top9_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate09" HeaderText="Rate09">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Top10_Code" HeaderText="Top10_Code">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Rate10" HeaderText="Rate10">
                                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" VerticalAlign="Middle" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" CssClass="fixedheader" />
                        <AlternatingRowStyle BackColor="White" VerticalAlign="Middle" />
                    </asp:GridView>
                </asp:Panel>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
