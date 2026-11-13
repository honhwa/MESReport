<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductDailyReport.aspx.cs" Inherits="WebReport.Reports.ProductDailyReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Cell2生產日報</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table style="width: 830px; height: 104px">
            <td colspan="1" style="font-family: Times New Roman; height: 27px; text-align: center; width: 15px;">
            </td>
            <td colspan="1" style="font-family: Times New Roman; height: 27px; text-align: center; width: 41px;">
            </td>
           <td colspan="5" style="font-family: Times New Roman; height: 27px; text-align: center">
                    <strong ><span style="font-size: 14pt">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; Cell2生產日報</span></strong></td>
            <tr>
                <td style="width: 15px; height: 44px">
                </td>
                <td align="center" style="width: 41px; height: 44px">
                    日期</td>
                <td style="width: 178px; height: 44px; text-align: left">
                    <br />
                    <asp:TextBox ID="txtFromTime" runat="server" Width="142px"></asp:TextBox>
                      <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,false);"
                       type="button" /><br />
                    <br />
                </td>
                <td align="center" style="width: 73px; height: 44px">
                </td>
                <td style="width: 261px; height: 44px; text-align: left">
                    <div id="Div1" class="timePicker" style="width: 150px; height: 30px">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 15px; height: 7px">
                </td>
                <td align="center" style="width: 41px; height: 7px">
                    站&nbsp; 別</td>
                <td style="width: 178px; height: 7px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 174px; border-bottom: black 1px solid; top: 78px; height: 58px">
                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstStation" runat="server" AutoPostBack="True" OnDataBinding="lstStation_SelectedIndexChanged"
                            Width="99%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td align="center" style="width: 73px; height: 7px">
                    產品型號</td>
                <td style="width: 261px; height: 7px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 193px; border-bottom: black 1px solid; top: 78px; height: 63px">
                        <asp:CheckBox ID="chkAllProdID" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllProdID_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstProductID" runat="server" AutoPostBack="True" OnDataBinding="lstProductID_SelectedIndexChanged"
                            Width="99%" BorderStyle="Inset">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td align="center" style="height: 7px" colspan="3">
                <asp:Button ID="cmdExportDef" runat="server" OnClick="cmdExportDef_Click" Text="匯出Defect資訊" Width="108px" Font-Names="Arial" Font-Size="Small" Height="42px" /></td>
                <td style="width: 103px; height: 7px; text-align: left">
                </td>
            </tr>
            <tr>
                <td style="width: 15px; height: 14px">
                </td>
                <td colspan="8" style="vertical-align: middle; height: 26px">
                    <br />
                    <asp:Label ID="lblErrMsg" runat="server" Width="451px"></asp:Label>
                    &nbsp; &nbsp; &nbsp; 
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢" Width="49px" />
                    &nbsp; 
                    <asp:Label ID="lblTargetQty" runat="server" Width="155px"></asp:Label></td>
            </tr>
        </table>
    </div>
<%--        <rsweb:ReportViewer ID="ReportViewerWip" runat="server" Width="986px" Height="565px">
        </rsweb:ReportViewer>--%>

        <rsweb:ReportViewer ID="ReportViewerWip" runat="server" Width="986px" Height="565px">
        </rsweb:ReportViewer>

    </form>
</body>
</html>
