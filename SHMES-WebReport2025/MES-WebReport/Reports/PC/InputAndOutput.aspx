<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InputAndOutput.aspx.cs" Inherits="WebReport.Reports.InputAndOutput" StylesheetTheme="UNICOMTheme" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>InputAndOutputQuery</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <table style="width: 1001px; height: 162px">
        <tr>
                                    <td style="width: 19px; height: 26px">
                                        </td>
                                    <td style="width: 50px; height: 26px">
                                        </td>
                                    <td style="width: 61px; height: 26px; text-align: left">
                                        &nbsp;</td>
                     </tr> 
            <tr>
                                    <td style="width: 19px; height: 42px">
                                        </td>
                                    <td style="width: 60px; height: 42px" align="center">
                                        從日期</td>
                                    <td style="text-align: left" class="style2">
                                        <div id="timePickerLeftPicker" class="timePicker" style="width: 180px; height: 30px">
                                               
                                        <asp:TextBox ID="txtDate" runat="server" Width="107px"></asp:TextBox>&nbsp;
                                            <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, txtDate, dateFormat,-1,-1,false);"
                                                type="button" /></div>
                                         </td>
                                        <td style="width: 60px; height: 42px" align="center">
                                            到日期</td>
                                    <td style="width: 61px; height: 42px; text-align: left">
                                        <div id="Div1" class="timePicker" style="width: 180px; height: 30px">
                                               
                                        <asp:TextBox ID="txtToDate" runat="server" Width="107px"></asp:TextBox>&nbsp;
                                            <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, txtToDate, dateFormat,-1,-1,false);"
                                                type="button" /></div>
                                         </td>
 
                     </tr> 
                                
           <tr>
                                    <td style="width: 19px; height: 7px;">
                                        </td>
                                    <td style="width: 50px; height: 7px;" align="center">
                                        站 別</td>
                                    <td style="text-align: left; " class="style3">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 148px; border-bottom: black 1px solid; top: 78px; height: 100px">
                                            <asp:CheckBox ID="chkAllStations" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllStations_CheckedChanged"
                                                Text="ALL" Width="100%" />
                                            <asp:CheckBoxList ID="lstStations" runat="server" AutoPostBack="True"
                                                Width="99%">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                    <td style="width: 50px; height: 7px;" align="center">
                                        機 種</td>
                                    <td style="width: 121px; text-align: left; height: 7px;">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 201px; border-bottom: black 1px solid; top: 78px; height: 100px">
                                            <asp:CheckBox ID="chkAllProdID" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllProdID_CheckedChanged"
                                                Text="ALL" Width="100%" />
                                            <asp:CheckBoxList ID="lstProdId" runat="server"
                                                Width="99%">
                                            </asp:CheckBoxList></div>
                                    </td>
                                    <td style="width: 50px; height: 7px;" align="center">
                                        群 組</td>
                                    <td style="width: 135px; text-align: left; height: 7px;">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 148px; border-bottom: black 1px solid; top: 78px; height: 100px">
                                            <asp:CheckBox ID="chkAllProdGroup" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllProdGroup_CheckedChanged"
                                                Text="ALL" Width="100%" />
                                            <asp:CheckBoxList ID="lstProdGroup" runat="server" AutoPostBack="True"
                                                Width="99%">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                    <td style="width: 50px; height: 7px;" align="center">
                                        工單</td>
                                    <td style="width: 135px; text-align: left; height: 7px;">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 148px; border-bottom: black 1px solid; top: 78px; height: 100px">
                                            <asp:CheckBox ID="chkAllWotype" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllWotype_CheckedChanged"
                                                Text="ALL" Width="100%" />
                                            <asp:CheckBoxList ID="lstWotype" runat="server"
                                                Width="99%" AutoPostBack="True" OnSelectedIndexChanged="lstWotype_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
            <tr>
                <td style="width: 19px; height: 77px">
                </td>
                <td align="center" style="width: 50px; height: 77px">
                    查詢<br />
                    類型</td>
                <td align="center" colspan="2" style="height: 77px; text-align:left;">
                    <br />
                    <asp:RadioButtonList AutoPostBack="True" Height="2px" ID="RadioButtype" OnSelectedIndexChanged="Button1_Click" RepeatDirection="Horizontal" runat="server" Width="189px" >
                        <asp:ListItem >BY Date</asp:ListItem>
                        <asp:ListItem Selected="True">BY ALL</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
           <tr style="height: 50px">
            <td style="width: 19px;">
                </td>
                <td align="center" style="width: 50px;">
                    其他<br />
                    選項</td>
                    <td align="center" colspan="4" style="text-align:left;">
                        <asp:CheckBox ID="chkDelWorkctr" Text = "剔除非出貨料號 8380 8700 站點產出" runat="server" />
                    </td>

            </tr>
           <tr>
                                    <td style="width: 19px; height: 14px;">
                                        <br />
                                        </td>
                <td colspan="4" style="vertical-align: middle; height: 26px">
                    <br />
                   
                        <asp:Label ID="lblErrMsg" runat="server" Width="312px"></asp:Label>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢" Width="49px" />              
                    <br />
                    </td>
            </tr>
        </table>
        
        <table >
            <tr >
                <td style="width: 19px; height: 474px;">
                </td>
                <td style="height: 474px">
                    <br />
<%--                    <rsweb:ReportViewer ID="ReportViewer" runat="server" Height="451px" Width="902px">
                    </rsweb:ReportViewer>--%>
                                        <rsweb:ReportViewer ID="ReportViewer" runat="server"  Height="451px" Width="902px">
                    </rsweb:ReportViewer>

                    <br />
                </td>
        </tr>
        </table>
        &nbsp;
      </form>
</body>
</html>
