<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialQuery.aspx.cs" Inherits="WebReport.Reports.MaterialQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">

    <title>物料明細查詢</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="popcalendar.js"></script>
      
        <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body style="POSITION: absolute">
    <form id="form1" runat="server"> 
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>    
        <%--<rsweb:ReportViewer ID="RVMaterial" runat="server" Style="position: relative; top: 426px; left: 0px;"
            Width="1022px" Height="391px" Font-Names="Verdana" Font-Size="8pt">
            <LocalReport>
            </LocalReport>
        </rsweb:ReportViewer>--%>
        <rsweb:ReportViewer ID="RVMaterial" runat="server" Style="position: relative; top: 426px; left: 0px;"
            Width="1022px" Height="391px" Font-Names="Verdana" Font-Size="8pt">
            <LocalReport>
            </LocalReport>
        </rsweb:ReportViewer>
        &nbsp;
        <table style="width: 710px; height: 332px; left: 24px; position: relative; top: -367px;">
            <tr>
                <td colspan="5" style="height: 23px">
                    <strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<span style="font-size: 14pt"> 物料明細查詢</span></strong>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td style="height: 4px">
                </td>
                <td style="width: 110px; height: 4px">
                    開始日期
                </td>
                <td style="width: 219px; height: 4px">
                    &nbsp;
                    <div id="timePickerLeftPicker" class="timePicker">
                        <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Style="left: -40px; position: relative;
                            top: -9px" Width="120px"></asp:TextBox></div>
                
                       <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,true);"
                            style="left: 156px; width: 24px;  top:-4px; position: relative;" type="button" />
                      
                </td>
                      <td style="width: 100px; height: 4px">
                    結束日期</td>
                <td style="height: 4px; width: 237px;">
                    &nbsp;<div id="timePickerRightPicker" class="timePicker">
                        <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Style="left: -57px; position: relative;
                            top: -11px" Width="120px"></asp:TextBox>
                        <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,true);"
                            style="left: -49px; width: 25px; position: relative; top: -6px" type="button" /></div>
                </td>
            </tr>
            <tr>
                <td style="height: 58px">
                </td>
                <td style="width: 110px; height: 58px">
                    物料類別</td>
                <td style="width: 219px; height: 58px">
                    <div id="partIdList">
                        &nbsp;<div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                            z-index: 104; left: 3px; overflow-x: hidden; border-left: black 1px solid; width: 205px;
                            border-bottom: black 1px solid; position: relative; top: -13px; height: 75px">
                            <asp:CheckBox ID="chkAlltype" runat="server" AutoPostBack="True" OnCheckedChanged="chkAlltype_CheckedChanged"
                                Text="ALL" Width="100%" />
                            <asp:CheckBoxList ID="lsttype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lsttype_SelectedIndexChanged"
                                Width="100%">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </td>
                <td style="width: 100px; height: 58px">
                    物料料號</td>
                <td style="height: 58px; width: 237px;">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 7px; overflow-x: hidden; border-left: black 1px solid; width: 216px;
                        border-bottom: black 1px solid; position: relative; top: 6px; height: 97px">
                        <asp:CheckBox ID="chkAllpart" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllpart_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstpart" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstpart_SelectedIndexChanged"
                            Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 19px">
                </td>
                <td colspan="1" rowspan="2" style="width: 110px">
                    線別</td>
                <td style="width: 75px;" rowspan="2"><div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                            z-index: 104; left: 3px; overflow-x: hidden; border-left: black 1px solid; width: 202px;
                            border-bottom: black 1px solid; position: relative; top: 7px; height: 85px">
                    <asp:CheckBox ID="chkAllline" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllline_CheckedChanged"
                                Text="ALL" Width="100%" />
                    <asp:CheckBoxList ID="lstline" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstline_SelectedIndexChanged"
                                Width="100%">
                    </asp:CheckBoxList>
                </div>
                </td>
                
                <td style="width: 100px;" rowspan="2">
                    班別</td>
            </tr>        
            <tr>
                <td style="height: 17px">
                </td>
                <td colspan="4" style="height: 17px">
                     <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                            z-index: 104; left: 8px; overflow-x: hidden; border-left: black 1px solid; width: 219px;
                            border-bottom: black 1px solid; position: relative; top: 4px; height: 72px">
                            <asp:CheckBox ID="chkAllshift" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllshift_CheckedChanged"
                                Text="ALL" Width="95%" BorderColor="White" Height="21px" />
                                
                            <asp:CheckBoxList ID="lstshift" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstshift_SelectedIndexChanged"
                                Width="100%">
                            </asp:CheckBoxList>
                     </div>
                 </td>
            </tr>
                      <tr>
                                    <td style="height: 19px">
                                    </td>
                                    <td style="width: 110px; height: 19px">
                                        群 &nbsp;&nbsp; 组</td>
                                    <td style="width: 168px; height: 19px">
                                       <div style ="position:relative;left:3px;top:10px;height:21px ">
                                        <asp:DropDownList ID="ddlApplication" runat="server" Height="22px" Width="154px">
                                        </asp:DropDownList>
                                        </div>
                                
                         <div id="ErrMsg">
                        &nbsp;<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢報表" style="left: 594px; position: relative; top: -11px" />
                        <asp:Label ID="lblErrMsg" runat="server" style="left: -211px; position: relative; top: 29px"></asp:Label>
                    </div>
                    </td>
                </tr>
        </table>
    </form>
</body>
</html>
