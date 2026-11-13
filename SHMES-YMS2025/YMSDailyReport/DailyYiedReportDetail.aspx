<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyYiedReportDetail.aspx.cs" Inherits="Cell2DailYiedReport_Cell2DailYiedDetailReport" %>

<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void tbFDate_TextChanged(object sender, EventArgs e)
    {

    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Daily Yield Report Detail</title>
    <style type="text/css"> 
        #winpop { width:200px; height:0px; position:absolute; right:0; bottom:0; border:1px solid #999999; margin:0; padding:1px; overflow:hidden;display:none; background:#FFFFFF} 
        #winpop .title { width:100%; height:20px; line-height:20px; background:#FFCC00; font-weight:bold; text-align:center; font-size:12px;} 
        #winpop .con { width:100%; height:80px; line-height:80px; font-weight:bold; font-size:12px; color:#FF0000; text-decoration:underline; text-align:center} 
        #silu { font-size:13px; color:#999999; position:absolute; right:0;bottom:0px; text-align:right; text-decoration:underline; line-height:22px;} 
        .close { position:absolute; right:4px; top:-1px; color:#FFFFFF; cursor:pointer} 
    </style> 
    <script type="text/javascript"> 
        function show_pop()
        {//?示窗口 
            document.getElementById("winpop").style.display="block"; 
            timer=setInterval("changeH(4)",2);//?用changeH(4),每0.002秒向上移?一次 
        } 
        function hid_pop()
        {//?藏窗口 
            timer=setInterval("changeH(-4)",2);//?用changeH(-4),每0.002秒向下移?一次 
        } 
        function changeH(addH) 
        { 
            var MsgPop=document.getElementById("winpop"); 
            var popH=parseInt(MsgPop.style.height||MsgPop.currentStyle.height);//用parseInt??象的高度?化??字,以方便下面比?（JS?<style>中的height要用"currentStyle.height"） 
            if (popH<=100&&addH>0||popH>=4&&addH<0)
            {//如果高度小于等于100(str>0)或高度大于等于4(str<0) 
                MsgPop.style.height=(popH+addH).toString()+"px";//高度增加或?少4?象素 
            } 
            else{//否? 
                clearInterval(timer);//取消?用,意思就是如果高度超?100象素了,就不再增?了，或高度等于0象素了，就不再?少了 
                MsgPop.style.display=addH>0?"block":"none"//向上移??窗口?示,向下移??窗口?藏（因?窗口有?框,所以?是可以看?1~2象素???去,??候就把DIV?藏掉） 
            } 
        }
        //window.onload=function()
        //{//加? 
        //    setTimeout("show_pop()",1000);//1秒后?用show_pop() 
        //    setTimeout("hid_pop()",10000);//10秒后?用hid_pop() 
        //} 

        function RunFunc()
        {//加? 
            setTimeout("show_pop()",1000);//1秒后?用show_pop() 
            setTimeout("hid_pop()",10000);//10秒后?用hid_pop() 
        } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="winpop">
        <div class="title"><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label><span class="close" onclick="hid_pop()" style="right: 3px; width: 213px; top: 1px"></span></div> 
        <div class="con"><asp:TextBox ID="tbBody" runat="server" Height="80px" ReadOnly="True" TextMode="MultiLine" Width="198px"></asp:TextBox></div> 
    </div>
    <div align="left">
        <table width="800" style="height: 400px" id="TableMain" runat="server">
            <tr align="left">
                <td style="height: 26px; width: 500px;">
                    <asp:DropDownList ID="DDLFABID" runat="server" Enabled="False" Font-Bold="True" Font-Names="Arial"
                        ForeColor="Blue" Width="100px">
                        <asp:ListItem Value="A">LCM1</asp:ListItem>
                        <asp:ListItem Value="B">LCM2</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="Button2" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Blue"
                        OnClick="Button2_Click" Text="ExportExcel" />
                    <asp:TextBox ID="tbFABID" runat="server" Visible="False" Width="80px"></asp:TextBox>
                    <asp:TextBox ID="tbOperation" runat="server" Visible="False" Width="80px"></asp:TextBox>
                    <asp:TextBox ID="tbPRODID" runat="server" Visible="False" Width="80px"></asp:TextBox>
                    <asp:TextBox ID="tbFDate" runat="server" Visible="False" Width="80px" OnTextChanged="tbFDate_TextChanged"></asp:TextBox>
                    <asp:TextBox ID="tbTDate" runat="server" OnTextChanged="tbFDate_TextChanged" Visible="False"
                        Width="80px"></asp:TextBox>
                    <asp:TextBox ID="tbyield" runat="server" OnTextChanged="tbFDate_TextChanged" Visible="False"
                        Width="80px"></asp:TextBox>
                    <asp:TextBox ID="tbInput" runat="server" OnTextChanged="tbFDate_TextChanged" Visible="False"
                        Width="80px"></asp:TextBox>
                    <asp:TextBox ID="tbsqlLevel" runat="server" OnTextChanged="tbFDate_TextChanged" Visible="False"
                        Width="1px"></asp:TextBox>
                    <asp:TextBox ID="tbsqlLevelA" runat="server" OnTextChanged="tbFDate_TextChanged"
                        Visible="False" Width="1px"></asp:TextBox>
                    <asp:TextBox ID="tbsqlLevelB" runat="server" OnTextChanged="tbFDate_TextChanged" Visible="False"
                        Width="1px"></asp:TextBox>
                    <asp:TextBox ID="tbAAQTY" runat="server" OnTextChanged="tbFDate_TextChanged" Visible="False"
                        Width="1px"></asp:TextBox>
                    <asp:TextBox ID="tbsqlLevelC" runat="server" OnTextChanged="tbFDate_TextChanged"
                        Visible="False" Width="1px"></asp:TextBox><asp:TextBox ID="Tbmode" OnTextChanged="tbFDate_TextChanged" runat="server" Visible="False" Width="1px"></asp:TextBox><asp:TextBox
                            ID="TbSIZE" runat="server" OnTextChanged="tbFDate_TextChanged" Visible="False"
                            Width="1px"></asp:TextBox></td>
                <td style="height: 26px">
                    <asp:TextBox ID="tbUserName" runat="server" Visible="False" Width="50px"></asp:TextBox></td>
                <td style="height: 26px">
                    <asp:TextBox ID="tbPCName" runat="server" Visible="False" Width="50px"></asp:TextBox>
                    <asp:TextBox ID="tbFunctionName" runat="server" Visible="False" Width="50px"></asp:TextBox></td>
                <td style="height: 26px">
                    </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Panel ID="PWorkOrderDelay" runat="server" ScrollBars="Auto" Width="790px" Height="500px">
                        <br /><table style="height: 35px" id="Table2" runat="server" width="790" >
                            <tbody>
                                <tr>
                                    <td style="width: 140px; height: 11px">
                                        <asp:Label ID="Label1" runat="server" Width="127px"></asp:Label></td>
                                    <td style="width: 126px; height: 11px">
                                        <asp:Label ID="Label2" runat="server" Width="152px"></asp:Label></td>
                                    <td style="width: 111px; height: 11px">
                                        <asp:Label ID="Label3" runat="server" Width="117px"></asp:Label></td>
                                    <td style="width: 18px; height: 11px">
                                        <asp:Label ID="Label5" runat="server" Width="17px">~</asp:Label></td>
                                    <td style="width: 153px; height: 11px">
                                        <asp:Label ID="Label4" runat="server" Width="120px"></asp:Label></td>
                                    <td style="width: 133px; height: 11px">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:GridView ID="GridView1" runat="server" Width="726px" OnRowDataBound="GridView1_RowDataBound">
                            <HeaderStyle BackColor="#C0C0FF" Font-Bold="True" Font-Size="Large" HorizontalAlign="Center" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
