<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMSP.aspx.cs" Inherits="WebReport.Reports.RMSP" StylesheetTheme="UNICOMTheme" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 27px">
                </td>
                <td style="width: 69px; font-family: Times New Roman; height: 27px">
                </td>
                <td colspan="4" style="font-family: Times New Roman; height: 27px; text-align: center">
                    <strong ><span style="font-size: 14pt">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; Module報廢報表</span></strong></td>
            </tr>
            <tr>
                 <td style="vertical-align: middle; width: 23px; height: 27px">
                    </td>
                <td style="vertical-align: middle; width: 69px; height: 27px">
                    <div id="timePickerLabelLeft" class="LabelText" style="vertical-align: middle; text-align: center">
                        <span style="font-family: Arial"><strong>開始時間</strong>&nbsp;</span></div>
                </td>
                <td style="width: 142px; font-family: Times New Roman; height: 27px">
                    <div id="timePickerLeftPicker" class="timePicker">
                        <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                        <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,false);"
                            type="button" /></div>
                </td>
                <td style="font-family: Times New Roman; height: 27px; width: 67px;">
                    <div id="timePickerLabelRight" class="LabelText">
                        <strong>結束時間</strong></div>
                </td>
                <td style="font-size: 12pt; width: 141px; font-family: Times New Roman; height: 27px">
                    <div id="timePickerRightPicker" class="timePicker">
                        <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                        <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,false);"
                            type="button" /></div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 25px">
                    </td>
                <td style="width: 69px; height: 25px; text-align: center">
                <span style="font-size: 11pt">工單型態</span></td>
                <td style="width: 142px; height: 25px">
                    <div id="partIdList">
                        &nbsp;<div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 62px">
                        <asp:CheckBox ID="chkAllWotype" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllWotype_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstWotype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstWotype_SelectedIndexChanged"
                            Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </div>
                </td>
                <td style="height: 25px; text-align: center; width: 67px;">
                    工單狀態</td>
                <td style="width: 141px; height: 25px">
                    &nbsp;
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 62px">
                        <asp:CheckBox ID="chkAllWostatus" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllWostatus_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstWostatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstWostatus_SelectedIndexChanged"
                            Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="height: 25px; text-align: center; width: 79px;">
                    工&nbsp; 單</td>
                <td style="width: 173px; height: 25px">
                    &nbsp;
                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                            width: 153px; border-bottom: black 1px solid; top: 78px; height: 62px">
                            <asp:CheckBox ID="chkAllWonbr" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllWonbr_CheckedChanged"
                                Text="ALL" Width="100%" />
                            <asp:CheckBoxList ID="lstWonbr" runat="server"
                                Width="100%">
                            </asp:CheckBoxList>
                        </div>
                </td>
            </tr>
             <tr>
                <td style="vertical-align: middle; width: 23px; height: 27px">
                </td>
                <td style="width: 69px; height: 27px; text-align: center">
                    產品號碼</td>
                <td style="width: 142px; height: 27px">
                    <div id="Div1">
                        &nbsp; &nbsp;<div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 62px">
                        <asp:CheckBox ID="chkAllProd" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllProd_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstProd" runat="server"
                            Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </div>
                </td>
                <td style="height: 27px; text-align: center; width: 67px;">
                    料 &nbsp; 號</td>
                <td style="width: 141px; height: 27px">
                    &nbsp;<div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 62px">
                        <asp:CheckBox ID="chkAllPartID" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllPartID_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstPartID" runat="server"
                            Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="height: 27px; text-align: center; width: 79px;">
                    欄目顯示</td>
                <td style="width: 173px; height: 27px">
                    &nbsp;<div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                            width: 153px; border-bottom: black 1px solid; top: 78px; height: 62px">
                            <asp:CheckBox ID="chkColumns" runat="server" AutoPostBack="True" OnCheckedChanged="chkColumns_CheckedChanged"
                                Text="ALL" Width="100%" />
                            <asp:CheckBoxList ID="lstColumns" runat="server"
                                Width="100%">
                            </asp:CheckBoxList>
                        </div></td>
                <td style="height: 27px; text-align: center; width: 77px;">
                    <asp:Button ID="btnQuery" runat="server" Font-Names="Times New Roman" Font-Size="10pt"
                        OnClick="btnQuery_Click" Text="查詢報表" Width="66px" /></td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 27px">
                </td>
                <td colspan="4" style="height: 15px">
                        <asp:Label ID="lblErrMsg" runat="server" Font-Underline="True" ForeColor="Red" Width="76px" Font-Names="Arial" Font-Size="Small"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                </td>
            </tr>
        </table>    
    </div>
     <table style="width: 977px; height: 1px;" >
            <tr >
                <td style="width: 21px;">
                </td>
                <td style="width: 803px;">
<%--                    <rsweb:ReportViewer ID="ReportViewer" runat="server" Height="400px" Width="923px">
                    </rsweb:ReportViewer>--%>

                    <rsweb:ReportViewer ID="ReportViewer" runat="server" Height="400px" Width="923px">
                    </rsweb:ReportViewer>

                    <br />
                </td>
        </tr>
          </table>
    </form>
</body>
</html>
