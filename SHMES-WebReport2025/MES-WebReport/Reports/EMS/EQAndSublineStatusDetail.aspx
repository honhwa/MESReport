<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EQAndSublineStatusDetail.aspx.cs" Inherits="WebReport.Reports.EQAndSublineStatusDetail" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>LCM2 EQ And Subline Performance</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table style="width: 855px; height: 162px">
            <tr>
                <td style="width: 19px; height: 19px;">
                </td>
                <td style="width: 110270px; height: 19px;">
                </td>
                <td style="width: 862px; text-align: left; height: 19px;">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 19px; height: 4px">
                </td>
                <td align="left" style="width: 110270px; height: 4px">
                    <br />
                    日期&nbsp;</td>
                <td align="left" colspan="5" style="height: 4px">
                    &nbsp;
                    &nbsp;&nbsp;&nbsp;<br />
                    從<asp:TextBox ID="txtFrDate" runat="server" Width="125px"></asp:TextBox><input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, txtFrDate, dateFormat,-1,-1,true);"
                            type="button" />&nbsp; 到

                    <asp:TextBox ID="txtToDate" runat="server" Width="120px" Height="18px"></asp:TextBox><input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, txtToDate, dateFormat,-1,-1,true);"
                            type="button" /></td>
                <td align="right" style="height: 4px;">
                    <br />
                    Line</td>
                <td align="left" style="width: 135px; height: 4px">
                    <br />
                    /EQ&nbsp;<asp:DropDownList ID="ddLineEq" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            Width="81px" Height="4px">
                        </asp:DropDownList></td>
              <td align="right" style="width: 621px; height: 4px">
                  <br />
                  段&nbsp;
                    </td>
              <td align="left" style="width: 119px; height: 4px">
                  <br />
                          <asp:DropDownList ID="ddModulCell" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            Width="103px" OnSelectedIndexChanged="ddModulCell_SelectedIndexChanged" Height="17px">
                        </asp:DropDownList></td>
            <td align="center" style="width: 76px; height: 4px">
                <br />
                    </td>
            <td align="center" style="width: 85px; height: 4px">
                <br />
                &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 19px; height: 3px">
                </td>
                <td align="left" style="width: 110270px; height: 3px">
                    站別</td>
                <td style="width: 862px; height: 3px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 181px; border-bottom: black 1px solid; top: 78px; height: 58px">
                        <asp:CheckBox ID="chkAllStations" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllStations_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstStations" runat="server" AutoPostBack="True" Width="99%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td align="center" style="height: 3px;">
                    EQType</td>
                <td style="width: 92px; height: 3px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 121px; border-bottom: black 1px solid; top: 78px; height: 55px">
                        <asp:CheckBox ID="chkAllEQType" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllEQType_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstEQType" runat="server" Width="99%" AutoPostBack="True" OnSelectedIndexChanged="lstEQType_SelectedIndexChanged">
                        </asp:CheckBoxList></div>
                </td>
                <td align="left" style="height: 3px;">
                    EQID</td>
                <td style="width: 135px; height: 3px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 119px; border-bottom: black 1px solid; top: 78px; height: 62px">
                        <asp:CheckBox ID="chkAllEQID" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllEQID_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstEQID" runat="server" AutoPostBack="True" Width="99%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td align="center" style="width: 621px; height: 3px">
                    Line</td>
                <td style="width: 119px; height: 3px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 128px; border-bottom: black 1px solid; top: 78px; height: 63px">
                        <asp:CheckBox ID="chkAllLine" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllLine_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstLine" runat="server" AutoPostBack="True" Width="99%" OnSelectedIndexChanged="lstLine_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </div>
                </td>
                 <td align="center" style="width: 76px; height: 3px">
                     Subline</td>
                <td style="width: 85px; height: 3px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 128px; border-bottom: black 1px solid; top: 78px; height: 63px">
                        <asp:CheckBox ID="chkAllSubline" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllSubline_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstSubline" runat="server" AutoPostBack="True" Width="99%">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 19px; height: 14px">
                </td>
                <td colspan="4" style="vertical-align: middle; height: 26px">
                    <asp:Label ID="lblErrMsg" runat="server" Width="312px"></asp:Label>
                    <asp:Button ID="ButtonQuery" runat="server" OnClick="ButtonQuery_Click" Text="查詢" Width="49px" />
                </td>
            </tr>
        </table>
        &nbsp;&nbsp;
<%--        <rsweb:ReportViewer ID="RVStatusDetail" runat="server" Width="982px">
        </rsweb:ReportViewer>--%>
        <rsweb:ReportViewer ID="RVStatusDetail" runat="server" Width="982px">
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
