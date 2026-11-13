<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SubLineEQ24HRStatusQuery.aspx.cs" Inherits="WebReport.Reports.SubLineEQ24HRStatusQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SUB-LINE前24小時狀態查詢</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div>
            <table style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                padding-top: 0px; width: 100%" cellspacing="0">
                <tr>
                    <td rowspan="2" style="background-position-x: center; background-image: url(../../Images/right_bg.jpg);
                        background-repeat: repeat-y; text-align: left">
                        <div id="filter">
                            <table>
                                <tr>
                                    <td>
                                        <div id="fromStationLabel" class="LabelText">
                                            &nbsp;Stations<br />
                                            (For SubLine Query)</div>
                                    </td>
                                    <td>
                                        <div id="prodIdDropDownList">
                                            <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                                z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                                width: 153px; border-bottom: black 1px solid; top: 78px; height: 72px">
                                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged"
                                                    Text="ALL" Width="100%" />
                                                <asp:CheckBoxList ID="lstStations" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstStations_SelectedIndexChanged"
                                                    Width="100%">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                            <asp:DropDownList ID="cmbSubLine" runat="server" OnSelectedIndexChanged="cmbSubLine_SelectedIndexChanged" AutoPostBack="True" Visible="False">
                                            </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <div id="partIdList">
                                            &nbsp;</div>
                                    </td>
                                    <td colspan="2" style="text-align: right">
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢報表" /></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div id="ErrMsg">
                                            <asp:Label ID="lblErrMsg" runat="server"></asp:Label></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="report">
                            <%--<rsweb:ReportViewer ID="reportViewer" runat="server" ShowFindControls="False"
                                Width="100%">
                            </rsweb:ReportViewer>--%>

                            <rsweb:ReportViewer ID="reportViewer" runat="server" ShowFindControls="False"
                                Width="100%">
                            </rsweb:ReportViewer>

                        </div>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #3c2679">
                    </td>
                </tr>
                </table>
        </div>
    </form>
</body>
</html>
