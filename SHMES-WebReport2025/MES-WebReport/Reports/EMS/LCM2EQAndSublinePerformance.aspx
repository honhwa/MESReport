<%@ Page Language="C#" AutoEventWireup="true" Codebehind="LCM2EQAndSublinePerformance.aspx.cs"
    Inherits="WebReport.Reports.LCM2EQAndSublinePerformance" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>LCM2 EQ And Subline Performance</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div>
            <table style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                padding-top: 0px; width: 100%; height: 1614px;" cellspacing="0">
                <tr>
                    <td style="background-position-x: center; background-image: url(../../Images/right_bg.jpg);
                        background-repeat: repeat-y; vertical-align: top; height: 1623px;">
                        <div id="filter">
                            <table style="width: 788px">
                                <tr>
                                    <td style="vertical-align: middle; width: 97px; height: 8px;"><asp:CheckBox ID="chkDate" runat="server" Text=" By Date" OnCheckedChanged="chkDate_CheckedChanged" AutoPostBack="True" Width="74px" /></td>
                                    <td style="width: 184px; height: 8px">
                                        &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp;<br />
                                        <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px" OnTextChanged="txtFromTime_TextChanged"></asp:TextBox><input
                                            id="btnFromTime" class="CalendarBotton"onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,false);"
                                            type="button" /></td>
                                    <td style="width: 81px; height: 8px">
                                        SubLine/EQ</td>
                                    <td style="text-align: left; width: 224px; height: 8px;">
                                        &nbsp;<asp:CheckBox ID="cbSubLine" runat="server" Text="SubLine" AutoPostBack="True" Width="74px" />
                                        <asp:CheckBox ID="cbEQ" runat="server" Text="EQ" AutoPostBack="True" /></td>
                                    <td style="width: 83px; height: 8px">
                                        
                                            Process
                                    </td>
                                    <td style="height: 8px; width: 364px;">
                                        <asp:DropDownList ID="cmdProcess" runat="server" Width="120px" AutoPostBack="True">
                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                            <asp:ListItem>Module</asp:ListItem>
                                            <asp:ListItem>Cell2</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: middle; width: 97px; height: 43px;"><asp:CheckBox ID="chkYear" runat="server" Text="Year/Month" OnCheckedChanged="chkYear_CheckedChanged" AutoPostBack="True" Width="85px" Height="27px" /></td>
                                    <td style="width: 184px; height: 43px;">
                                        <div id="timePickerLeftPicker">
                                            &nbsp; &nbsp;<asp:DropDownList ID="cmbBy" runat="server" Width="63px" OnSelectedIndexChanged="cmbBy_SelectedIndexChanged" AutoPostBack="True" Height="21px">
                                            <asp:ListItem Selected="True">Month</asp:ListItem>
                                            <asp:ListItem>Week</asp:ListItem>
                                        </asp:DropDownList>
                                            &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; From</div>
                                    </td>
                                    <td style="width: 81px; height: 43px;">
                                        
                                            <asp:DropDownList ID="cmbFromYear" runat="server" AutoPostBack="True" Width="82px">
                                            </asp:DropDownList></td>
                                    <td style="text-align: left; width: 224px; height: 43px;">
                                        <div id="timePickerRightPicker">
                                            <asp:Label ID="Label1" runat="server" Text="年第"></asp:Label><asp:DropDownList ID="cmbFromTime" runat="server" Width="63px">
                                            </asp:DropDownList><asp:Label ID="lblFromTime" runat="server" Height="4px" Width="1px"></asp:Label>&nbsp;</div>
                                    </td>
                                    <td style="width: 83px; height: 43px;">
                                        &nbsp;To</td>
                                    <td style="width: 364px; height: 43px">
                                    <asp:DropDownList ID="cmbToYear" runat="server" Width="55px">
                                            </asp:DropDownList><asp:Label ID="Label2" runat="server" Text="年第"></asp:Label><asp:DropDownList ID="cmbToTime" runat="server" Width="52px">
                                            </asp:DropDownList><asp:Label ID="lblToTime" runat="server" Width="49px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td rowspan="2" style="vertical-align: middle; width: 97px;">
                                        站&nbsp; 別</td>
                                    <td rowspan="2" style="width: 184px">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 153px; border-bottom: black 1px solid; top: 78px; height: 156px">
                                            <asp:CheckBox ID="chkAllStations" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllStations_CheckedChanged"
                                                Text="ALL" Width="100%" />
                                            <asp:CheckBoxList ID="lstStations" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstStations_SelectedIndexChanged"
                                                Width="100%">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                    <td style="height: 12px; width: 81px;">
                                      
                                            Line ID
                                    </td>
                                    <td style="text-align: left; height: 12px; width: 224px;">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 168px; border-bottom: black 1px solid; top: 78px; height: 64px">
                                            <asp:CheckBox ID="chkAllLineIDs" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllLineIDs_CheckedChanged"
                                                Text="ALL" Width="100%" />
                                            <asp:CheckBoxList ID="lstLineId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstLineId_SelectedIndexChanged"
                                                Width="100%">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                    <td style="height: 12px; width: 83px;">
                                      
                                            EQ ID
                                    </td>
                                    <td style="height: 12px; width: 364px;">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 154px; border-bottom: black 1px solid; top: 78px; height: 62px">
                                            <asp:CheckBox ID="chkAllEQIDs" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllEQIDs_CheckedChanged"
                                                Text="ALL" Width="100%" />
                                            <asp:CheckBoxList ID="lstEqId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstEqId_SelectedIndexChanged"
                                                Width="100%">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 81px; height: 59px;">
                                        Subline ID
                                    </td>
                                    <td style="text-align: left; width: 224px; height: 59px;">
                                        &nbsp;<div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 168px; border-bottom: black 1px solid; top: 78px; height: 61px">
                                            <asp:CheckBox ID="chkAllSubLineIDs" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllSubLineIDs_CheckedChanged"
                                                Text="ALL" Width="100%" />
                                            <asp:CheckBoxList ID="lstSubLineId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstSubLineId_SelectedIndexChanged"
                                                Width="100%">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                    <td style="width: 83px; height: 59px;">
                                       
                                            EQ Type
                                    </td>
                                    <td style="width: 364px; height: 59px">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 153px; border-bottom: black 1px solid; top: 78px; height: 61px">
                                            <asp:CheckBox ID="chkAllEQTypes" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllEQTypes_CheckedChanged"
                                                Text="ALL" Width="100%" />
                                            <asp:CheckBoxList ID="lstEqType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstEqType_SelectedIndexChanged"
                                                Width="100%">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="vertical-align: middle; height: 24px;">
                                        <div id="ErrMsg">
                                            <asp:Label ID="lblErrMsg" runat="server"></asp:Label>&nbsp;</div>
                                    </td>
                                    <td colspan="1" style="width: 83px; height: 24px">
                                        </td>
                                    <td colspan="1" style="height: 24px; width: 364px;">
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢報表" Width="68px" /></td>
                                </tr>
                            </table>
                        </div>
                        <div style="width:100%;height:1790px">
                            <%--                            <rsweb:ReportViewer ID="reportViewer1" runat="server" Height="387px" Width="100%">
                            </rsweb:ReportViewer>
                            <rsweb:ReportViewer ID="reportViewer2" runat="server" Height="440px" Width="100%" HyperlinkTarget="IFrame1">
                                <LocalReport EnableHyperlinks="True">
                                </LocalReport>
                            </rsweb:ReportViewer>--%>
                            <rsweb:ReportViewer ID="reportViewer1" runat="server" Height="387px" Width="100%">
                            </rsweb:ReportViewer>
                            <rsweb:ReportViewer ID="reportViewer2" runat="server" Height="440px" Width="100%" HyperlinkTarget="IFrame1">
                                <LocalReport EnableHyperlinks="True">
                                </LocalReport>
                            </rsweb:ReportViewer>
                            <iframe id="IFrame1" name="IFrame1" frameborder="1" scrolling="no" src="Graph1.aspx"  width="350px" height="350px" style="height: 275px"></iframe>
                            <%--                        <reportviewer ID="reportViewer3" runat="server" Height="409px" Width="100%" HyperlinkTarget="IFrame2">
                                <LocalReport EnableHyperlinks="True">
                                </LocalReport>
                            </reportviewer>--%>
                            <rsweb:ReportViewer ID="reportViewer3" runat="server" Height="409px" Width="100%" HyperlinkTarget="IFrame2">
                                <LocalReport EnableHyperlinks="True">
                                </LocalReport>
                            </rsweb:ReportViewer>

                            <iframe id="IFrame2" name="IFrame2" frameborder="1" scrolling="no"src="Graph2.aspx" width="350px" height="350px" style="height: 276px"></iframe>
                        </div>
                    </td>
                </tr>
                </table>
        </div>
    </form>
</body>
</html>
