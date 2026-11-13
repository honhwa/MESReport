<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IE_MEDIAN.aspx.cs" Inherits="Report.zhongweishu.IE_MEDIAN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>中位數報表</title>
    <script type="text/javascript" language="javascript" src="../popcalendar.js"></script>
    <script type="text/javascript" language="JavaScript">        dateFormat = 'yyyy-mm-dd'</script>
    <style type="text/css">
           #gvMedian
        {
            font-style: normal;
            font-family: Tahoma;
            font-size: 10pt;
            font-weight: 400;
            text-decoration: none;
            unicode-bidi: normal;
            vertical-align: middle; 
            border:1px none Gray;
            background-color: #FFFFFF;
        }
        #gvContent td
        {
            width:80px;
            height:20px;
            white-space:nowrap;
        }
        .style1
        {
            width: 160px;
        }
        .style2
        {
            width: 91px;
        }
        #topInfo
        {
            height: 836px;
            background-color: #FFFFFF;
        }
    </style>
</head>
<body style="background-color: #FFFFFF; height: 900px;">
    <form id="form1" runat="server">
    <div id="topInfo" align="center">
        <asp:Panel ID="Panel1" runat="server" 
            style="color: #0033CC; background-color: White" BackColor="White" 
            Width="1400px">
            <asp:Label ID="Label1" runat="server" BorderColor="White" Font-Names="微軟正黑體" 
                Font-Overline="False" Font-Size="XX-Large" Font-Strikeout="False" 
                ForeColor="#246B61" Text="上海中位數系統-CarUX Median System"
                 
            BackColor="White"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" style="background-color: #C0C0C0" 
            Width="1400px">
            <table border="0.5px" style="height: 34px; width: 1221px;">
                <tr>
                    <td>
                        DATE:
                    </td>
                    <td>
                        <br />
                        <asp:TextBox ID="txtDate" runat="server" Height="18px" Width="120px"
                        onclick="popUpCalendar(this,this, dateFormat,-1,-1,false)" Wrap="False" 
                          ontextchanged="txtDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                        &nbsp;</td>
                         <td>
                             &nbsp;&nbsp;to
                    </td>
                    <td>
                        <br />
                        <asp:TextBox ID="txtDate2" runat="server" Height="18px" Width="120px"
                        onclick="popUpCalendar(this,this, dateFormat,-1,-1,false)" Wrap="False" 
                          ontextchanged="txtDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                        &nbsp;</td>
                    
                    <td class="style2">
                        WORKCTR:
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="ddlWorkctr" runat="server" AutoPostBack="True" 
                        Height="23px" Width="160px" 
                        ontextchanged="txtDate_TextChanged" 
                        OnSelectedIndexChanged="dllWorkctr_SelectIndexChanged">
                            <asp:ListItem Value="">--請選擇--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;LineID:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLineID" runat="server" AutoPostBack="True" 
                        Height="23px" Width="160px" 
                        ontextchanged="txtDate_TextChanged"
                        onselectedindexchanged="ddlLineID_SelectedIndexChanged">
                            <asp:ListItem Value="">--請選擇--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;&nbsp;Prod.ID:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProdid" runat="server" AutoPostBack="True" 
                        Height="23px" Width="160px" 
                        ontextchanged="txtDate_TextChanged" 
                        onselectedindexchanged="ddlProdid_SelectedIndexChanged">
                            <asp:ListItem Value="">--請選擇--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="background-color: #C0C0C0">
                        &nbsp;&nbsp;Prod.SIZE:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProdidSize" runat="server" AutoPostBack="True" 
                    Height="23px" Width="120px"
                    ontextchanged="txtDate_TextChanged"
                    onselectedindexchanged="ddlProdidSize_SelectedIndexChanged">
                            <asp:ListItem Value="">--請選擇--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="查詢" Height="23px" Width="80px" 
                        onclick="btnQuery_Click"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<hr 
            
            style=" height:1px; border-top:5px dotted #185598; width: 1600px; border-left-style: none; border-left-color: inherit; border-left-width: medium; border-right-style: inherit; border-right-color: inherit; border-right-width: medium; border-bottom-style: none; border-bottom-color: inherit; border-bottom-width: medium; margin-left: 0px; margin-top: 0px;" 
            align="center" />
        &nbsp;<asp:Panel ID="Panel3" runat="server" Width="1001px">
        </asp:Panel>
        <asp:GridView ID="gvMedian" runat="server" HorizontalAlign="Center" 
            onrowcreated="gvMedian_RowCreated" CellPadding="4" GridLines="None" 
            Width="1403px" onselectedindexchanged="gvMedian_SelectedIndexChanged" 
            ForeColor="#333333" >
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" 
                HorizontalAlign="Center" />
            <RowStyle HorizontalAlign="Center" BackColor="#E3EAEB"/>
            <SelectedRowStyle BackColor="#C5BBAF" ForeColor="#333333" Font-Bold="True" />
            <%--<SortedAscendingCellStyle BackColor="#F8FAFA" />--%>
            <%--<SortedAscendingHeaderStyle BackColor="#246B61" />--%>
            <%--<SortedDescendingCellStyle BackColor="#D4DFE1" />--%>
            <%--<SortedDescendingHeaderStyle BackColor="#15524A" />--%>
        </asp:GridView>
         <img align="bottom" class="style1" src="1501207380371080313.jpg" /></div>
    </form>
</body>
</html>

