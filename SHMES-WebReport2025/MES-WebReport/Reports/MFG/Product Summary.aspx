<%@ Page Language="C#" EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="Product Summary.aspx.cs" Inherits="Report.Reports.MFG.Product_Summary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Product Summary</title>
    <script language="javascript" src="../IEB/popcalendar.js"></script>
	<script language="JavaScript">dateFormat='yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
    <div>
        <div>
            <table id="Table1" align="center" border="0">
                <tr>
                    <td style="height: 48px">
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 501px; height: 6px">
                        <asp:Image ID="Image2" runat="server" Height="4px" ImageUrl="~/Images/line.gif" Width="500px" /></td>
                </tr>
                <tr>
                    <td align="center" style="width: 501px; height: 64px">
                        <p>
                            <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="X-Large" ForeColor="DodgerBlue"
                                Height="24px" Width="468px">Product Summary</asp:Label></p>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 501px; height: 7px">
                        <p style="line-height: 1pt">
                            <asp:Image ID="Image3" runat="server" Height="4px" ImageUrl="~/Images/line.gif" Width="500px" /></p>
                    </td>
                </tr>
            </table>
            <table id="Table3" align="center" border="0">
                <tr>
                    <td align="left" style="width: 247px; height: 18px">
                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="144px">Please Select:</asp:Label></td>
                </tr>
                <tr>
                    <td align="left" style="width: 247px">
                    </td>
                </tr>
            </table>
            <table id="Table4" align="center" border="0" style="width: 244px; height: 210px">
                <tr>
                    <td align="left" style="width: 79px; height: 13px">
                        <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="72px">Frm Time:</asp:Label></td>
                    <td align="center" style="width: 208px; height: 13px">
                        <p align="left">
                            <asp:TextBox ID="txtStartDT" runat="server" Width="124px"></asp:TextBox><input id="Button11"
                                class="CalendarBotton" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)"
                                style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                                background-image: url(../../Images/Button.gif); border-bottom-width: 0px;
                                padding-bottom: 0px; width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px"
                                type="button" />&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 79px; height: 14px">
                        <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="56px">To Time:</asp:Label></td>
                    <td align="center" style="width: 208px; height: 13px">
                        <p align="left">
                            <asp:TextBox ID="TxtEndDT" runat="server" Width="124px"></asp:TextBox><input id="Button22"
                                class="CalendarBotton" onclick="popUpCalendar(this, TxtEndDT, dateFormat,-1,-1,false)"
                                style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                                background-image: url(../../Images/Button.gif); border-bottom-width: 0px;
                                padding-bottom: 0px; width: 25px; padding-top: 0px; height: 17px; border-right-width: 0px"
                                type="button" />&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 79px; height: 26px">
                        <asp:Label ID="Label2" runat="server" Text="Size" Width="72px"></asp:Label></td>
                    <td style="width: 208px; height: 26px">
                        <asp:DropDownList ID="DropSi" runat="server" AppendDataBoundItems="True" Height="21px"
                            Width="160px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="center" style="width: 79px">
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Query" /></td>
                    <td align="center" style="width: 208px">
                        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Reset" /></td>
                </tr>
                <tr>
                    <td style="width: 79px">
                    </td>
                    <td style="width: 208px">
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 79px">
                        <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Red"
                            Height="16px" Width="80px">◆請選擇條件</asp:Label></td>
                    <td style="width: 208px">
                        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Export Excel" Width="86px" /></td>
                </tr>
            </table>
        </div>
        &nbsp; &nbsp;<asp:GridView ID="GridViewtest" runat="server" AutoGenerateColumns="False"
            BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
            CellPadding="4" Font-Size="Smaller" OnRowDataBound="GridViewtest_RowDataBound">
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <RowStyle BackColor="White" ForeColor="#003399" />
            <Columns>
                <asp:BoundField ApplyFormatInEditMode="True" DataField="PRODGROUP" HeaderText="PRODGROUP" />
                <asp:BoundField DataField="PROJECT_NAME" HeaderText="PROJECT_NAME" />
                <asp:BoundField DataField="PRODID" HeaderText="PRODID" />
                <asp:BoundField DataField="COUTPUTQTY" HeaderText="COUTPUTQTY" />
                <asp:BoundField DataField="CSCRAPQTY" HeaderText="CSCRAPQTY" />
                <asp:BoundField DataField="CYIELD" HeaderText="CYIELD" />
                <asp:BoundField DataField="FOUTPUTQTY" HeaderText="FOUTPUTQTY" />
                <asp:BoundField DataField="FSCRAPQTY" HeaderText="FSCRAPQTY" />
                <asp:BoundField DataField="FYIELD" HeaderText="FYIELD" />
                <asp:BoundField DataField="AOUTPUTQTY" HeaderText="AOUTPUTQTY" />
                <asp:BoundField DataField="ASCRAPQTY" HeaderText="ASCRAPQTY" />
                <asp:BoundField DataField="AYIELD" HeaderText="AYIELD" />
                <asp:BoundField DataField="TYIELD" HeaderText="TYIELD" />
            </Columns>
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        </asp:GridView>
    
    </div>
    </form>
</body>
</html>
