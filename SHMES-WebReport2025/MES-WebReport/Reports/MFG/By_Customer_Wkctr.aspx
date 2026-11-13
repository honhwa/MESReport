<%@ Page Language="C#"EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="By_Customer_Wkctr.aspx.cs" Inherits="Report.Reports.MFG.By_Customer_Wkctr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>By_Customer_Wkctr</title>

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
                                Height="24px" Width="468px">By Customer&Wkctr</asp:Label></p>
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
                    <td align="left" style="width: 242px; height: 18px">
                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="144px">Please Select:</asp:Label></td>
                </tr>
                <tr>
                    <td align="left" style="width: 242px">
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Show FRENTITYID" Width="152px" Visible="False" /></td>
                </tr>
            </table>
            <table id="Table4" align="center" border="0" style="width: 244px; height: 210px">
                <tr>
                    <td align="left" style="width: 43px; height: 13px">
                        <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="72px">Frm Time:</asp:Label></td>
                    <td align="center" style="width: 888px; height: 13px">
                        <p align="left">
                            <asp:TextBox ID="txtStartDT" runat="server" Width="115px"></asp:TextBox><input id="Button11"
                                class="CalendarBotton" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,true)"
                                style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                                background-image: url(../../Images/Button.gif); border-bottom-width: 0px;
                                padding-bottom: 0px; width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px"
                                type="button" />&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px; height: 14px">
                        <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            Height="16px" Width="56px">To Time:</asp:Label></td>
                    <td align="center" style="width: 888px; height: 14px">
                        <p align="left">
                            <asp:TextBox ID="TxtEndDT" runat="server" Width="115px"></asp:TextBox><input id="Button22"
                                class="CalendarBotton" onclick="popUpCalendar(this, TxtEndDT, dateFormat,-1,-1,true)"
                                style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                                background-image: url(../../Images/Button.gif); border-bottom-width: 0px;
                                padding-bottom: 0px; width: 25px; padding-top: 0px; height: 17px; border-right-width: 0px"
                                type="button" />&nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px; height: 22px">
                        <asp:Label ID="Label2" runat="server" Text="Customer" Width="71px"></asp:Label></td>
                    <td style="width: 888px; height: 22px">
                        <asp:DropDownList ID="DropCs" runat="server" AppendDataBoundItems="True"
                            Width="305px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px">
                        <asp:Label ID="Label4" runat="server" Text="Process" Width="72px"></asp:Label></td>
                    <td align="left" style="width: 888px">
                        <asp:DropDownList ID="DropPs" runat="server" AppendDataBoundItems="True" Width="121px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 43px">
                    </td>
                    <td style="width: 888px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 43px">
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Query" /></td>
                    <td style="width: 888px">
                        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Reset" /></td>
                </tr>
                <tr>
                    <td align="left" style="width: 43px; height: 24px;">
                        <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Red"
                            Height="16px" Width="80px">◆導出Excel</asp:Label></td>
                    <td style="width: 888px; height: 24px;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/xls.jpg" OnClick="ImageButton1_Click"
                            Width="33px" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="font-size: smaller; text-transform: lowercase; color: blue;
                        height: 23px; text-decoration: none">
                        請儘量選擇短期條件，否則會影響你的耐心</td>
                </tr>
            </table>
        </div>
        &nbsp;&nbsp;
        <div style="width:1064px; height:350px; overflow:auto;">
        <asp:GridView ID="GridView1" runat="server" BackColor="White"
            BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="WONBR"
            Font-Size="Smaller" PageSize="60" AllowPaging="True" OnDataBound="GridView1_DataBound" OnPageIndexChanged="GridView1_PageIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging" ShowFooter="True" OnRowCreated="GridView1_RowCreated">
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <RowStyle BackColor="White" ForeColor="#003399" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            
        </asp:GridView>
    </div>
    </div>
    </form>
</body>
</html>
