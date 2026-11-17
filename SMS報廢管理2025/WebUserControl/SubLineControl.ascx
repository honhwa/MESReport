<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SubLineControl.ascx.cs" Inherits="WebUserControl_SubLineControl" %>
<style type="text/css">
    .style11
    {
        width: 100%;
        height: 100%;
    }
</style>
<script type="text/javascript">
    function JsSubLineDownClick() { document.getElementById('<%=this.ibSubLineDown.ClientID %>').click(); }
    function JsSubLineUpClick() { document.getElementById('<%=this.ibSubLineUp.ClientID %>').click(); }
</script>
<asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Images/ListBoxDownUp.jpg" 
    Height="412px" HorizontalAlign="Center" Width="198px" BorderStyle="None">
    <asp:Panel ID="pTop" runat="server" BorderStyle="None" Height="18px">
    </asp:Panel>
    <asp:Label ID="lblTitle" runat="server" Font-Names="Microsoft Sans Serif" 
        Font-Size="10pt">SubLine</asp:Label>
    <asp:Panel ID="Panel2" runat="server" BorderStyle="None" Height="10px">
    </asp:Panel>
    <asp:Panel ID="pTop0" runat="server" BorderStyle="None" Height="140px">
        <asp:ListBox ID="lbFromValue" runat="server" 
            Font-Names="Microsoft Sans Serif" Font-Size="10pt" Height="140px" OnDblClick="JsSubLineDownClick()"
            Width="177px" SelectionMode="Multiple">
        </asp:ListBox>
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" BorderStyle="None" Height="48px">
        <table class="style11" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center">
                    <asp:ImageButton ID="ibSubLineDown" runat="server" Height="43px" 
                        ImageUrl="~/Images/DownButtonColor.jpg"
                        Width="43px" onclick="ibSubLineDown_Click" />
                </td>
                <td align="center">
                    <asp:ImageButton ID="ibSubLineUp" runat="server" Height="43px" 
                        ImageUrl="~/Images/UpButtonColor.jpg" 
                        Width="43px" onclick="ibSubLineUp_Click" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" BorderStyle="None" Height="140px">
        <asp:ListBox ID="lbToValue" runat="server" 
            Font-Names="Microsoft Sans Serif" Font-Size="10pt" Height="140px"  OnDblClick="JsSubLineUpClick()"
            Width="180px" SelectionMode="Multiple">
        </asp:ListBox>
    </asp:Panel>
</asp:Panel>
