<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ByCartidQLot.aspx.cs" Inherits="WebReport.Reports.ByCartidQLot" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>By Cartid查詢Lotid</title>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        &nbsp;</div>
        &nbsp;&nbsp;
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <table style="left: 30px; width: 489px; position: relative; top: -156px">
            <tr>
                <td colspan="3" style="height: 28px; text-align: left">
                    <strong><span style="font-size: 25pt">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<span style="font-size: 20pt">&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                            <asp:Label ID="Label3" runat="server" Font-Size="X-Large" Text=" By CartID Query"></asp:Label></span></span></strong></td>
            </tr>
            <tr style="font-size: 12pt">
                <td colspan="3" style="height: 28px">
                    <strong><span>
                        <span style="font-size: 13pt">
                            Please Enter Cartid :</span></span></strong><asp:TextBox ID="TxtCartId" runat="server" Font-Size="Smaller" Width="138px"></asp:TextBox>
                    &nbsp;&nbsp;
                    <asp:Button ID="ButQuery" runat="server" Font-Bold="True" Text="查詢" OnClick="ButQuery_Click" BorderStyle="Ridge" Font-Size="12pt" /><br />
                    <asp:Label ID="LblErr" runat="server" ForeColor="Red" Style="position: relative"
                        Width="397px" Font-Bold="True"></asp:Label>
                    &nbsp; &nbsp; &nbsp;
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="ControlText" Text="lbl1"
                        Visible="False" Width="222px"></asp:Label>
                    &nbsp;<br />
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="lbl2" Visible="False"
                        Width="224px"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="3" style="height: 28px">
                    &nbsp;<table>
                        <tr>
                            <td style="width: 160px">
<%--                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="392px" Width="840px">
                                </rsweb:ReportViewer>--%>
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="392px" Width="840px">
                                </rsweb:ReportViewer>
                            </td>
                        </tr>
                    </table>
                </td>
                <td colspan="1" style="height: 28px">
                </td>
            </tr>
        </table>
        &nbsp;
    </form>
</body>
</html>
