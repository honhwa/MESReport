<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SGradeDfectPanelDQuery.aspx.cs" Inherits="Report.Reports.PC.SGradeDfectPanelDQuery" %>
<%--
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>S Grade Defect Pane ID Query</title>
 <style> 
    .Freezing 
       { 
        
       position:relative ; 
       table-layout:fixed;
       top:expression(this.offsetParent.scrollTop);   
       z-index: 10;
       }

    .Freezing th{text-overflow:ellipsis;overflow:hidden;white-space: nowrap;padding:2px;}

</style>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
     
                    <script type ="text/javascript" src ="popcalendar.js"></script>
                    <script language="javascript" type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body style="font-family: Times New Roman" background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div>
        <div>
            <div>
                <div style="text-align: left">
                    <br />
                    <table style="width: 1018px">
                        <tr>
                            <td>
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="S Grade Defect Pane ID Query"></asp:Label></td>
                        </tr>
                    </table>
                    <br />
                    <table style="font-weight: bold; font-size: 12pt; color: #000000; font-family: Arial">
                        <tr>
                            <%--    ///
                    <td style="width: 111px; height: 30px;">
                        <span style="font-family: Arial"><strong>開始時間</strong> :</span></td>
                        
                    <td style="width: 231px; height: 30px;">
                          < id="timePickerLeftPicker" class="timePicker">
                        <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="132px"></asp:TextBox><input
                            id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,false);"
                            style="width: 27px" type="button" /></td> 
                    <td style="width: 149px; height: 30px">
                        <strong>結束時間</strong></td>
                    <td style="width: 231px; height: 30px">
                        <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="133px"></asp:TextBox>
                        <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,false);"
                            style="width: 24px" type="button" /></td>
                       //--%>
                            <td style="vertical-align: middle; width: 73px; height: 28px; text-align: center">
                                &nbsp; &nbsp; <span style="font-family: Times New Roman">開始時間</span></td>
                            <td style="width: 199px; font-family: Times New Roman; height: 28px">
                                &nbsp;<asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,false);"
                                    type="button" /></td>
                            <td style="width: 149px; font-family: Times New Roman; height: 28px; text-align: center">
                                結束時間</td>
                            <td style="font-size: 12pt; width: 149px; font-family: Times New Roman; height: 28px">
                                &nbsp;<asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox><input
                                    id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,false);"
                                    type="button" /></td>
                        </tr>
                        <tr>
                            <td style="width: 111px; height: 58px; text-align: center">
                                <span style="font-family: Times New Roman">ProdID</span></td>
                            <td style="width: 199px; font-family: Times New Roman; height: 58px">
                                &nbsp;<div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                    z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                    width: 191px; border-bottom: black 1px solid; top: 78px; height: 106px">
                                    <asp:CheckBox ID="chkFrAllProdID" runat="server" AutoPostBack="True" OnCheckedChanged="chkFrAllProdID_CheckedChanged"
                                        Text="ALL" Width="100%" /><asp:CheckBoxList ID="lstFrProdId" runat="server" Width="99%">
                                        </asp:CheckBoxList></div>
                            </td>
                            <td style="width: 149px; height: 58px; text-align: center">
                            <asp:Button ID="ButQ" runat="server" Font-Bold="True" Font-Size="12pt" OnClick="ButQ_Click1"
                                    Text="查詢" /></td>
                            <td style="width: 231px; height: 58px">
                                &nbsp;<asp:Button ID="ButExcel" runat="server" Font-Bold="True" Font-Size="12pt" OnClick="ButExcel_Click"
                                    Text="匯出Excel" /></td>
                        </tr>
   
                    </table>
                    &nbsp;<br />
                    <table style="font-size: 12pt; width: 1018px">
                        <tr>
                            <td style="height: 21px">
                                &nbsp; &nbsp;<asp:Label ID="lblErrMsg" runat="server" Font-Names="Times New Roman"
                                    Font-Size="11pt" ForeColor="Red" Width="142px"></asp:Label>&nbsp;</td>
                        </tr>
                    </table>
                    <asp:Panel ID="PnlGrid" runat="server" Height="380px" HorizontalAlign="Center" ScrollBars="Auto"
                        Style="overflow: auto" Width="871px">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="None" Height="177px" HorizontalAlign="Justify"
                            Visible="False" Width="1200px" Font-Size="X-Small">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="TIMESTAMP" HeaderText="TIMESTAMP" SortExpression="TIMESTAMP">
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GLASSID" HeaderText="GLASSID" SortExpression="GLASSID" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GLASSPRODUCTID" HeaderText="GLASSPRODUCTID" SortExpression="GLASSPRODUCTID" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DEFECTTIMESTAMP" HeaderText="DEFECTTIMESTAMP" SortExpression="DEFECTTIMESTAMP" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PRODUCTID" HeaderText="PRODUCTID" SortExpression="PRODUCTID" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PRODUCTSPECNAME" HeaderText="PRODUCTSPECNAME" SortExpression="PRODUCTSPECNAME" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OPERATIONNAME" HeaderText="OPERATIONNAME" SortExpression="OPERATIONNAME" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOTID" HeaderText="LOTID" SortExpression="LOTID" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOTOUTPUTTIME" HeaderText="LOTOUTPUTTIME" SortExpression="LOTOUTPUTTIME" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PANELID" HeaderText="PANELID" SortExpression="PANELID" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GRADE" HeaderText="GRADE" SortExpression="GRADE" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOTLEVEL" HeaderText="LOTLEVEL" SortExpression="LOTLEVEL" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DEFECTCD" HeaderText="DEFECTCD" SortExpression="DEFECTCD" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UPFLAG" HeaderText="UPFLAG" SortExpression="UPFLAG" >
                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle BackColor="#EFF3FB" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" CssClass="Freezing" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                        &nbsp;&nbsp;</asp:Panel>
                    <br />
                    &nbsp; &nbsp; &nbsp; &nbsp;
                    <br />
                </div>
            </div>
        </div>
    
    </div>
    </form>
</body>
</html>
