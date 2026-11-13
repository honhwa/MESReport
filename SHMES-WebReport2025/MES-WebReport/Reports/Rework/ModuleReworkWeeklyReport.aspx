<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleReworkWeeklyReport.aspx.cs" Inherits="WebReport.Reports.ModuleReworkWeeklyReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Module重工Weekly報表</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="Stylesheet" type="text/html" />
    <script language="javascript" type="text/javascript">
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>    

<script type="text/javascript" src="../Material/popcalendar.js"></script>
<script type ="text/javascript" >dateFormat = 'yyyy-mm-dd'</script>

    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body topmargin=0  leftmargin=0 background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <table border="1" style="font-size: 10pt; color: green; font-family: 新細明體; mso-ascii-font-family: 'Courier New';
                        mso-hansi-font-family: 'Courier New'; mso-bidi-font-family: 'Courier New'; mso-font-kerning: 0pt;
                        mso-no-proof: yes; width: 982px; border-top-style: ridge; border-right-style: ridge; border-left-style: ridge; border-bottom-style: ridge;">
            <tr>
                <td style="width: 103px; height: 19px; text-align: right;">
                                        Bussiness owner：</td>
                <td style="width: 82px; height: 19px">
                    劉彩霞&nbsp; 55589</td>
                <td style="width: 81px; text-align: center;" rowspan="2">
                   <a href="../../Description/ModuleReworkWeekly.mht" target="_blank">報表規格書</a></td>
                <td rowspan="2" style="width: 654px; text-align: left">
                    </td>
            </tr>
            <tr>
                <td style="width: 103px; text-align: right; height: 31px;">
                                        IT owner：</td>
                <td style="width: 82px; height: 31px;">
                    胡俊寶&nbsp; 55883</td>
            </tr>
        </table> 
    
        <div>
            <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                margin: 0px; width: 99%; padding-top: 0px; height: 630px;">
                <tr>
                    <td style="background-position-x: left; background-image: url(../Images/right_bg.jpg);
                        width: 56px; background-repeat: repeat-y; height: 108px;">
                    </td>
                    <td style="background-position-x: center; background-image: url(../Images/right_bg.jpg);
                        background-repeat: repeat-y; vertical-align: text-top; text-align: left; height: 108px; width: 997px;">
                        <table style="vertical-align: text-bottom; width: 597px; height: 48px; text-align: center">
                            <tr>
                                <td colspan="8" style="height: 46px">
                                    <strong><span style="font-size: 14pt">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        Module重工Weekly報表</span></strong></td>
                            </tr>
                             <tr>
                                <td style="width: 73px; height: 24px">
                                    開始日期</td>
                                <td style="height: 24px; text-align: left; width: 166px;" colspan="3">
                                    &nbsp;<asp:TextBox ID="txtFromTime" runat="server" Width="117px"></asp:TextBox>
                                    <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,true);"
                                        style="width: 25px; background-image: url(../../Images/Button.gif); height: 23px;" type="button" /></td>
                                <td style="width: 86px; height: 24px" colspan="2">
                                    結束日期</td>
                                <td style="width: 203px; height: 24px; text-align: left">
                                    <asp:TextBox ID="txtToTime" runat="server" Width="120px"></asp:TextBox><input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,true);"
                                        style="width: 25px; background-image: url(../../images/Button.gif); height: 20px;" type="button" /></td>
                                <td style="width: 64px; height: 24px; text-align: left">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 73px; height: 82px;">
                                    產品號碼</td>
                                <td colspan="3" style="text-align: left; width: 166px; height: 82px;">
                                    &nbsp;
                                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow: auto;
                                                z-index: 104; left: 202px; border-left: black 1px solid;
                                                width: 167px; border-bottom: black 1px solid; top: 78px; height: 64px" id="DIV1" onclick="return DIV1_onclick()">
                                        <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged"
                                            Text="全選" AutoPostBack="True" Width="150px" /><asp:CheckBoxList ID="cblProdID" runat="server" Width="150px" Height="22px">
                                        </asp:CheckBoxList></div>
                                </td>
                                <td style="width: 86px; height: 82px;" colspan="2">
                                    群 &nbsp; &nbsp; &nbsp; 組</td>
                                <td style="width: 203px; height: 82px; text-align: left">
                                    <asp:DropDownList ID="dlGroup" runat="server" Width="144px" OnSelectedIndexChanged="dlGroup_SelectedIndexChanged" Enabled="False">
                                    </asp:DropDownList>
                                    </td>
                                <td style="width: 64px; height: 82px; text-align: left">
                                    <asp:Button ID="btnSerch" runat="server" Text="查詢報表" OnClick="btnSerch_Click" Width="60px" /></td>
                            </tr>
                        </table>
                        &nbsp;
                        <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                </tr>
                <tr>
                    <td rowspan="1" style="width: 56px; background-color: #3c2679; background-image: url(../Images/right_bg.jpg); height: 450px;">
                    </td>
                    <td style="background-position-x: center; background-image: url(../Images/right_bg.jpg);
                        vertical-align: top; background-repeat: repeat-y; width: 997px; height: 450px;">
                        <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1167px" Height="431px">
                        </rsweb:ReportViewer>--%>
                        
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1167px" Height="431px">
                        </rsweb:ReportViewer>
                        
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
