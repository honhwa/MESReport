<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MaterielScrapQuery.aspx.cs" Inherits="WebReport.Reports.MaterielScrapQuery" %>

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
                padding-top: 0px; width: 74%; height: 415px;" cellspacing="0" align="left">
                <tr>
                    <td style="background-image: url(../../Images/right_bg.jpg);
                        background-repeat: repeat-y; width: 955px; height: 677px; background-position: center top;" rowspan="2">
                        <div id="filter">
                            <table style="width: 629px; height: 69px;">
                             <tr>
                                <td colspan="4" style="height: 46px">
                                       <strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                           &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                           &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<span style="font-size: 14pt"> 物料報廢查詢</span></strong>
                                  </td>
                               </tr>
                                <tr>
                                    <td style="height: 6px">                                       
                                    </td>
                                    <td style="width: 110px; height: 6px;">                                       
                                            開始日期                                       
                                    </td>
                                    <td style="width: 168px; height: 6px;">
                                        <asp:TextBox ID="txtFrtime" runat="server" Width="148px"></asp:TextBox></td>
                                    <td style="width: 77px; height: 6px;">                                        
                                            結束日期
                                        </td>
                                    <td style="height: 6px">
                                        <asp:TextBox ID="txtToDatatime" runat="server" Width="148px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="height: 19px">
                                        </td>
                                    <td style="width: 110px; height: 19px">
                                        COPM FLAG</td>
                                    <td style="height: 19px; width: 168px;">
                                        <div id="partIdList">
                                            <asp:DropDownList ID="ddlcopmflag" runat="server" Width="154px">
                                                <asp:ListItem>ALL</asp:ListItem>
                                                <asp:ListItem Value="P">Panel</asp:ListItem>
                                                <asp:ListItem Value="M">Material</asp:ListItem>
                                        </asp:DropDownList>&nbsp;</div>
                                    </td>
                                    <td style="width: 77px; height: 19px">
                                        SHOP</td>
                                    <td style="height: 19px">
                                        <asp:DropDownList ID="DdlShop" runat="server" Width="147px">
                                            <asp:ListItem>ALL</asp:ListItem>
                                            <asp:ListItem>CELL2</asp:ListItem>
                                            <asp:ListItem>Module</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="height: 19px">
                                    </td>
                                    <td style="width: 110px; height: 19px">
                                        群 &nbsp;&nbsp; 组</td>
                                    <td style="width: 168px; height: 19px">
                                        <asp:DropDownList ID="ddlApplication" runat="server" Height="22px" Width="154px">
                                        </asp:DropDownList></td>
                                    <td style="width: 77px; height: 19px">
                                        P/E TYPE</td>
                                    <td style="height: 19px">
                                        <asp:DropDownList ID="ddlpetype" runat="server" Height="22px" Width="147px">
                                            <asp:ListItem>ALL</asp:ListItem>
                                            <asp:ListItem>E</asp:ListItem>
                                            <asp:ListItem>P</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="height: 19px">
                                    </td>
                                    <td style="width: 110px; height: 19px">
                                        </td>
                                    <td style="width: 168px; height: 19px">
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
                                            <asp:Label ID="lblErrMsg" runat="server"></asp:Label>&nbsp;</div>
                                    </td>
                                </tr>
                            </table>
                            &nbsp; &nbsp;
                        </div>
                        <div id="report" style="width: 72%; height: 371px; overflow: auto;">                           
                            <%--<rsweb:ReportViewer ID="rvdata" runat="server" ShowFindControls="False"
                                Width="97%" Height="368px">
                            </rsweb:ReportViewer>--%>

                            <rsweb:ReportViewer ID="rvdata" runat="server" ShowFindControls="False"
                                Width="97%" Height="368px">
                            </rsweb:ReportViewer>

                        </div>
                    </td>
                </tr>

            </table>
    </form>
</body>
</html>
