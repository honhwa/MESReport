<%@ page language="C#" autoeventwireup="true" inherits="MainButton, App_Web_l0h4z2ft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
<style type="text/css"> 
#winpop { width:200px; height:0px; position:absolute; right:0; bottom:0; border:1px solid #999999; margin:0; padding:1px; overflow:hidden;display:none; background:#FFFFFF} 
#winpop .title { width:100%; height:20px; line-height:20px; background:#FFCC00; font-weight:bold; text-align:center; font-size:12px;} 
#winpop .con { width:100%; height:80px; line-height:80px; font-weight:bold; font-size:12px; color:#FF0000; text-decoration:underline; text-align:center} 
#silu { font-size:13px; color:#999999; position:absolute; right:0;bottom:0px; text-align:right; text-decoration:underline; line-height:22px;} 
.close { position:absolute; right:4px; top:-1px; color:#FFFFFF; cursor:pointer} 
</style> 
<script type="text/javascript">
    function show_pop() {//显示窗口 
        document.getElementById("winpop").style.display = "block";
        timer = setInterval("changeH(4)", 2); //调用changeH(4),每0.002秒向上移动一次 
    }
    function hid_pop() {//隐藏窗口 
        timer = setInterval("changeH(-4)", 2); //调用changeH(-4),每0.002秒向下移动一次 
    }
    function changeH(addH) {
        var MsgPop = document.getElementById("winpop");
        var popH = parseInt(MsgPop.style.height || MsgPop.currentStyle.height); //用parseInt将对象的高度转化为数字,以方便下面比较（JS读<style>中的height要用"currentStyle.height"） 
        if (popH <= 100 && addH > 0 || popH >= 4 && addH < 0) {//如果高度小于等于100(str>0)或高度大于等于4(str<0) 
            MsgPop.style.height = (popH + addH).toString() + "px"; //高度增加或减少4个象素 
        }
        else {//否则 
            clearInterval(timer); //取消调用,意思就是如果高度超过100象素了,就不再增长了，或高度等于0象素了，就不再减少了 
            MsgPop.style.display = addH > 0 ? "block" : "none"//向上移动时窗口显示,向下移动时窗口隐藏（因为窗口有边框,所以还是可以看见1~2象素没缩进去,这时候就把DIV隐藏掉） 
        }
    }
    //window.onload=function()
    //{//加载 
    //    setTimeout("show_pop()",1000);//1秒后调用show_pop() 
    //    setTimeout("hid_pop()",10000);//10秒后调用hid_pop() 
    //} 

    function RunFunc() {//加载 
        setTimeout("show_pop()", 1000); //1秒后调用show_pop() 
        setTimeout("hid_pop()", 10000); //10秒后调用hid_pop() 
    } 
</script>
</head>
<body style="margin: 0px">
    <form id="form1" runat="server">
<div id="winpop">
    <div class="title"><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label><span class="close" onclick="hid_pop()" style="right: 3px; width: 213px; top: 1px"></span></div> 
    <div class="con"><asp:TextBox ID="tbCon" runat="server" Height="80px" ReadOnly="True" TextMode="MultiLine" Width="198px"></asp:TextBox></div> 
</div>
        <div>
            <table style="width: 658px;" id="ss">
                <tr>
                    <td colspan="3" style="font-weight: bold; font-size: 24pt; color: blue; font-style: italic;
                        font-family: 標楷體; width: 869px;" align="left">
                        <asp:Image ID="Image10" runat="server" Height="50px" ImageUrl="~/images/CMI_ch2.jpg"
                            Width="118px" />(報廢管理系統)Scrap Management System</td>
                </tr>
                <tr>
                    <td colspan="3" style="background-position: center center; width: 869px; height: 21px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="background-position: center center;  width: 869px;" align="right">
                        <asp:Panel ID="PMain" runat="server" Height="400px" Width="860px">
                            <table style="width: 658px; height: 405px;" id="Table1">
                                <tr>
                                    <td align="center" rowspan="11" style="width: 34px">
                        <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="10pt" Width="400px" ForeColor="Red"></asp:Label><br />
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/YieldUp.jpg" Height="288px" Width="402px" /></td>
                    <td style="width: 42px; height: 30px">
                        <asp:Image ID="Image2" runat="server" Height="35px" ImageUrl="~/images/G21.gif" Width="86px" /></td>
                    <td style="width: 176px; height: 30px" align="left">
                        <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" Font-Bold="True" Font-Names="Arial"
                                Font-Overline="False" Font-Size="14pt" Height="28px" Width="360px" Font-Italic="True" ForeColor="#FF8000">報廢率數據查詢</asp:HyperLink>
                        <%--<asp:LinkButton ID="LinkButton1" runat="server" target="_blank" Font-Bold="True" Font-Names="Arial"
                            Font-Overline="False" Font-Size="14pt" Height="28px" Width="360px" Font-Italic="True" ForeColor="#FF8000">報廢率數據查詢</asp:LinkButton>--%>
                            </td>
                                </tr>
                                <tr>
                    <td style="width: 42px">
                         <asp:Image ID="Image7" runat="server" Height="35px" ImageUrl="~/images/G26.gif" Width="86px" /></td>
                    <td style="width: 176px" align="left">
                        <asp:HyperLink ID="HyperLink2" runat="server" Target="_blank" Font-Bold="True" Font-Names="Arial"
                                    Font-Overline="False" Font-Size="14pt" Height="28px" Width="360px" Font-Italic="True" ForeColor="#FF8000">部材超耗查詢</asp:HyperLink>
                       <%-- <asp:LinkButton ID="LinkButton6" runat="server" Font-Bold="True" Font-Italic="True"
                            Font-Names="Arial" Font-Overline="False" Font-Size="14pt" ForeColor="#FF8000"
                            Height="28px" Width="360px">部材超耗查詢</asp:LinkButton>--%>
                            </td>
                                </tr>
                                 <tr>
                    <td style="width: 42px">
                        <asp:Image ID="Image9" runat="server" Height="35px" ImageUrl="~/images/G29.gif" Width="86px"/></td>
                    <td align="left">
                        <asp:HyperLink ID="HyperLink3" runat="server" Target="_blank" Font-Bold="True" Font-Names="Arial"
                                        Font-Overline="False" Font-Size="14pt" Height="28px" Width="360px" Font-Italic="True" ForeColor="#FF8000">SR報表-區間明細查詢</asp:HyperLink>
                       <%-- <asp:LinkButton ID="LinkButton9" runat="server" Font-Bold="True" Font-Italic="True"
                            Font-Names="Arial" Font-Overline="False" Font-Size="14pt" ForeColor="#FF8000"
                            Height="28px" Width="360px">SR報表-區間明細查詢</asp:LinkButton>--%>
                            </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="Image11" runat="server" Height="35px" ImageUrl="~/images/G31.gif" Width="86px" /></td>
                                    <td align="left">
                                         <asp:HyperLink ID="HyperLink4" runat="server" Target="_blank" Font-Bold="True" Font-Names="Arial"
                                            Font-Overline="False" Font-Size="14pt" Height="28px" Width="360px" Font-Italic="True" ForeColor="#FF8000">SR報表-趨勢查詢</asp:HyperLink>
                                       <%-- <asp:LinkButton ID="LinkButton10" runat="server" Font-Bold="True" Font-Italic="True"
                                            Font-Names="Arial" Font-Overline="False" Font-Size="14pt" ForeColor="#FF8000"
                                            Height="28px" Width="360px">SR報表-趨勢查詢</asp:LinkButton>--%>
                                            </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="Image12" runat="server" Height="35px" ImageUrl="~/images/G30.gif" Width="86px" /></td>
                                    <td align="left">
                                        <asp:HyperLink ID="HyperLink5" runat="server" Target="_blank" Font-Bold="True" Font-Names="Arial"
                                                Font-Overline="False" Font-Size="14pt" Height="28px" Width="360px" Font-Italic="True" ForeColor="#FF8000">站點損耗率明細</asp:HyperLink>
                                        <%--<asp:LinkButton ID="LinkButton11" runat="server" Font-Bold="True" Font-Italic="True"
                                            Font-Names="Arial" Font-Overline="False" Font-Size="14pt" ForeColor="#FF8000"
                                            Height="28px" Width="360px">站點損耗率明細</asp:LinkButton>--%>
                                            </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="Image14" runat="server" Height="35px" ImageUrl="~/images/G30.gif" Width="86px" /></td>
                                    <td align="left">
                                     <asp:HyperLink ID="HyperLink6" runat="server" Target="_blank" Font-Bold="True" Font-Names="Arial"
                                                Font-Overline="False" Font-Size="14pt" Height="28px" Width="360px" Font-Italic="True" ForeColor="#FF8000">站點報廢率明細</asp:HyperLink>
                                        <%--<asp:LinkButton ID="LinkButton12" runat="server" Font-Bold="True" Font-Italic="True"
                                            Font-Names="Arial" Font-Overline="False" Font-Size="14pt" ForeColor="#FF8000"
                                            Height="28px" Width="360px">站點報廢率明細</asp:LinkButton>--%>
                                            </td>
                                </tr>
                                <tr>
                                    <td><asp:Image ID="Image13" runat="server" Height="35px" ImageUrl="~/images/G24.gif" Width="86px" Visible="False" /></td>
                                    <td align="left">
                                        <asp:LinkButton ID="LinkButton13" runat="server" Font-Bold="True" Font-Italic="True"
                                            Font-Names="Arial" Font-Overline="False" Font-Size="14pt" ForeColor="#FF8000"
                                            Height="28px" Width="360px" Visible="False">EDC Report</asp:LinkButton></td>
                                </tr>
                            </table>
                            <span style="font-size: 8pt; color: #4f432b; background-color: #c0c0c0"></span>
                        </asp:Panel>
                        &nbsp;
                        <asp:TextBox ID="tbFABID" runat="server" Visible="False" Width="68px"></asp:TextBox>
                        <asp:TextBox ID="tbFABName" runat="server" Visible="False" Width="68px"></asp:TextBox><asp:TextBox
                            ID="tbLoginNTName" runat="server" Visible="False" Width="76px"></asp:TextBox><asp:TextBox
                                ID="tbLoginPCName" runat="server" Visible="False" Width="76px"></asp:TextBox><asp:TextBox
                                    ID="tbLoginPCIP" runat="server" Visible="False" Width="76px"></asp:TextBox><asp:TextBox
                                    ID="tbLoginUserID" runat="server" Visible="False" Width="76px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" colspan="3" style="height: 17px; width: 869px;">
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Arial"
                            Font-Size="8pt" ForeColor="Red" Width="712px" Visible="False"></asp:Label>&nbsp;&nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                        <asp:LinkButton ID="LinkButton7" runat="server" Font-Names="Arial" Font-Size="10pt"
                            OnClick="LinkButton7_Click" ToolTip="返回選擇倉別頁面">Cancel</asp:LinkButton></td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 17px; width: 869px;" align="center">
                        <span style="font-size: 8pt; color: #4f432b; background-color: #c0c0c0">
                            <asp:Panel ID="Panel1" runat="server" BackColor="Silver" Height="15px" Width="600px">
                                &nbsp;Copyright © 2023 CarUX Technology (Shanghai) Ltd. All Rights Reserved.</asp:Panel>
                        </span></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
