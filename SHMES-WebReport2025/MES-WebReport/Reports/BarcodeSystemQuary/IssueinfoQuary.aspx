<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IssueinfoQuary.aspx.cs" Inherits="WebReport.Reports.IssueinfoQuary" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
     <script type ="text/javascript" src ="popcalendar.js"></script>
     <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>

    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body style="POSITION: absolute" background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
     <table style="width: 1018px">
    <tr>
     <td style="height: 29px">
        <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Style="left: 303px;
            position: relative; top: 30px" Text="IssueinfoQuary" Width="160px"></asp:Label>
      </td>
    </tr>
     </table>
      <table style="width: 1004px">
        <tr>
        <td style="width: 294px; height: 27px;">
        <asp:Label ID="Label3" runat="server" Style="left: 10px; position: relative; top: 58px"
            Text="開始日期"></asp:Label>
            <asp:TextBox ID="txtDate" runat="server" Style="left: 10px;
                position: relative; top: 63px" Width="143px"></asp:TextBox>
                <input id="btnFromTime"
                    onclick="popUpCalendar(this, txtDate, dateFormat,-1,-1,false)" style="border-top-width: 0px;
                    padding-right: 0px; padding-left: 0px; border-left-width: 0px; left: 10px;
                    background-image: url(../../images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                    width: 25px; padding-top: 0px; position: relative; top: 63px; height: 17px; border-right-width: 0px"
                    type="button" />
         </td>
         <td style="width: 294px; height: 27px;">
                    <asp:Label ID="Label2" runat="server" Style="left: -66px; position: relative;
                        top: 60px" Text="結束日期"></asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" Style="left: -63px;
                            position: relative; top: 63px" Width="144px"></asp:TextBox>
                            <input id="btnToTime"
                                onclick="popUpCalendar(this, txtToDate, dateFormat,-1,-1,false)" style="border-top-width: 0px;
                                padding-right: 0px; padding-left: 0px; border-left-width: 0px; left: -52px; background-image: url(../../images/Button.gif);
                                border-bottom-width: 0px; padding-bottom: 0px; width: 25px; padding-top: 0px;
                                position: relative; top: 59px; height: 17px; border-right-width: 0px" type="button" />
         </td>
             </tr>    
              </table >
  
                                <%--<rsweb:ReportViewer
                                    ID="ReportViewer1" runat="server" Height="452px" Style="left: 6px; position: relative;
                                    top: 135px" Width="975px">
                                 
                                </rsweb:ReportViewer>--%>
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="452px" Style="left: 6px; position: relative;
                                    top: 135px" Width="975px">
                                </rsweb:ReportViewer>

        <asp:Button ID="Seac_but" runat="server" BackColor="#CCFFFF" BorderStyle="Ridge"
            OnClick="Seac_but_Click" Style="left: 23px; position: relative; top: -380px"
            Text="查詢" Width="75px" />

            </div>
    </form>
</body>
</html>
