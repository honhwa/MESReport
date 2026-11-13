<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default_BMW.aspx.cs" Inherits="OEE_REPORT.Default_BMW" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<%@ Register TagPrefix="uc" TagName="datepicker"Src="WebUserControl1.ascx" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OEE Report</title>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">dateFormat = 'yyyy/mm/dd'</script>
    <script src="popcalendar.js" type="text/javascript"></script>
    <style type="text/css">
        .auto-style1 {
            width: 93px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">  
    
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering ="true"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="true"   UpdateMode ="always"  >
        
          <ContentTemplate >
          
           <table  width="100%">
            <tr >
            <td align="center" colspan ="4" style="font-size:larger">OEE  Query  Report</td>
            </tr>
            <tr>
            <td align ="center" class="auto-style1" >Time From :</td>
            <td><asp:TextBox ID="txtStartDT" runat="server" Width="115px"></asp:TextBox>
                        <input id="Button11" class="CalendarBotton" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)"
                            style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                            background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                            width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px" type="button" /></td>
            <td align ="center" style="width: 73px">Time To :</td>
            <td><asp:TextBox ID="TxtEndDT" runat="server" Width="115px"></asp:TextBox>
                        <input id="Button22" class="CalendarBotton" onclick="popUpCalendar(this, TxtEndDT, dateFormat,-1,-1,false)"
                            style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                            background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                            width: 25px; padding-top: 0px; height: 17px; border-right-width: 0px" type="button" /></td>
            </tr>
            <tr>
            <td align ="center" class="auto-style1" > SH LCM :</td>
            <td    ><asp:DropDownList ID="ddlfactory" runat ="server"  Width="95%"></asp:DropDownList></td>
            <td align ="center" style="width: 73px">Process :</td>
            <td><asp:DropDownList ID="ddlpro" runat ="server" Width="95%" AutoPostBack="True" OnSelectedIndexChanged="ddlpro_SelectedIndexChanged"></asp:DropDownList></td>
            </tr>            
            <tr>
            <td align ="center" class="auto-style1"> EQ Type :</td>
            <td> <asp:DropDownList ID="ddltype" runat ="server" Width="95%"></asp:DropDownList></td>
            <td align ="center" style="width: 73px">EQ ID :</td>
            <td> <asp:DropDownList ID="ddlid" runat ="server" Width="95%"></asp:DropDownList></td>
            </tr>
            <tr>
            
            <td colspan ="4" align ="center"  ><asp:Button ID="cmdquery" runat ="server" AutoPostBack="True"  Text="Query" OnClick="cmdquery_Click"/></td>
            </tr>
            <tr >
            <td align="center" colspan ="4" style="font-size:larger; color:Red "><asp:Label ID="lblerror" runat ="server" Text="" ></asp:Label></td>
            </tr>
            </table>
       
            </ContentTemplate>
        </asp:UpdatePanel>  &nbsp; &nbsp;&nbsp; &nbsp;
        
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="1593px">
            <LocalReport ReportPath="RDLC\Report_OEE.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
        
    </form>
</body>
</html>

