<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
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
<body>
    <form id="form1" runat="server">
<div id="winpop">
    <div class="title"><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label><span class="close" onclick="hid_pop()" style="right: 3px; width: 213px; top: 1px"></span></div> 
    <div class="con"><asp:TextBox ID="tbCon" runat="server" Height="80px" ReadOnly="True" TextMode="MultiLine" Width="198px"></asp:TextBox></div> 
</div>
        <div>
            <table style="width: 658px; height:500px" id="ss">
                <tr style="width: 658px; height:60px">
                    <td colspan="3" style="font-weight: bold; font-size: 24pt; color: blue; font-style: italic;
                        font-family: 標楷體" align="left">
                        <asp:Image ID="Image10" runat="server" Height="50px" ImageUrl="~/images/CMI_ch2.jpg"
                            Width="118px" />(良率管理系統)Yield Management System</td>
                </tr>
                <tr  style="height:30px">
                    <td colspan="3" style="background-position: center center; height: 21px;">
                    </td>
                </tr>
                <tr valign="top" style="height:300px" >
                    <td colspan="3" style="background-position: center center;height:50px" align="right" valign="top">
                        <asp:Panel ID="PTop" runat="server" Style="font-weight: bold; font-size: 18pt;
                            color: blue; font-family: 標楷體" Width="865px" Height="30px" HorizontalAlign="Center">
                            請先選擇登陸的廠別:<asp:RadioButtonList ID="rblFabID" runat="server" Font-Bold="True"
                            Font-Names="Arial" Font-Size="12pt" ForeColor="Red"
                            RepeatDirection="Horizontal" RepeatLayout="Flow" Width="141px" AutoPostBack="True" OnSelectedIndexChanged="rblFabID_SelectedIndexChanged">
                            <asp:ListItem>SH</asp:ListItem>
                        </asp:RadioButtonList><asp:Panel ID="Panel2" runat="server" Height="300px" ScrollBars="Vertical"
                                Width="850px">
                                <table style="font-size: 10pt; color: black; font-family: 標楷體;
                                    font-variant: small-caps">
                                    <tr>
                                        <td style="font-weight: bold; color: blue;" align="left">
                                            更新日期</td>
                                        <td style="width: 521px; color: blue;" align="left">
                                            版本內容</td>
                                        <td style="width: 107px; color: blue;" align="left">
                                            需求人</td>
                                        <td align="left" style="color: blue">
                                            版本號</td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="height: 17px">
                                            </td>
                                        <td align="left" style="width: 521px; height: 17px">
                                            </td>
                                        <td align="left" style="width: 107px; height: 17px">
                                            </td>
                                        <td align="left" style="height: 17px">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="height: 17px">
                                            </td>
                                        <td align="left" style="width: 521px; height: 17px">
                                            </td>
                                        <td align="left" style="width: 107px; height: 17px">
                                            </td>
                                        <td align="left" style="height: 17px">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" style="height: 18px">
                                            2013-08-01</td>
                                        <td valign="middle" align="left" style="width: 521px; height: 18px;">
                                            1.良率系統升級,新系統正式上線;</td>
                                        <td valign="middle" align="left" style="width: 107px; height: 18px;">
                                            MES</td>
                                        <td valign="middle" align="left" style="height: 18px">
                                            V1.00</td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="tbFABID" runat="server" Visible="False" Width="68px"></asp:TextBox><asp:TextBox
                            ID="tbFABName" runat="server" Visible="False" Width="68px"></asp:TextBox><asp:TextBox
                                ID="tbLoginNTName" runat="server" Visible="False" Width="76px"></asp:TextBox><asp:TextBox
                                    ID="tbLoginPCName" runat="server" Visible="False" Width="76px"></asp:TextBox><asp:TextBox
                                        ID="tbLoginPCIP" runat="server" Visible="False" Width="76px"></asp:TextBox></td>
                </tr>
                <tr style="height:30px">
                    <td align="left" colspan="3">
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial"
                            Font-Size="8pt" ForeColor="Red" Width="712px" Visible="False"></asp:Label>&nbsp;</td>
                </tr>
                <tr style="height:30px">
                    <td colspan="3"  align="center">
                        <span style="font-size: 8pt; color: #4f432b; background-color: #c0c0c0">
                            <asp:Panel ID="Panel1" runat="server" BackColor="Silver" Height="15px" Width="600px">
                                &nbsp;Copyright © 2013 Innolux(Shanghai) Corp. All Rights Reserved. &nbsp; &nbsp;&nbsp;</asp:Panel>
                        </span></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
