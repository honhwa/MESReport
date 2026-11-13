<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NonKeyMaterialQuery.aspx.cs" Inherits="WebReport.Reports.NonKeyMaterialQuery" StylesheetTheme="UNICOMTheme" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>NonKeyMaterialQuery</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <table style="width: 831px; height: 162px">
        <tr>
                  <td colspan="11" rowspan="1" style="font-size: 24px; color: black; height: 13px;text-align: center">在製品Nonkey物料查詢</td>
         </tr> 
            <tr>
                                    <td style="width: 19px; height: 42px">
                                        </td>
                                    <td style="width: 96px; height: 42px" align="center">
                                        </td>
                                    <td style="width: 168px; height: 42px; text-align: left">
                                            &nbsp;
                                         </td>
                                        <td style="width: 49px; height: 42px" align="center">
                                            </td>
                                    <td style="width: 61px; height: 42px; text-align: left">
                                            &nbsp;
                                         </td>
 
                     </tr> 
                                
            <tr>
                                    <td style="width: 19px; height: 7px;">
                                        </td>
                                    <td style="width: 70px; height: 7px;" align="center">
                                        &nbsp; &nbsp; &nbsp; &nbsp;站 別</td>
                                    <td style="width: 121px; text-align: left; height: 7px;">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 148px; border-bottom: black 1px solid; top: 78px; height: 58px">
                                            <asp:CheckBox ID="chkAllStations" runat="server" AutoPostBack="True"
                                                Text="ALL" Width="100%" OnCheckedChanged="chkAllStations_CheckedChanged" />
                                            <asp:CheckBoxList ID="lstStations" runat="server"
                                                Width="99%" OnSelectedIndexChanged="lstStations_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                    <td style="width: 70px; height: 7px;" align="center">
                                        機 種</td>
                                    <td style="width: 121px; text-align: left; height: 7px;">
                                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                            width: 151px; border-bottom: black 1px solid; top: 78px; height: 58px">
                                            <asp:CheckBox ID="chkAllProdID" runat="server" AutoPostBack="True"
                                                Text="ALL" Width="100%" OnCheckedChanged="chkAllProdID_CheckedChanged" />
                                            <asp:CheckBoxList ID="lstProdId" runat="server"
                                                Width="99%"></asp:CheckBoxList></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 19px">
                                    </td>
                                    <td style="width: 96px; height: 7px" align="center">
                                        群 组

                                    </td>
                                    <td style="width: 168px; height: 19px">
                                       <div style ="position:relative;left:3px;top:10px;height:21px ">
                                        <asp:DropDownList ID="dlGroup" runat="server" Height="22px" Width="153px" OnSelectedIndexChanged="dlGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        </div>
                                       <div >
                             &nbsp;
                        &nbsp;<asp:Button ID="Button2" runat="server" OnClick="Button1_Click" Text="查  詢" style="left: 280px; position: relative; top: -9px" />&nbsp;
                    </div> 
                                    </td>
                               </tr>   
           <tr>
                                    <td style="width: 19px; height: 26px;">
                                        </td>
                <td colspan="4" style="vertical-align: middle; height: 26px">
                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblErrMsg" runat="server" Width="312px" Visible="False"></asp:Label>             
                    </td>
            </tr>
        </table>
        
        <table >
            <tr >
                <td style="width: 32px; height: 14px;">
                </td>
                <td style="width: 903px">
                    <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="451px" Width="800px">
                    </rsweb:ReportViewer>--%>

                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="451px" Width="800px">
                    </rsweb:ReportViewer>

                    <br />
                </td>
        </tr>
        </table>
        &nbsp;
      </form>
</body>
</html>
