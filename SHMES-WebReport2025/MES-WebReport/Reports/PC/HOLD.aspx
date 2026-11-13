<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HOLD.aspx.cs" Inherits="WebReport.Reports.HOLD" StylesheetTheme="UNICOMTheme" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
</head>
<body background="../../Images/right_bg.jpg"  style="font-size: 12pt">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table>
            <tr>
                <td style="vertical-align: middle; height: 27px" colspan="3">
                    &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                    &nbsp; &nbsp;
                    <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="HOLD 报表"></asp:Label></td>
            </tr>
             <tr>
                <td style="vertical-align: middle; width: 23px; height: 27px">
                </td>
                <td style="width: 67px; height: 22px; text-align: center">
                    產品號碼</td>
                <td style="width: 191px; height: 22px">
                    <div id="Div1">
                        &nbsp; &nbsp;
                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 94px">
                        <asp:CheckBox ID="chkAllProd" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllProd_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstProd" runat="server" AutoPostBack="True"
                            Width="100%">
                        </asp:CheckBoxList>
                       </div>
                </div>
                </td>
                <td style="width: 173px; height: 22px">
                    &nbsp;<asp:Button ID="btnQuery" runat="server" Font-Names="Times New Roman" Font-Size="10pt"
                        OnClick="btnQuery_Click" Text="查詢報表" Width="66px" /></td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 27px">
                </td>
                <td colspan="2" style="height: 15px">
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
     <table style="width: 977px" >
            <tr >
                <td style="width: 21px; height: 808px;">
                </td>
                <td style="width: 803px; height: 808px;">
<%--                    <rsweb:ReportViewer ID="ReportViewer" runat="server" Height="885px" Width="923px">
                    </rsweb:ReportViewer>--%>
                    <br />
                    <rsweb:ReportViewer ID="ReportViewer" runat="server" Height="885px" Width="923px">
                    </rsweb:ReportViewer>
                </td>
        </tr>
          </table>
    </form>
</body>
</html>
