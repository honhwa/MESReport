<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FabNavigator.aspx.cs" Inherits="WebReport.FabNavigator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
</head>

<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Timer ID="TimerPage" runat="server" 
        ontick="TimerPage_Tick" Interval="600000">
    </asp:Timer>
    <asp:Panel ID="pnlTitle" Style="background-color: #99CCFF;" runat="server" 
        GroupingText="Overview opition" >
        <asp:Label ID="lblArea" runat="server" Text="區域/Area:"></asp:Label>
        <asp:DropDownList ID="ddlArea" runat="server">        
            <asp:ListItem Text="Finishing" Value="Finishing" >
            </asp:ListItem>
            <asp:ListItem Text="LAM" Value="LAM" >
            </asp:ListItem>
            <asp:ListItem Text="Assy" Value="Assy" Selected>
            </asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server" Text="樓層/Floor:"></asp:Label>
        <asp:DropDownList ID="ddlFloor" runat="server">
            <asp:ListItem Text="2F" Value="2F">
            </asp:ListItem>
            <asp:ListItem Text="3F" Value="3F">
            </asp:ListItem>
            <asp:ListItem Text="4F" Value="4F">
            </asp:ListItem>
            <asp:ListItem Text="ALL" Value="ALL" Selected>
            </asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnQuery" runat="server" Text="查詢/Query" 
            onclick="btnQuery_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" Text="DateTime:" Visible="true"></asp:Label>
        <asp:Label ID="lbldatetime" runat="server" Visible="true"
            Font-Size="18pt" Font-Bold="True"></asp:Label>
    </asp:Panel>
    <div >
        <asp:GridView ID="gvYieldSummary" runat="server" CellPadding="3" 
            AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
            BorderStyle="None" BorderWidth="1px" 
            onrowdatabound="gvYieldSummary_RowDataBound" Width="1060px">
            <Columns>
                <asp:BoundField DataField="Line" HeaderText="Line" >
                <HeaderStyle Width="120px" /><ItemStyle Wrap="False" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Area" HeaderText="Area" >
                <HeaderStyle Width="80px" /><ItemStyle Wrap="False" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Product" HeaderText="Product" >
                <HeaderStyle Width="100px" /><ItemStyle Wrap="False" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="Yield1" HeaderText="Yield" 
                    DataFormatString="{0:F1}" />
                <asp:BoundField DataField="Output1" HeaderText="Assy Output" />
                <asp:BoundField DataField="StdOutput1" HeaderText="Assy Target Output" />
                <asp:BoundField DataField="Yield2" DataFormatString="{0:F1}" 
                    HeaderText="Module Test Yield" />
                <asp:BoundField DataField="Output2" HeaderText="Module Test Output" />
                <asp:BoundField DataField="StdOutput2" HeaderText="Module Test Target Output" />
                <asp:BoundField DataField="Yield3" DataFormatString="{0:F1}" 
                    HeaderText="OQC Yield" />
                <asp:BoundField DataField="Output3" HeaderText="OQC Output" />
                <asp:BoundField DataField="StdOutput3" HeaderText="OQC Target Output" />

                <asp:BoundField DataField="Yield4" DataFormatString="{0:F1}" 
                    HeaderText="" />
                <asp:BoundField DataField="Output4" HeaderText="" />
                <asp:BoundField DataField="StdOutput4" HeaderText="" />

                <asp:BoundField DataField="Yield5" DataFormatString="{0:F1}" 
                    HeaderText="" />
                <asp:BoundField DataField="Output5" HeaderText="" />
                <asp:BoundField DataField="StdOutput5" HeaderText="" />

                <asp:BoundField DataField="Yield6" DataFormatString="{0:F1}" 
                    HeaderText="" />
                <asp:BoundField DataField="Output6" HeaderText="" />
                <asp:BoundField DataField="StdOutput6" HeaderText="" />

                <%--<asp:HyperLinkField DataNavigateUrlFields="Line" 
                    DataNavigateUrlFormatString="~\OnLineReportAssyVer3.aspx?Line={0}" 
                    DataTextField="Line" HeaderText="Online Detail" Target="_blank" ><HeaderStyle Width="120px" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:HyperLinkField>--%>
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
