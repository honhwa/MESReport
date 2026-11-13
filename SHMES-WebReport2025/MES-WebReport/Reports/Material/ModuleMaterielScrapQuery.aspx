<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ModuleMaterielScrapQuery.aspx.cs" Inherits="WebReport.Reports.ModuleMaterielScrapQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>物料報廢查詢</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
            <table style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                padding-top: 0px; width: 100%" cellspacing="0">
                <tr>
                     <td style="width: 25px"></td>
                    <td style="background-position-x: center; background-image: url(../../Images/right_bg.jpg);
                        background-repeat: repeat-y; width: 955px;" rowspan="2">
                        <div id="filter">
                            <table>
                             <tr>
                                <td colspan="4" style="height: 46px">
                                       <strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                           &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                           &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<span style="font-size: 14pt"> 物料報廢查詢</span></strong>
                                  </td>
                               </tr>
                                <tr>
                                    <td>                                       
                                    </td>
                                    <td style="width: 78px">                                       
                                            開始日期                                       
                                    </td>
                                    <td style="width: 154px">
                                        <div id="timePickerLeftPicker" class="timePicker">
                                            <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                            <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,true);"
                                                type="button" /></div>
                                    </td>
                                    <td style="width: 77px">                                        
                                            結束日期
                                        </td>
                                    <td>
                                        <div id="timePickerRightPicker" class="timePicker">
                                            <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                            <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,true);"
                                                type="button" /></div>
                                    </td>
                                </tr>
                                <tr>
                                     <td>                                 
                                        
                                    </td>
                                    <td style="width: 78px">                                 產品型號
                                        
                                    </td>
                                    <td style="width: 154px">
                                        <div id="prodIdDropDownList">
                                            <asp:DropDownList ID="cmbProdId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbProdId_SelectedIndexChanged" Width="145px">
                                            </asp:DropDownList></div>
                                    </td>
                                    <td style="width: 77px">
                                       物料料號
                                    </td>
                                    <td>
                                            <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                                z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                                width: 153px; border-bottom: black 1px solid; top: 78px; height: 62px">
                                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged"
                                                    Text="ALL" Width="100%" />
                                                <asp:CheckBoxList ID="lstPartId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstPartId_SelectedIndexChanged"
                                                    Width="100%">
                                                </asp:CheckBoxList>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 19px">
                                        </td>
                                    <td style="width: 78px; height: 19px">
                                        線 &nbsp;&nbsp; 別</td>
                                    <td style="height: 19px; width: 154px;">
                                        <div id="partIdList">
                                            <asp:DropDownList ID="cmbSubLine" runat="server" Width="149px">
                                            </asp:DropDownList>&nbsp;</div>
                                    </td>
                                    <td style="width: 77px; height: 19px">
                                        </td>
                                    <td style="height: 19px">
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢報表" /></td>
                                </tr>
                              <tr>
                                <td style="height: 19px">
                                        </td>
                                    <td colspan="4" style="height: 15px">
                                        <div id="ErrMsg">
                                            <asp:Label ID="lblErrMsg" runat="server"></asp:Label></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="report" style="width: 88%">                           
                            <%--<rsweb:ReportViewer ID="reportViewer" runat="server" ShowFindControls="False"
                                Width="100%">
                            </rsweb:ReportViewer>--%>
                            <rsweb:ReportViewer ID="reportViewer" runat="server" ShowFindControls="False"
                                Width="100%">
                            </rsweb:ReportViewer>
                        </div>
                    </td>
                </tr>

            </table>
    </form>
</body>
</html>
