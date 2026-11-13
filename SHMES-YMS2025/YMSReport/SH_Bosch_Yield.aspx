<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SH_Bosch_Yield.aspx.cs" Inherits="YMSReport_SH_Bosch_Yield" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SH Bosch Yield Query Report</title>
    <script type="text/javascript" language="javascript" src="popcalendar.js"></script>
    <script type="text/javascript" language="JavaScript">dateFormat='yyyy-mm-dd'</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="PMain" runat="server" Height="30px" BorderColor="blue" BorderStyle="Solid" BorderWidth="1px" Width="800px">
                    <asp:DropDownList ID="ddlDateType" runat="server" Font-Bold="True" Width="60px" AutoPostBack="True" OnSelectedIndexChanged="ddlDateType_SelectedIndexChanged">
                        <asp:ListItem Selected="True">Date</asp:ListItem>
                        <asp:ListItem>Week</asp:ListItem>
                        <asp:ListItem>Month</asp:ListItem>
                    </asp:DropDownList>From:<asp:TextBox ID="txtFDate" runat="server" Width="80px" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" MaxLength="10"></asp:TextBox><img height="17"
                        onclick="popUpCalendar(this, form1.txtFDate, dateFormat,-1,-1,false)" src="../images/calendar.gif"
                        type="button" width="30" id="imgF" runat="server" /><asp:DropDownList ID="ddlFDate" runat="server" Font-Bold="True" Width="115px" Visible="False">
                    </asp:DropDownList>To:<asp:TextBox ID="txtTDate" runat="server" Width="80px" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" MaxLength="10"></asp:TextBox><img height="17"
                        onclick="popUpCalendar(this, form1.txtTDate, dateFormat,-1,-1,false)" src="../images/calendar.gif"
                        type="button" width="30" id="imgT" runat="server"  /><asp:DropDownList ID="ddlTDate" runat="server" Font-Bold="True" Width="115px" Visible="False">
                    </asp:DropDownList><asp:DropDownList ID="ddlYear" runat="server" Font-Bold="True" Width="60px" Enabled="False">
            </asp:DropDownList>
            Station:<asp:DropDownList ID="ddlStation" runat="server" Font-Bold="True" Width="75px">
                <asp:ListItem>M2</asp:ListItem>
                <asp:ListItem>M3</asp:ListItem>
            </asp:DropDownList><asp:TextBox ID="tbFABID" runat="server" Visible="False" Width="50px"></asp:TextBox>&nbsp;<asp:Button
                ID="btQuery" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                OnClick="btQuery_Click" Text="Query" />&nbsp;<asp:Button ID="btExport" runat="server"
                    Font-Bold="True" Font-Names="Arial" Font-Size="10pt" OnClick="ExportExcel_Click"
                    Text="ExportExcel" />&nbsp;<asp:Button ID="btClear" runat="server" Font-Bold="True"
                        Font-Names="Arial" Font-Size="10pt" OnClick="btClear_Click" Text="Clear" /></asp:Panel>
        <div style="height:5px"></div>
        <asp:Panel ID="Panel1" runat="server" Width="800px">
            <asp:Panel ID="P5" runat="server" Height="80px" Width="800px" ScrollBars="Auto" BorderColor="blue" BorderStyle="Solid" BorderWidth="1px">
                <asp:GridView ID="GridView1" runat="server" CellPadding="1" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" ShowFooter="True">
                    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="PRODUCT_ID" HeaderText="產品號碼">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OPERATION_ID" HeaderText="站點">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="50px" />
                        </asp:BoundField>
                            <asp:BoundField DataField="Qty" HeaderText="Chip_Qty" DataFormatString="{0:N0}">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ARate" HeaderText="A_Rate" DataFormatString="{0:P}">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False"  />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="SRate" HeaderText="S_Rate" DataFormatString="{0:P}">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px"  />
                            </asp:BoundField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
            <div style="height:5px"></div>
            <asp:Panel ID="Panel2" runat="server" Height="200px" Width="800px" ScrollBars="Auto" BorderColor="blue" BorderStyle="Solid" BorderWidth="1px">
                <asp:GridView ID="GridView2" runat="server" CellPadding="1" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" ShowFooter="True">
                    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="PRODUCT_ID" HeaderText="產品號碼">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OPERATION_ID" HeaderText="站點">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LOT_GRADE" HeaderText="等級">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="50px" />
                        </asp:BoundField>
                            <asp:BoundField DataField="DEF_CODE" HeaderText="不良代碼">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DEFECTDESC" HeaderText="不良代碼描述">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="160px" />
                            </asp:BoundField>
                        <asp:BoundField DataField="DefectRate" HeaderText="DefectRate" DataFormatString="{0:P}">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="Qty" HeaderText="DefectQty" DataFormatString="{0:N0}">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                        </asp:BoundField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
            <div style="height:5px"></div>
            <asp:Panel ID="Panel3" runat="server" Height="200px" Width="800px" ScrollBars="Auto" BorderColor="blue" BorderStyle="Solid" BorderWidth="1px">
                <asp:GridView ID="GridView3" runat="server" CellPadding="1" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" ShowFooter="True">
                    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="ReportDate" HeaderText="日期">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PRODUCT_ID" HeaderText="產品號碼">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OPERATION_ID" HeaderText="站點">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="50px" />
                        </asp:BoundField>
                            <asp:BoundField DataField="Qty" HeaderText="Chip_Qty" DataFormatString="{0:N0}">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ARate" HeaderText="A_Rate" DataFormatString="{0:P}">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SRate" HeaderText="S_Rate" DataFormatString="{0:P}">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                            </asp:BoundField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
            <div style="height:5px"></div>
            <asp:Panel ID="Panel4" runat="server" Height="200px" Width="800px" ScrollBars="Auto" BorderColor="blue" BorderStyle="Solid" BorderWidth="1px">
                <asp:GridView ID="GridView4" runat="server" CellPadding="1" Font-Bold="True" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" ShowFooter="True">
                    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="ReportDate" HeaderText="日期">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PRODUCT_ID" HeaderText="產品號碼">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"  Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OPERATION_ID" HeaderText="站點">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LOT_GRADE" HeaderText="等級">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="50px" />
                        </asp:BoundField>
                            <asp:BoundField DataField="DEF_CODE" HeaderText="不良代碼">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DEFECTDESC" HeaderText="不良代碼描述">
                                <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="160px" />
                            </asp:BoundField>
                        <asp:BoundField DataField="DefectRate" HeaderText="DefectRate" DataFormatString="{0:P}">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="Qty" HeaderText="DefectQty" DataFormatString="{0:N0}">
                            <ItemStyle Font-Names="Arial" Font-Size="12px" HorizontalAlign="Left" Wrap="False" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" Width="80px" />
                        </asp:BoundField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
            </asp:Panel>
    </form>
</body>
</html>
