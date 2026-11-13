<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebEQSM.aspx.cs" Inherits="ESM.WebEQSM" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/WebControl/EQUnit.ascx" TagName="EQUnit" TagPrefix="uc1" %>
<%@ Reference Control="~/WebControl/EQUnit.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    <script language="javascript" type="text/javascript">
function ToLarger()
{
    window.moveTo(0,0);
}
</script>
</head>
<body style="border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid; border-collapse: separate;">


        <form id="Form2" runat="server" >
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table style="border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;
                        width: 939px; border-bottom: black 1px solid; height: 424px">
                        <tr>
                            <td style="height: 179px; width: 80px;">
                                <asp:Panel ID="Panel1" runat="server" Height="400px" Width="80px">
                                </asp:Panel>
                            </td>
                            <td style="width: 80px; height: 179px">
                                <asp:Panel ID="Panel2" runat="server" Height="400px" Width="80px">
                                </asp:Panel>
                            </td>
                            <td style="height: 179px" width="80">
                                <asp:Panel ID="Panel3" runat="server" Height="400px" Width="65px">
                                </asp:Panel>
                            </td>
                            <td style="height: 179px" width="80">
                                <asp:Panel ID="Panel4" runat="server" Height="400px" Width="65px">
                                </asp:Panel>
                            </td>
                            <td style="height: 179px" width="80">
                                <asp:Panel ID="Panel5" runat="server" Height="400px" Width="65px">
                                </asp:Panel>
                            </td>
                            <td style="height: 179px" width="80">
                                <asp:Panel ID="Panel6" runat="server" Height="400px" Width="65px">
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; height: 64px; width: 80px;">
                                <asp:Panel ID="Panel7" runat="server" Height="80px" Width="80px">
                                </asp:Panel>
                            </td>
                            <td style="vertical-align: top; width: 80px; height: 64px">
                                <asp:Panel ID="Panel8" runat="server" Height="80px" Width="80px">
                                </asp:Panel>
                            </td>
                            <td style="vertical-align: top; height: 64px" width="80">
                                <asp:Panel ID="Panel9" runat="server" Height="80px" Width="80px">
                                </asp:Panel>
                            </td>
                            <td style="vertical-align: top; height: 64px" width="80">
                                <asp:Panel ID="Panel10" runat="server" Height="80px" Width="80px">
                                </asp:Panel>
                            </td>
                            <td style="vertical-align: top; height: 64px" width="80">
                                <asp:Panel ID="Panel11" runat="server" Height="80px" Width="80px">
                                </asp:Panel>
                            </td>
                            <td style="vertical-align: top; height: 64px" width="80">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 24px; width: 80px;">
                            </td>
                            <td style="width: 80px; height: 24px">
                            </td>
                            <td style="height: 24px" width="80">
                            </td>
                            <td style="height: 24px" width="80">
                            </td>
                            <td style="height: 24px" width="80">
                            </td>
                            <td style="height: 24px" width="80">
                            </td>
                        </tr>
                    </table>
                    <asp:Timer ID="Timer1" runat="server" Interval="6000" OnTick="Timer1_Tick">
                    </asp:Timer>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">返回</asp:LinkButton>&nbsp;
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    

</body>
</html>
